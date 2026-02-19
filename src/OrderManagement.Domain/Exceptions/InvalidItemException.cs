namespace OrderManagement.Domain.Exceptions;

/// <summary>
/// Thrown when an OrderItem violates invariants (e.g. negative price, non-positive quantity).
/// </summary>
public class InvalidItemException : Exception
{
    public InvalidItemException(string message) : base(message) { }
    public InvalidItemException(string message, Exception inner) : base(message, inner) { }
}
