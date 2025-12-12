using Application.Contracts.Repositories;
using Application.Dtos;
using Domain.Entities;
using Domain.Enum;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries.OrderQueries;

public class ListOrders
{
    public record ListOrdersQuery(OrderStatus? Status)
        : IRequest<List<OrdersResponseBase>>;

    public class ListOrdersQueryHandler(
        IOrderRepository orderRepository
    ) : IRequestHandler<ListOrdersQuery, List<OrdersResponseBase>>
    {
        public async Task<List<OrdersResponseBase>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Order, bool>> predicate = order =>
                request.Status == null || order.Status == request.Status;

            var orders = await orderRepository.GetListAsync(predicate, include: true);

            return orders
                .Select(o => new OrdersResponseBase
                {
                    Id = o.Id,
                    OrderReference = o.OrderReference,
                    CustomerName = o.CustomerName,
                    CustomerNumber = o.CustomerPhoneNumber,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    CreatedAt = o.CreatedAt
                })
                .ToList();
        }
    }
}
