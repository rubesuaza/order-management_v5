using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.ValueObjects;

/// <summary>
/// Immutable value object for monetary amounts. Throws CurrencyMismatchException when operating with different currencies.
/// </summary>
public sealed record Money(decimal Amount, string Currency)
{
    public static Money Usd(decimal amount) => new(amount, "USD");

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new CurrencyMismatchException($"Cannot add {other.Currency} to {Currency}.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new CurrencyMismatchException($"Cannot subtract {other.Currency} from {Currency}.");
        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor) => new(Amount * factor, Currency);
}
