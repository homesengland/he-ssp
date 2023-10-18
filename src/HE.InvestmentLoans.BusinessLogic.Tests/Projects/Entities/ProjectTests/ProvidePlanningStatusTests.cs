using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Application.Enums;
using Xunit;
using static HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData.PlanningPermissionStatusTestData;
using static HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData.PlanningReferenceNumberTestData;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvidePlanningStatusTests
{
    [Fact]
    public void ShouldFail_WhenPlanningReferenceNumberWasNotProvided()
    {
        var project = new Project();

        var action = () => project.ProvidePlanningPermissionStatus(AnyStatus);

        action.Should().ThrowExactly<DomainException>();
    }

    [Fact]
    public void ShouldFail_WhenProjectHasNoPlanningReferenceNumber()
    {
        var project = new Project();

        project.ProvidePlanningReferenceNumber(NonExistingReferenceNumber);

        var action = () => project.ProvidePlanningPermissionStatus(AnyStatus);

        action.Should().ThrowExactly<DomainException>();
    }

    [Fact]
    public void ShouldSetStatus_WhenPlanningReferenceNumberExists()
    {
        var project = new Project();

        project.ProvidePlanningReferenceNumber(ExistingReferenceNumber);

        project.ProvidePlanningPermissionStatus(AnyStatus);

        project.PlanningPermissionStatus.Should().Be(AnyStatus);
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        // when
        project.ProvidePlanningReferenceNumber(ExistingReferenceNumber);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
