using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideChargesDebtCommandHandlerTests : TestBase<ProvideChargesDebtCommandHandler>
{
    private ProvideChargesDebtCommand _command;

    public ProvideChargesDebtCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        _command = new ProvideChargesDebtCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            ValidChargesDebt_NoSelected(),
            ValidChargesDebtInfo());

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

        _command = new ProvideChargesDebtCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            ValidChargesDebt_NoSelected(),
            ValidChargesDebtInfo());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldFail_WhenChargesDebtConfirmedButValueNotProvided()
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

        _command = new ProvideChargesDebtCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId!,
            ValidChargesDebt_YesSelected(),
            InvalidChargesDebtInfo());

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        result.Errors.Should().ContainsOnlyOneErrorMessage(ValidationErrorMessage.EnterExistingLegal);
    }

    [Fact]
    public async Task ShouldNotFail_WhenNoDebtSelectedAndNoDebtInfoProvided()
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

        _command = new ProvideChargesDebtCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId!, ValidChargesDebt_NoSelected(), null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.ChargesDebt.Should().Be(ChargesDebt.From(ValidChargesDebt_NoSelected(), null));
    }

    [Fact]
    public async Task ShouldNotFail_WhenChargesDebtConfirmedAndValidInfoProvided()
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

        _command = new ProvideChargesDebtCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId!,
            ValidChargesDebt_YesSelected(),
            ValidChargesDebtInfo());

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.ChargesDebt.Should().Be(ChargesDebt.From(ValidChargesDebt_YesSelected(), ValidChargesDebtInfo()));
    }

    private string ValidChargesDebt_NoSelected() => CommonResponse.No;

    private string ValidChargesDebt_YesSelected() => CommonResponse.Yes;

    private string ValidChargesDebtInfo() => "Some Debt";

    private string InvalidChargesDebtInfo() => string.Empty;
}
