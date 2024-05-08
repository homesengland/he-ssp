using FluentAssertions;
using HE.Investments.Common.Contract;
using Xunit;

namespace HE.Investments.Common.Tests.Domain.ValueObjects;

public class OrganisationIdTests
{
    [Theory]
    [InlineData("0fa56497-4912-4d58-a15f-0f60c882dbfd", "0fa56497-4912-4d58-a15f-0f60c882dbfd")]
    [InlineData("l2SlDxJJWE2hXw9gyILb_Q", "l2SlDxJJWE2hXw9gyILb_Q")]
    [InlineData("0fa56497-4912-4d58-a15f-0f60c882dbfd", "l2SlDxJJWE2hXw9gyILb_Q")]
    [InlineData("l2SlDxJJWE2hXw9gyILb_Q", "0fa56497-4912-4d58-a15f-0f60c882dbfd")]
    public void ShouldReturnTrue_WhenComparingTheSameOrganisationId(string first, string second)
    {
        // given
        var organisation1 = new OrganisationId(first);
        var organisation2 = new OrganisationId(second);

        // when
        var result = organisation1 == organisation2;

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenComparingDifferentEncodedGuids()
    {
        // given
        var organisation1 = OrganisationId.From(Guid.NewGuid());
        var organisation2 = OrganisationId.From(Guid.NewGuid());

        // when
        var result = organisation1 == organisation2;

        // then
        result.Should().BeFalse();
    }
}
