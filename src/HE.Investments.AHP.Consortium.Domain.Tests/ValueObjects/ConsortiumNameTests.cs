using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.ValueObjects;

public class ConsortiumNameTests
{
    [Fact]
    public void ShouldCreateConsortiumName()
    {
        // given
        var programmeName = "Affordable Homes Programme 21-26 Continuous Market Engagement";
        var organisationName = "Carq was here";

        // when
        var result = ConsortiumName.GenerateName(programmeName, organisationName);

        // then
        result.Value.Should().Be("Affordable Homes Programme 21-26 Continuous Market Engagement - Carq was here");
    }

    [Fact]
    public void ShouldThrowValidationErrorWhenProgrammeNameIsEmpty()
    {
        // given
        var programmeName = string.Empty;
        var organisationName = "Carq was here";

        // when
        var act = () => ConsortiumName.GenerateName(programmeName, organisationName);

        // then
        act.Should().Throw<ArgumentException>().And.Message.Should().StartWith("Programme name cannot be empty.");
    }

    [Fact]
    public void ShouldThrowValidationErrorWhenOrganisationNameIsEmpty()
    {
        // given
        var programmeName = "Affordable Homes Programme 21-26 Continuous Market Engagement";
        var organisationName = string.Empty;

        // when
        var act = () => ConsortiumName.GenerateName(programmeName, organisationName);

        // then
        act.Should().Throw<ArgumentException>().And.Message.Should().StartWith("Lead partner name cannot be empty.");
    }
}
