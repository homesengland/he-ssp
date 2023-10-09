using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class ProvidePlanningPermissionStatusCommandHandlerTests : TestBase<ProvidePlanningPermissionStatusCommandHandler>
{
    private ProvidePlanningPermissionStatusCommand _command;

    public ProvidePlanningPermissionStatusCommandHandlerTests()
    {
        LoanUserContextTestBuilder.New().Register(this);
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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvidePlanningPermissionStatusCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, statusAsString);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.PlanningPermissionStatus.Should().Be(status);
    }
}
