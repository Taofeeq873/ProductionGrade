using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Application.Contracts.Repositories;
using Domain.Exceptions;
using MediatR;
using Domain.Entities;
using Domain.Enum;

namespace Application.Queries.OrderQuery
{
    public class GetOrder
    {
        public record GetOrderQuery(Guid OrderId) : IRequest<GetOrderResponse>;
        public record GetOrderResponse(
                Guid Id,
                string OrderReference,
                OrderStatus Status,
                DateTime CreatedAt,
                ICollection<OrderItemResponse> OrderItems,
                int Quantity,
                decimal ItemPrice,
                string CustomerName,
                string CustomerPhoneNumber,
                string CustomerEmail
        );

        public record OrderItemResponse(
            string productName,
            decimal? UnitPrice,
            int Quantity
        );

        public class Handler(IOrderRepository _orderRepository) : IRequestHandler<GetOrderQuery, GetOrderResponse>
        {
            public async Task<GetOrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
            {
                if (!await _orderRepository.ExistsAsync(x => x.Id == request.OrderId))
                    throw new DomainException("Order not found", "OrderNotFound", (int)HttpStatusCode.NotFound);

                var order = await _orderRepository.GetAsync(x => x.Id == request.OrderId, include: true);

                var totalQuantity = order!.Items.Sum(item => item.Quantity);

                var orderItemsResponse = order.Items.Select(item => new OrderItemResponse(
                    item.Product.Name,
                    item.Product.Price,
                    item.Quantity
                )).ToList();

                return new GetOrderResponse(
                    order!.Id,
                    order.OrderReference,
                    order.Status,
                    order.CreatedAt,
                    orderItemsResponse,
                    totalQuantity,
                    order.TotalAmount,
                    order.CustomerName,
                    order.CustomerEmail,
                    order.CustomerPhoneNumber
                );
            }


        }

    }
}