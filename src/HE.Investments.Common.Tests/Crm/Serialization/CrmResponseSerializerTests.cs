using FluentAssertions;
using HE.Investments.Common.CRM.Serialization;
using Xunit;

namespace HE.Investments.Common.Tests.Crm.Serialization;

public class CrmResponseSerializerTests
{
    [Fact]
    public void MapTrueStringToBoolValue()
    {
#pragma warning disable JSON002 // Probable JSON string detected
        var result = CrmResponseSerializer.Deserialize<SerializationResult>("{\"BoolProperty\":\"True\"}");
#pragma warning restore JSON002 // Probable JSON string detected

        result.Should().NotBeNull();
        result!.BoolProperty.Should().BeTrue();
    }

    [Fact]
    public void MapFalseStringToBoolValue()
    {
#pragma warning disable JSON002 // Probable JSON string detected
        var result = CrmResponseSerializer.Deserialize<SerializationResult>("{\"BoolProperty\":\"False\"}");
#pragma warning restore JSON002 // Probable JSON string detected

        result.Should().NotBeNull();
        result!.BoolProperty.Should().BeFalse();
    }
}
