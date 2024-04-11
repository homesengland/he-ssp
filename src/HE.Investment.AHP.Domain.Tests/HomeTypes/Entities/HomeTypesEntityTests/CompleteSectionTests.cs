using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.Entities.HomeTypesEntityTests;

public class CompleteSectionTests
{
    [Fact]
    public void ShouldThrowException_WhenFinishAnswerIsNotDefined()
    {
        // given
        var testCandidate = new HomeTypesEntityBuilder().Build();

        // when
        var complete = () => testCandidate.CompleteSection(FinishHomeTypesAnswer.Undefined, 0);

        // then
        complete.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.GetAllErrors()
            .Should()
            .Be("Select whether you have completed this section");
    }

    [Fact]
    public void ShouldChangeStatusToInProgress_WhenFinishAnswerIsNo()
    {
        // given
        var testCandidate = new HomeTypesEntityBuilder().WithStatus(SectionStatus.Completed).Build();

        // when
        testCandidate.CompleteSection(FinishHomeTypesAnswer.No, 0);

        // then
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
        testCandidate.IsStatusChanged.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenFinishAnswerIsYesAndThereAreNoHomeTypes()
    {
        // given
        var testCandidate = new HomeTypesEntityBuilder().Build();

        // when
        var complete = () => testCandidate.CompleteSection(FinishHomeTypesAnswer.Yes, 1);

        // then
        complete.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.GetAllErrors()
            .Should()
            .Be("Home Types cannot be completed because at least one Home Type needs to be added");
    }

    [Fact]
    public void ShouldThrowException_WhenFinishAnswerIsYesAndOneHomeTypeIsNotCompleted()
    {
        // given
        var homeType = new HomeTypeEntityBuilder().WithName("Milky way").WithStatus(SectionStatus.InProgress).Build();
        var testCandidate = new HomeTypesEntityBuilder().WithHomeTypes(homeType).Build();

        // when
        var complete = () => testCandidate.CompleteSection(FinishHomeTypesAnswer.Yes, 1);

        // then
        complete.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.GetAllErrors()
            .Should()
            .Be("Complete Milky way to save and continue");
    }

    [Fact]
    public void ShouldThrowException_WhenFinishAnswerIsYesAndExpectedNumberOfHomesDoesNotMatch()
    {
        // given
        var homeType1 = new HomeTypeEntityBuilder()
            .WithName("Mercury")
            .WithStatus(SectionStatus.Completed)
            .WithSegments(new HomeInformationBuilder().WithNumberOfHomes(10).Build())
            .Build();
        var homeType2 = new HomeTypeEntityBuilder()
            .WithName("Venus")
            .WithStatus(SectionStatus.Completed)
            .WithSegments(new HomeInformationBuilder().WithNumberOfHomes(5).Build())
            .Build();
        var testCandidate = new HomeTypesEntityBuilder().WithHomeTypes(homeType1, homeType2).Build();

        // when
        var complete = () => testCandidate.CompleteSection(FinishHomeTypesAnswer.Yes, 16);

        // then
        complete.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.GetAllErrors()
            .Should()
            .Be("You have not assigned all of the homes you are delivering to a home type");
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenFinishAnswerIsYesAndAllHomesAreAllocated()
    {
        // given
        var homeType1 = new HomeTypeEntityBuilder()
            .WithName("Mercury")
            .WithStatus(SectionStatus.Completed)
            .WithSegments(new HomeInformationBuilder().WithNumberOfHomes(10).Build())
            .Build();
        var homeType2 = new HomeTypeEntityBuilder()
            .WithName("Venus")
            .WithStatus(SectionStatus.Completed)
            .WithSegments(new HomeInformationBuilder().WithNumberOfHomes(5).Build())
            .Build();
        var testCandidate = new HomeTypesEntityBuilder().WithHomeTypes(homeType1, homeType2).Build();

        // when
        testCandidate.CompleteSection(FinishHomeTypesAnswer.Yes, 15);

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsStatusChanged.Should().BeTrue();
    }
}
