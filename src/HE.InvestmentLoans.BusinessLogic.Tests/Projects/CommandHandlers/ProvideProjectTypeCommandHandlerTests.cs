using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideProjectTypeCommandHandlerTests : TestBase<ProvideProjectTypeCommandHandler>
{
    private ProvideProjectTypeCommand _command;

    public ProvideProjectTypeCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        _command = new ProvideProjectTypeCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidProjectType());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldFail_WhenProjectCannotBeFound()
    {
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithoutProjects()
            .Build();

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .ReturnsAllProjects(applicationProjects));

        _command = new ProvideProjectTypeCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidProjectType());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldNotFail_WhenValidProjectTypeProvided()
    {
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

        _command = new ProvideProjectTypeCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId!, ValidProjectType());

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.ProjectType.Should().Be(new ProjectType(ValidProjectType()));
    }

    private string ValidProjectType() => "brownfield";
}
