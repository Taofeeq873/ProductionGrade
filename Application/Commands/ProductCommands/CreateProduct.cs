using System.Net;
using Application.Contracts.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands.Store.ProductCommands;

public class CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Quantity
    ) : IRequest<CreateProductCommandResponse>;

    public class Handler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            
            var existingProductByName = await productRepository.GetAsync(p => p.Name == request.Name);
            if (existingProductByName is not null)
                throw new DomainException($"A product with the name '{request.Name}' already exists.",
                    "InvalidParameter", 400);


            var product = new Product(
                request.Name,
                request.Description,
                request.Quantity > 0 ? ProductStatus.Available : ProductStatus.OutOfStock,
                request.Price,
                request.Quantity
            );
            await productRepository.CreateAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            return new CreateProductCommandResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Quantity
            );
        }
    }

    public record CreateProductCommandResponse(
        Guid Id,
        string Name,
        string Description,
        int Quantity
 
    );
}