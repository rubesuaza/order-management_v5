namespace OrderManagement.Domain.Exceptions;

/// <summary>
/// Thrown when monetary operations involve different currencies.
/// </summary>
public class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string message) : base(message) { }
    public CurrencyMismatchException(string message, Exception inner) : base(message, inner) { }
}
