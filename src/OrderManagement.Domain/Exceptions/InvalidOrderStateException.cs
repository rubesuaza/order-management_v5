namespace OrderManagement.Domain.Exceptions;

/// <summary>
/// Thrown when an illegal order status transition is attempted.
/// </summary>
public class InvalidOrderStateException : Exception
{
    public InvalidOrderStateException(string message) : base(message) { }
    public InvalidOrderStateException(string message, Exception inner) : base(message, inner) { }
}
