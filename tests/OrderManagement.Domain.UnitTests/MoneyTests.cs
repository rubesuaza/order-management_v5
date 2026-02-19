using FluentAssertions;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.UnitTests;

public class MoneyTests
{
    [Fact]
    public void Add_DifferentCurrencies_ThrowsCurrencyMismatchException()
    {
        var usd = Money.Usd(10m);
        var eur = new Money(10m, "EUR");

        var act = () => usd.Add(eur);

        act.Should().Throw<CurrencyMismatchException>()
            .WithMessage("*EUR*USD*");
    }

    [Fact]
    public void Add_SameCurrency_ReturnsSum()
    {
        var a = Money.Usd(5m);
        var b = Money.Usd(3m);

        var result = a.Add(b);

        result.Amount.Should().Be(8m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Subtract_DifferentCurrencies_ThrowsCurrencyMismatchException()
    {
        var usd = Money.Usd(10m);
        var eur = new Money(5m, "EUR");

        var act = () => usd.Subtract(eur);

        act.Should().Throw<CurrencyMismatchException>();
    }

    [Fact]
    public void Multiply_ReturnsCorrectAmount()
    {
        var m = Money.Usd(10m);
        var result = m.Multiply(2m);
        result.Amount.Should().Be(20m);
        result.Currency.Should().Be("USD");
    }
}
