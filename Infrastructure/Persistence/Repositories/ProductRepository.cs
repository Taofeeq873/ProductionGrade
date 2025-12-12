using Application.Contracts.Repositories;
using Application.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        public async Task<Product> CreateAsync(Product item)
        {
            await context.Products.AddAsync(item);
            return item;
        }

        public Task DeleteAsync(Product item)
        {
            context.Products.Remove(item);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Product, bool>> predicate)
        {
            return await context.Products.AnyAsync(predicate);
        }

        public async Task<Product?> GetAsync(Expression<Func<Product, bool>> predicate, bool include = false)
        {
            return await context.Products
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<Product?> ReadAsync(Guid id)
        {
            return await context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> UpdateAsync(Product item)
        {
            context.Products.Update(item);
            return await Task.FromResult(item);
        }

        public async Task<List<ProductDto>> GetListAsync(Expression<Func<Product, bool>> predicate)
        {
            return await context.Products
            .AsNoTracking()
            .Where(p => !p.IsDeleted) // return only active products
            .Where(predicate)         // apply external filter
            .Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.Name,
                Description = p.Description,
                Price = p.Price,
                Status = p.Status.ToString(),
                Quantity = p.Quantity,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
         }

        public Task<List<Product>> GetListAsync(Expression<Func<Product, bool>> predicate, bool include = false)
        {
            throw new NotImplementedException();
        }
    }
}
