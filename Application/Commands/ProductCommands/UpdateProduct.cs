using System.Net;
using System.Text.Json.Serialization;
using Application.Contracts.Repositories;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands.Store.ProductCommands;

public class UpdateProduct
{
    public record UpdateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Quantity
    ) : IRequest<UpdateProductCommandResponse>
    {
        [JsonIgnore] public Guid Id { get; set; } = default;
    }

    public class Handler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
         
            var product = await productRepository.ReadAsync(request.Id);
            if (product is null) throw new DomainException("Product not found.", "InvalidParameter", 404);


            product.Update(
                request.Name,
                request.Description,
                request.Price,
                request.Quantity
            );
            await productRepository.UpdateAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateProductCommandResponse(
                product.Id,
                product.Name,
                product.Description
            );
        }
    }

    public record UpdateProductCommandResponse(
        Guid Id,
        string Name,
        string Description

    );
}