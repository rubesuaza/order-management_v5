namespace OrderManagement.Domain.Exceptions;

/// <summary>
/// Thrown when an entity (e.g. Order) is not found by id.
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message) { }
    public EntityNotFoundException(string message, Exception inner) : base(message, inner) { }
}
