using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class CheckAnswersTests
{
    [Fact]
    public void ThrowValidationError_WhenUndefinedAnswerIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Undefined);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoCheckAnswers);
    }

    [Fact]
    public void ThrowValidationError_WhenStartDateIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideStartDate(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenHomesCountIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideHomesCount(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenProjectTypeIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideProjectType(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenNoHomeTypeIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideHomesTypes(new HomesTypes(Array.Empty<string>(), string.Empty));

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenPlanningReferenceNumberIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvidePlanningReferenceNumber(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenPlanningReferenceExistsAndNoNumberIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvidePlanningReferenceNumber(null);
        project.ProvidePlanningReferenceNumber(new PlanningReferenceNumber(true, null));

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenPlanningReferenceExistsAndPlanningPermissionStatusIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvidePlanningReferenceNumber(new PlanningReferenceNumber(true, "number"));
        project.ProvidePlanningPermissionStatus(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenNoLocationIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideCoordinates(null);
        project.ProvideLandRegistryNumber(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenNoOwnershipInformationIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideLandOwnership(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenApplicantHasFullOwnershipOfTheLandButAdditionalDataNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideLandOwnership(new LandOwnership(true));
        project.ProvideAdditionalData(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_GrantFundingInformationIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideGrantFundingStatus(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_GrantFundingWasReceivedButNoAdditionalInformationIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);
        project.ProvideGrantFundingInformation(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_ChargesDebtIsNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);
        project.ProvideGrantFundingInformation(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ThrowValidationError_WhenAffordableHomesNotProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.ProvideAffordableHomes(null);

        // when
        var action = () => project.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenAllDataIsProvided()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        // when
        project.CheckAnswers(YesNoAnswers.Yes);

        // then
        project.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldChangeStatusBackToInProgress_WhenNoIsSelected()
    {
        // given
        var project = ProjectTestData.ProjectReadyToBeCompleted();

        project.CheckAnswers(YesNoAnswers.Yes);

        // when
        project.CheckAnswers(YesNoAnswers.No);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
