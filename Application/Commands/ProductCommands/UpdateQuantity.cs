using System.Text.Json.Serialization;
using Application.Contracts.Repositories;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands.Store.ProductCommands;

public class UpdateProductQuantity
{
    public record UpdateProductQuantityCommand(int Quantity) : IRequest<UpdateProductQuantityResponse>
    {
        [JsonIgnore] public Guid ProductId;
    }

    public class Handler : IRequestHandler<UpdateProductQuantityCommand, UpdateProductQuantityResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductQuantityResponse> Handle(UpdateProductQuantityCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(p => p.Id == request.ProductId);
            if (product is null) throw new DomainException("Product not found.", "InvalidParameter", 404);


            product.IncreaseQuantity(request.Quantity);


            product = await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateProductQuantityResponse(product.Id, product.Quantity);
        }
    }

    public record UpdateProductQuantityResponse(Guid ProductId, int Quantity);
}