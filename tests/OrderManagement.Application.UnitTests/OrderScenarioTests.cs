using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Application.UnitTests;

/// <summary>
/// Application-layer sanity tests: verify Domain types work when used from Application context.
/// No Application services exist yet; these assert core domain behavior used by future use cases.
/// </summary>
public class OrderScenarioTests
{
    [Fact]
    public void CreateOrder_WithItems_TotalAmountReflectsLineTotals()
    {
        var items = new[]
        {
            OrderItem.Create("Product A", 2, Money.Usd(5.00m)),
            OrderItem.Create("Product B", 1, Money.Usd(12.50m))
        };
        var order = Order.Create(Guid.NewGuid(), items);

        order.TotalAmount.Amount.Should().Be(22.50m);
        order.Status.Should().Be(OrderStatus.Pending);
    }

    [Fact]
    public void Order_CanTransitionPendingToPaidWhenTotalAtLeast10()
    {
        var items = new[] { OrderItem.Create("Item", 1, Money.Usd(10m)) };
        var order = Order.Create(items);

        order.MarkAsPaid();

        order.Status.Should().Be(OrderStatus.Paid);
    }
}
