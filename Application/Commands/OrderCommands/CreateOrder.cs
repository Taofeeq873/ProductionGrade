using System.Net;
using System.Text;
using Application.Contracts.Repositories;
using Application.Dtos;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands.OrderCommands;

public class CreateOrder
{
    public record CreateOrderCommand : IRequest<CreateOrderCommandResponse>
    {
        public string CustomerName { get; set; } = default!;
        public string CustomerEmail { get; set; } = default!;
        public string CustomerPhone { get; set; } = default!;
        public string CustomerAddress { get; set; } = default!;
        public List<ProductSelectionDto> Products { get; set; } = new();
    }

    public record ProductSelectionDto(Guid ProductId, int Quantity);

    public class Handler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
    {
        private const decimal ServiceFeePercentage = 0.04m; // 4%

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Products == null || !request.Products.Any())
                throw new DomainException("At least one product must be selected", "NoProductsSelected", (int)HttpStatusCode.BadRequest);

            var order = new Order(OrderStatus.Pending, GenerateOrderReference(), request.CustomerName, request.CustomerEmail, request.CustomerPhone, request.CustomerAddress);

            var itemsHtmlBuilder = new StringBuilder();

            // ✅ 1. Add product to order
            foreach (var selection in request.Products)
            {
                var product = await productRepository.GetAsync(t => t.Id == selection.ProductId);
                if (product == null)
                    throw new DomainException("Product not found", "ProductNotFound", (int)HttpStatusCode.NotFound);

                var reserved = await productRepository.TryReserveProductAsync(selection.ProductId, selection.Quantity);
                if (!reserved)
                    throw new DomainException($"Product '{product.Name}' is out of stock.", "OutOfStock", 400);

                await productRepository.UpdateAsync(product.DecreaseQuantity(selection.Quantity));

                var orderItem = new OrderItem(product.Id, product.Name, product.Price, selection.Quantity);
                order.AddItem(orderItem);

                itemsHtmlBuilder.AppendLine(
                $"<li>{product.Name} x {selection.Quantity} — ₦{product.Price * selection.Quantity:N2}</li>"
                );
            }

            await orderRepository.CreateAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var serviceFee = Math.Round(order.TotalAmount * ServiceFeePercentage, 2);
            var totalPayable = order.TotalAmount + serviceFee;

            order.UpdateStatus(OrderStatus.Completed);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateOrderCommandResponse
            {
                Id = order.Id,
                Message = "Order created successfully.",
                CreatedAt = order.CreatedAt,
                DisplayValue = request.CustomerEmail,
                TotalAmount = order.TotalAmount,
                ServiceFee = serviceFee
            };
        }

        private static string GenerateOrderReference()
        {
            return $"ORD-{DateTime.UtcNow.Ticks % 100:D2}";
        }
    }


    public class CreateOrderCommandResponse : BaseResponse
    {
        public decimal TotalAmount { get; set; }
        public decimal ServiceFee { get; set; }
    }
}
