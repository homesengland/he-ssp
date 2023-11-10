using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Messages;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideNameCommandHandlerTests : TestBase<ChangeProjectNameCommandHandler>
{
    private ChangeProjectNameCommand _command;

    public ProvideNameCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidProjectName());

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

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidProjectName());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        (await action.Should().ThrowExactlyAsync<NotFoundException>()).ForEntity(nameof(ApplicationProjects));
    }

    [Fact]
    public async Task ShouldNotChangeName_WhenEmptyNameIsProvided()
    {
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .ReturnsAllProjects(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;
        var oldName = project.Name;

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId!, string.Empty);

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.Name.Should().Be(oldName);
    }

    [Fact]
    public async Task ShouldFail_WhenNameLongerThanShortInputLimitIsProvided()
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

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId!, TextTestData.TextThatExceedsShortInputLimit);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        result.Errors.Should().ContainsOnlyOneErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.ProjectName));
    }

    [Fact]
    public async Task ShouldChangeProjectName_WhenValidNameIsProvided()
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

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId!, ValidProjectName());

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.Name.Should().Be(new ProjectName(TextTestData.TextThatNotExceedsShortInputLimit));
    }

    private string ValidProjectName() => TextTestData.TextThatNotExceedsShortInputLimit;
}
