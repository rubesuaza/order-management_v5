using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.UnitTests;

public class OrderItemTests
{
    [Fact]
    public void Create_WithNegativeUnitPrice_ThrowsInvalidItemException()
    {
        var act = () => OrderItem.Create("Widget", 1, new Money(-1m, "USD"));

        act.Should().Throw<InvalidItemException>()
            .WithMessage("*negative*");
    }

    [Fact]
    public void Create_WithZeroQuantity_ThrowsInvalidItemException()
    {
        var act = () => OrderItem.Create("Widget", 0, Money.Usd(5m));

        act.Should().Throw<InvalidItemException>()
            .WithMessage("*greater than zero*");
    }

    [Fact]
    public void Create_WithNegativeQuantity_ThrowsInvalidItemException()
    {
        var act = () => OrderItem.Create("Widget", -2, Money.Usd(5m));

        act.Should().Throw<InvalidItemException>();
    }

    [Fact]
    public void Create_WithValidArgs_ReturnsOrderItem_WithCorrectLineTotal()
    {
        var item = OrderItem.Create("Widget", 3, Money.Usd(2.50m));

        item.Quantity.Should().Be(3);
        item.UnitPrice.Amount.Should().Be(2.50m);
        item.LineTotal.Amount.Should().Be(7.50m);
    }
}
