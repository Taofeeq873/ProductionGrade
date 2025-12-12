using Application.Contracts.Repositories;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands.Store.ProductCommands;

public class DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId) : IRequest<DeleteProductCommandResponse>;

    public class Handler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteProductCommand, DeleteProductCommandResponse>
    {
        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await productRepository.GetAsync(p => p.Id == request.ProductId);
            if (product is null) throw new DomainException("Product not found.", "InvalidParameter", 404);


            await productRepository.DeleteAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteProductCommandResponse(product.Id, true);
        }
    }

    public record DeleteProductCommandResponse(Guid ProductId, bool Success);
}