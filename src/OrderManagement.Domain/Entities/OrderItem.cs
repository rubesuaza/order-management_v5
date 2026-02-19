using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

/// <summary>
/// Entity within the Order aggregate. Quantity > 0 and UnitPrice must not be negative.
/// </summary>
public class OrderItem
{
    public Guid Id { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    private OrderItem() { ProductName = string.Empty; UnitPrice = Money.Usd(0); }

    public static OrderItem Create(string productName, int quantity, Money unitPrice)
    {
        if (quantity <= 0)
            throw new InvalidItemException("Quantity must be greater than zero.");
        if (unitPrice.Amount < 0)
            throw new InvalidItemException("Unit price cannot be negative.");

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }

    public Money LineTotal => UnitPrice.Multiply(Quantity);
}
