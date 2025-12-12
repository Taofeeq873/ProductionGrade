using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string name,
        string description,
        ProductStatus status,
        decimal price,
        int quantity
        )
        {
            Name = name;
            Description = description;
            Status = status;
            Price = price;
            Quantity += quantity;
            AvailableQuantity += quantity;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public ProductStatus Status { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public int AvailableQuantity { get; private set; }

        public Product UpdateStatus(ProductStatus newStatus)
        {
            Status = newStatus;
            ModifiedAt = DateTime.UtcNow;
            return this;
        }

        public Product IncreaseQuantity(int quantity)
        {
            AvailableQuantity += quantity;
            ModifiedAt = DateTime.UtcNow;
            return this;
        }

        public Product DecreaseQuantity(int quantity)
        {
            if (AvailableQuantity < quantity) throw new ArgumentException("Product Out of Stock");
            AvailableQuantity -= quantity;
            if (AvailableQuantity == 0) Status = ProductStatus.OutOfStock;
            ModifiedAt = DateTime.UtcNow;
            return this;
        }

        public void Update(string name, string description, decimal price, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity += quantity;
            AvailableQuantity += quantity;
            if (AvailableQuantity > 0)
            {
                Status = ProductStatus.Available;
            }
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
