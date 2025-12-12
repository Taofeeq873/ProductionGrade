using Application.Contracts.Repositories;
using Application.Dtos;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Linq.Expressions;


namespace Infrastructure.Persistence.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        public async Task<Order> CreateAsync(Order item)
        {
            await context.Orders.AddAsync(item);
            return item;
        }

        public Task DeleteAsync(Order item)
        {
            context.Orders.Remove(item);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Order, bool>> expression)
        {
            return await context.Orders.AnyAsync(expression);
        }

        public async Task<Order?> GetAsync(Expression<Func<Order, bool>> predicate, bool include = false)
        {
            return await context.Orders
            .AsQueryable()
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(predicate);
        }

        public Task<Order?> ReadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> UpdateAsync(Order item)
        {
            context.Orders.Update(item);
            return await Task.FromResult(item);
        }

        public async Task<List<Order>> GetListAsync(Expression<Func<Order, bool>> predicate, bool include = false)
        {
            IQueryable<Order> query = context.Orders.AsNoTracking();

            if (include)
            {
                query = query
                    .Include(o => o.Items)
                      .ThenInclude(i => i.Product);
            }

            return await query
                .Where(predicate)
                .ToListAsync();
        }

    }

}
