using Application.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Repositories
{
    public interface IProductRepository : ICrud<Product>
    {
        Task<List<ProductDto>> GetListAsync(Expression<Func<Product, bool>> predicate);
    }
}
