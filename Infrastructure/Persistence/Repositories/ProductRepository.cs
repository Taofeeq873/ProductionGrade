using Application.Contracts.Repositories;
using Application.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ProductRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Product> CreateAsync(Product item)
        {
            await _context.Products.AddAsync(item);
            return item;
        }

        public Task DeleteAsync(Product item)
        {
            _context.Products.Remove(item);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products.AnyAsync(predicate);
        }

        public async Task<Product?> GetAsync(Expression<Func<Product, bool>> predicate, bool include = false)
        {
            return await _context.Products
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<Product?> ReadAsync(Guid id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> UpdateAsync(Product item)
        {
            _context.Products.Update(item);
            return await Task.FromResult(item);
        }

        public async Task<List<ProductDto>> GetListAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted) // only active products
                .Where(predicate)
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


        public async Task<bool> TryReserveProductAsync(Guid productId, int quantity)
        {
            var sql = @"
                UPDATE ""Products""
                SET ""AvailableQuantity"" = ""AvailableQuantity"" - @qty
                WHERE ""Id"" = @id AND ""AvailableQuantity"" >= @qty
                RETURNING ""AvailableQuantity"";
            ";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("qty", quantity);
            cmd.Parameters.AddWithValue("id", productId);

            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }
}
