#pragma warning disable JSON002 // Probable JSON string detected
using FluentAssertions;
using HE.Investments.Loans.Common.CrmCommunication.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Loans.Common.Tests.CrmCommunication;

[TestClass]
public class CrmResponseSerializerTests
{
    [TestMethod]
    public void MapTrueStringToBoolValue()
    {
        var result = CrmResponseSerializer.Deserialize<SerializationResult>("{\"BoolProperty\":\"True\"}");

        result.Should().NotBeNull();
        result!.BoolProperty.Should().BeTrue();
    }

    [TestMethod]
    public void MapFalseStringToBoolValue()
    {
        var result = CrmResponseSerializer.Deserialize<SerializationResult>("{\"BoolProperty\":\"False\"}");

        result.Should().NotBeNull();
        result!.BoolProperty.Should().BeFalse();
    }
}
#pragma warning restore JSON002 // Probable JSON string detected
