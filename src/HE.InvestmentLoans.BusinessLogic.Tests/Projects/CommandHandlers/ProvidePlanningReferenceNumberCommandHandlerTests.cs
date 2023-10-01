using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Projects.Commands;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class ProvidePlanningReferenceNumberCommandHandlerTests : TestBase<ProvidePlanningReferenceNumberCommandHandler>
{
    private ProvidePlanningReferenceNumberCommand _command;

    public ProvidePlanningReferenceNumberCommandHandlerTests()
    {
        LoanUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        // given
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, CommonResponse.Yes, null!);

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
            .Returns(applicationProjects));

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, CommonResponse.Yes, null!);

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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.No, null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber.Exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldSetExistingPlanningReferenceNumberWithoutAValue()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes, null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber.Exists.Should().BeTrue();
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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvidePlanningReferenceNumberCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes, "number");

        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        project.PlanningReferenceNumber.Should().NotBeNull();

        project.PlanningReferenceNumber.Exists.Should().BeTrue();
        project.PlanningReferenceNumber.Value.Should().Be("number");
    }
}
