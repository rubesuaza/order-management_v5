namespace OrderManagement.Domain.Ports;

/// <summary>
/// Contract for persisting and retrieving Order aggregates. Implemented by Infrastructure.
/// </summary>
public interface IOrderRepository
{
    Task<Entities.Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task SaveAsync(Entities.Order order, CancellationToken cancellationToken = default);
}
