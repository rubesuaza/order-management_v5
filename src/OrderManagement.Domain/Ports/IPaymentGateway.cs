using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Ports;

/// <summary>
/// Contract for processing payments via external providers. Implemented by Infrastructure.
/// </summary>
public interface IPaymentGateway
{
    Task<bool> ProcessPaymentAsync(Guid orderId, Money amount, CancellationToken cancellationToken = default);
}
