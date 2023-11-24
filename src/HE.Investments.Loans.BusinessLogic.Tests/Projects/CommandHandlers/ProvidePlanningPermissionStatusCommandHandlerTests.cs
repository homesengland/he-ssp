using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Projects.Enums;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvidePlanningPermissionStatusCommandHandlerTests : TestBase<ProvidePlanningPermissionStatusCommandHandler>
{
    private ProvidePlanningPermissionStatusCommand _command;

    public ProvidePlanningPermissionStatusCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Theory]
    [InlineData(ProjectFormOption.PlanningPermissionStatusOptions.ReceivedFull, PlanningPermissionStatus.ReceivedFull)]
    [InlineData(ProjectFormOption.PlanningPermissionStatusOptions.OutlineOrConsent, PlanningPermissionStatus.OutlineOrConsent)]
    [InlineData(ProjectFormOption.PlanningPermissionStatusOptions.NotSubmitted, PlanningPermissionStatus.NotSubmitted)]
    [InlineData(ProjectFormOption.PlanningPermissionStatusOptions.NotReceived, PlanningPermissionStatus.NotReceived)]
    public async Task ShouldSetStatus(string statusAsString, PlanningPermissionStatus status)
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
        _command = new ProvidePlanningPermissionStatusCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, statusAsString);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.PlanningPermissionStatus.Should().Be(status);
    }
}
