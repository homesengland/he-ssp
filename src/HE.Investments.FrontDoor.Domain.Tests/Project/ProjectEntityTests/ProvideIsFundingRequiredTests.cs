using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideIsFundingRequiredTests
{
    [Fact]
    public void ShouldChangeIsFundingRequired_WhenIsFundingRequiredIsProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var isFundingRequired = new IsFundingRequired(true);

        // when
        project.ProvideIsFundingRequired(isFundingRequired);

        // then
        project.IsFundingRequired.Should().Be(isFundingRequired);
    }

    [Fact]
    public void ShouldResetRequiredFundingQuestion_WhenIsFundingRequiredHasChanged()
    {
        // given
        var project = ProjectEntityBuilder.New().WithRequiredFunding().WithIsProfit().Build();

        // when
        project.ProvideIsFundingRequired(new IsFundingRequired(false));

        // then
        project.RequiredFunding.Should().Be(RequiredFunding.Empty);
        project.IsProfit.Should().Be(IsProfit.Empty);
    }

    [Fact]
    public void ShouldNotResetRequiredFundingQuestion_WhenIsFundingRequiredHasChangedToTheSameValue()
    {
        // given
        var project = ProjectEntityBuilder.New().WithRequiredFunding().WithIsProfit().Build();

        // when
        project.ProvideIsFundingRequired(new IsFundingRequired(true));

        // then
        project.RequiredFunding.Should().NotBe(RequiredFunding.Empty);
        project.IsProfit.Should().NotBe(IsProfit.Empty);
    }
}
