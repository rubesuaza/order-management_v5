using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.UnitTests;

public class OrderTests
{
    [Fact]
    public void Create_WithNoItems_ThrowsInvalidOrderStateException()
    {
        var act = () => Order.Create(Array.Empty<OrderItem>());

        act.Should().Throw<InvalidOrderStateException>()
            .WithMessage("*at least one item*");
    }

    [Fact]
    public void Create_WithNullItems_Throws()
    {
        var act = () => Order.Create(null!);

        act.Should().Throw<InvalidOrderStateException>();
    }

    [Fact]
    public void TotalAmount_EqualsSumOfUnitPriceTimesQuantity()
    {
        var items = new[]
        {
            OrderItem.Create("A", 2, Money.Usd(3m)),
            OrderItem.Create("B", 1, Money.Usd(4m))
        };
        var order = Order.Create(items);

        order.TotalAmount.Amount.Should().Be(10m); // 2*3 + 1*4
    }

    [Fact]
    public void MarkAsPaid_WhenTotalLessThan10_ThrowsInvalidOrderStateException()
    {
        var items = new[] { OrderItem.Create("X", 1, Money.Usd(5m)) };
        var order = Order.Create(items);

        var act = () => order.MarkAsPaid();

        act.Should().Throw<InvalidOrderStateException>()
            .WithMessage("*10*");
    }

    [Fact]
    public void MarkAsPaid_WhenTotalAtLeast10_Succeeds()
    {
        var items = new[] { OrderItem.Create("X", 1, Money.Usd(10m)) };
        var order = Order.Create(items);

        order.MarkAsPaid();

        order.Status.Should().Be(Domain.Enums.OrderStatus.Paid);
    }

    [Fact]
    public void Cancel_WhenShipped_ThrowsInvalidOrderStateException()
    {
        var items = new[] { OrderItem.Create("X", 2, Money.Usd(10m)) };
        var order = Order.Create(items);
        order.MarkAsPaid();
        order.Ship();

        var act = () => order.Cancel();

        act.Should().Throw<InvalidOrderStateException>()
            .WithMessage("*PENDING*PAID*");
    }

    [Fact]
    public void Cancel_FromPending_Succeeds()
    {
        var items = new[] { OrderItem.Create("X", 1, Money.Usd(10m)) };
        var order = Order.Create(items);

        order.Cancel();

        order.Status.Should().Be(Domain.Enums.OrderStatus.Cancelled);
    }

    [Fact]
    public void Ship_FromPaid_Succeeds()
    {
        var items = new[] { OrderItem.Create("X", 1, Money.Usd(10m)) };
        var order = Order.Create(items);
        order.MarkAsPaid();

        order.Ship();

        order.Status.Should().Be(Domain.Enums.OrderStatus.Shipped);
    }

    [Fact]
    public void Ship_FromPending_ThrowsInvalidOrderStateException()
    {
        var items = new[] { OrderItem.Create("X", 1, Money.Usd(10m)) };
        var order = Order.Create(items);

        var act = () => order.Ship();

        act.Should().Throw<InvalidOrderStateException>()
            .WithMessage("*PAID*");
    }
}
