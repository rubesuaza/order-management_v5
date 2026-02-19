namespace OrderManagement.Domain.ValueObjects;

/// <summary>
/// Immutable value object for shipping and billing locations.
/// </summary>
public sealed record Address(
    string Street,
    string City,
    string PostalCode,
    string Country
);
