using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record ProductDto
    {
        public Guid Id { get; init; }
        public string ProductName { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public string Status { get; init; }
        public int Quantity { get; init; }
        public DateTime CreatedAt { get; init; }
    }

}
