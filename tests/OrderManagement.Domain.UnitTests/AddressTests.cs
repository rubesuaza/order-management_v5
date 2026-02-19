using FluentAssertions;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.UnitTests;

public class AddressTests
{
    [Fact]
    public void Create_WithValidFields_StoresValues()
    {
        var address = new Address("123 Main St", "Springfield", "62701", "US");

        address.Street.Should().Be("123 Main St");
        address.City.Should().Be("Springfield");
        address.PostalCode.Should().Be("62701");
        address.Country.Should().Be("US");
    }

    [Fact]
    public void RecordEquality_EqualValues_ReturnsTrue()
    {
        var a = new Address("S", "C", "P", "CO");
        var b = new Address("S", "C", "P", "CO");

        a.Should().Be(b);
    }
}
