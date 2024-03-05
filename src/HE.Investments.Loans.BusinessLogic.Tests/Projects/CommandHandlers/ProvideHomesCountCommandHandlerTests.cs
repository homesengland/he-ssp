using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideHomesCountCommandHandlerTests : TestBase<ProvideHomesCountCommandHandler>
{
    private ProvideHomesCountCommand _command;

    public ProvideHomesCountCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidHomesCount());

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

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidHomesCount());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldFail_WhenHomesCountIsEmpty()
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

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, InvalidHomesCount_EmptyString());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.ManyHomesAmount);
    }

    [Fact]
    public async Task ShouldFail_WhenHomesCountIsTooLong()
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

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, InvalidHomesCount_TooLong());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.ManyHomesAmount);
    }

    [Fact]
    public async Task ShouldFail_WhenHomesCountContainsLetters()
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

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, InvalidHomesCount_WithLetters());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.ManyHomesAmount);
    }

    [Fact]
    public async Task ShouldFail_WhenHomesCountStartsWIthZero()
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

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, InvalidHomesCount_StartWithZero());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.ManyHomesAmount);
    }

    [Fact]
    public async Task ShouldNotFail_WhenValidHomesCountProvided()
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

        _command = new ProvideHomesCountCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, ValidHomesCount());

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.HomesCount.Should().Be(new HomesCount(ValidHomesCount()));
    }

    private string ValidHomesCount() => "13";

    private string InvalidHomesCount_WithLetters() => "jk567";

    private string InvalidHomesCount_StartWithZero() => "0045";

    private string InvalidHomesCount_TooLong() => "68689";

    private string InvalidHomesCount_EmptyString() => string.Empty;
}
