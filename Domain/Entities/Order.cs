using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public Order(
        OrderStatus status,
        string orderReference,
        string customerName,
        string customerEmail,
        string customerPhoneNumber,
        string customerAddress)
    {
        Status = status;
        OrderReference = orderReference;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CustomerPhoneNumber = customerPhoneNumber;
        CustomerAddress = customerAddress;
        CreatedAt = DateTime.UtcNow;
    }

    public OrderStatus Status { get; private set; }

    [MaxLength(50)]
    public string OrderReference { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public string CustomerPhoneNumber { get; private set; }
    public string CustomerAddress { get; private set; }
    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

    public decimal TotalAmount => Items.Sum(item => (item.Price ?? 0m) * item.Quantity);

    public Order UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        ModifiedAt = DateTime.UtcNow;
        return this;
    }

    public Order AddItem(OrderItem item)
    {
        Items.Add(item);
        return this;
    }
}