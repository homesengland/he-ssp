using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvidePlanningReferenceNumberCommandHandlerTests : TestBase<ProvidePlanningReferenceNumberCommandHandler>
{
    private ProvidePlanningReferenceNumberCommand _command;

    public ProvidePlanningReferenceNumberCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        // given
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        // when
        _command = new ProvidePlanningReferenceNumberCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            CommonResponse.Yes,
            null!);

        // then
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldFail_WhenProjectCannotBeFound()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithoutProjects()
            .Build();

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .ReturnsAllProjects(applicationProjects));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            CommonResponse.Yes,
            null!);

        // then
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ReferenceNumberShouldRemainEmpty_WhenNoAnswerToExistsIsProvided()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ForProject(projectId)
            .ReturnsOneProject(project));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, null!, null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().BeNull();
    }

    [Fact]
    public async Task ShouldSetNonExistingPlanningReferenceNumber_WhenNoWasProvided()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ForProject(projectId)
            .ReturnsOneProject(project));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.No, null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber!.Exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldSetExistingPlanningReferenceNumberWithoutAValue()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ForProject(projectId)
            .ReturnsOneProject(project));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes, null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber!.Exists.Should().BeTrue();
        project.PlanningReferenceNumber.Value.Should().BeNull();
    }

    [Fact]
    public async Task ShouldSetExistingPlanningReferenceNumberWithAValue()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ForProject(projectId)
            .ReturnsOneProject(project));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes, "number");

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber!.Exists.Should().BeTrue();
        project.PlanningReferenceNumber.Value.Should().Be("number");
    }

    [Fact]
    public async Task ShouldNotChangePlanningReferenceNumber_WhenPlanningReferenceNumberExistAndValueIsNotProvided()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithoutDefaultProject()
            .WithProjectWithPlanningReferenceNumber("number")
            .Build();

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ForProject(projectId)
            .ReturnsOneProject(project));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes, null);

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber!.Exists.Should().BeTrue();
        project.PlanningReferenceNumber.Value.Should().Be("number");
    }
}
