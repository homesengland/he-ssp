using FluentAssertions;
using HE.Investments.Common.CRM.Serialization;
using Xunit;

namespace HE.Investments.Common.Tests.Crm.Serialization;

public class CrmResponseSerializerTests
{
    [Fact]
    public void MapTrueStringToBoolValue()
    {
        var result = CrmResponseSerializer.Deserialize<SerializationResult>("{\"BoolProperty\":\"True\"}");

        result.Should().NotBeNull();
        result!.BoolProperty.Should().BeTrue();
    }

    [Fact]
    public void MapFalseStringToBoolValue()
    {
        var result = CrmResponseSerializer.Deserialize<SerializationResult>("{\"BoolProperty\":\"False\"}");

        result.Should().NotBeNull();
        result!.BoolProperty.Should().BeFalse();
    }
}
