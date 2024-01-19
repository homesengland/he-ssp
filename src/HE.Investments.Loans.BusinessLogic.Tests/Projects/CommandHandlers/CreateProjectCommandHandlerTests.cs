using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;
public class CreateProjectCommandHandlerTests : TestBase<CreateProjectCommandHandler>
{
    private readonly CreateProjectCommand _command;

    public CreateProjectCommandHandlerTests()
    {
        _command = new CreateProjectCommand(LoanApplicationIdTestData.LoanApplicationIdOne);

        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldAddNewProjectToApplication()
    {
        var projects = ApplicationProjectsBuilder.EmptyProjects();

        Given(
            ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .ReturnsAllProjects(projects));

        await TestCandidate.Handle(_command, CancellationToken.None);

        projects.Projects.Should().HaveCount(1);
    }

    [Fact]
    public async Task ShouldReturnNewProjectId_WhenProjectCreatedSuccessfully()
    {
        var projects = ApplicationProjectsBuilder.EmptyProjects();

        Given(
            ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .ReturnsAllProjects(projects));

        var response = await TestCandidate.Handle(_command, CancellationToken.None);

        projects.Projects.Single().Id.Should().Be(response.ReturnedData);
    }
}
