using Domain.Enum;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record OrdersResponseBase
    {
        public Guid Id { get; set; }
        public string? OrderReference { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        

        public string? OrderDetails { get; set; } = null;

        public string? CustomerName { get; set; } = null;
        public string? CustomerNumber { get; set; } = null;
    }

    public record OrdersResponse<T> : OrdersResponseBase
    {
        public ICollection<T> OrderItems { get; set; }
    }

}   
