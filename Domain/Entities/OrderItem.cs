namespace Domain.Entities;

public class OrderItem : BaseEntity
{
    private OrderItem() { }

    public OrderItem(
        Guid productId,
        string productName,
        decimal? price,
        int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public decimal? Price { get; private set; }
    public int Quantity { get; private set; }

    public Order Order { get; private set; } = default!;
    public Product Product { get; private set; } = default!;

}