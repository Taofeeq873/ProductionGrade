using System.Net;
using Application.Contracts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Queries.Store.ProductQueries;

public class GetProduct
{
    public record GetProductQuery(Guid? ProductId) : IRequest<GetProductQueryResponse>;

    public class Handler(IProductRepository productRepository)
        : IRequestHandler<GetProductQuery, GetProductQueryResponse>
    {
        public async Task<GetProductQueryResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetAsync(x => x.Id == request.ProductId);

            if (product is null)
                throw new DomainException("Product not found", "ProductNotFound", (int)HttpStatusCode.NotFound);

           
            return new GetProductQueryResponse(product.Name, product.Description, product.Price, product.Quantity,
                 product.AvailableQuantity);
        }
    }

    public record GetProductQueryResponse(
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        int AvailableQuantity);
}