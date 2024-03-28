using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class CanBeCompletedTests
{
    [Fact]
    public void ShouldNotThrowDomainValidationException_WhenProjectCanBeCompleted()
    {
        // given
        var projectEntity = ProjectEntityBuilder
            .New()
            .WithSupportActivities()
            .WithNonSiteQuestionFulfilled()
            .WithIsSupportRequired()
            .WithRequiredFunding()
            .WithIsProfit()
            .WithExpectedStartDate()
            .Build();

        // when
        var action = () => projectEntity.CanBeCompleted();

        // then
        action.Should().NotThrow<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProjectIsMissingSupportActivities()
    {
        // given
        var projectEntity = ProjectEntityBuilder
            .New()
            .WithNonSiteQuestionFulfilled()
            .WithIsSupportRequired()
            .WithRequiredFunding()
            .WithIsProfit()
            .WithExpectedStartDate()
            .Build();

        // when
        var action = () => projectEntity.CanBeCompleted();

        // then
        action.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.ProvideAllProjectAnswers);
    }
}
