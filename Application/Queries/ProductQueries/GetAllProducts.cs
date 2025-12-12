using System.Linq;
using System.Linq.Expressions;
using Application.Contracts.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Queries.Store.ProductQueries;

public class GetAllProducts
{
    public record GetAllProductsQuery(
        ProductStatus? Status = null
    )
        :IRequest<List<GetAllProductResponse>>;

    public class Handler(IProductRepository productRepository)
        : IRequestHandler<GetAllProductsQuery, List<GetAllProductResponse>>
    {
        public async Task<List<GetAllProductResponse>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            // Build filter expression
            Expression<Func<Product, bool>> predicate = product =>
                request.Status == null || product.Status == request.Status;

            var products = await productRepository.GetListAsync(predicate);

            return products
                .Select(p => new GetAllProductResponse(
                    Id: p.Id,
                    Name: p.ProductName,
                    Description: p.Description,
                    Price: p.Price,
                    Status: p.Status,
                    Quantity: p.Quantity,
                    DateAdded: p.CreatedAt
                ))
                .ToList();
        }
    }

    public record GetAllProductResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string Status,
        int Quantity,
        DateTime DateAdded
    );
}



