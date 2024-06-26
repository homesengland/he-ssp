using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideHomesTypesCommandHandlerTests : TestBase<ProvideHomesTypesCommandHandler>
{
    private ProvideHomesTypesCommand _command;

    public ProvideHomesTypesCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenApplicationProjectsCannotBeFound()
    {
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        _command = new ProvideHomesTypesCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            ValidHomesTypes_WithoutOtherSelected(),
            string.Empty);

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

        _command = new ProvideHomesTypesCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            ValidHomesTypes_WithoutOtherSelected(),
            string.Empty);

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldFail_WhenOtherHomesTypesIsSelectedButValueNotProvided()
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

        _command = new ProvideHomesTypesCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId,
            ValidHomesTypes_WithOtherSelected(),
            InvalidOtherHomesTypes());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.TypeHomesOtherType);
    }

    [Fact]
    public async Task ShouldNotFail_WhenValidHomesTypesWithoutOtherSelected()
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

        _command = new ProvideHomesTypesCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, ValidHomesTypes_WithoutOtherSelected(), null!);

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.HomesTypes.Should().NotBeNull();
        project.HomesTypes?.HomesTypesValue.Length.Should().Be(ValidHomesTypes_WithoutOtherSelected().Length);
        project.HomesTypes?.HomesTypesValue[0].Should().Be(ValidHomesTypes_WithoutOtherSelected()[0]);
        project.HomesTypes?.HomesTypesValue[1].Should().Be(ValidHomesTypes_WithoutOtherSelected()[1]);
        project.HomesTypes?.OtherHomesTypesValue.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotFail_WhenValidHomesTypesWithOtherProvided()
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

        _command = new ProvideHomesTypesCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId,
            ValidHomesTypes_WithOtherSelected(),
            ValidOtherHomesTypes());

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.HomesTypes.Should().NotBeNull();
        project.HomesTypes?.HomesTypesValue.Length.Should().Be(ValidHomesTypes_WithOtherSelected().Length);
        project.HomesTypes?.HomesTypesValue[0].Should().Be(ValidHomesTypes_WithOtherSelected()[0]);
        project.HomesTypes?.HomesTypesValue[1].Should().Be(ValidHomesTypes_WithOtherSelected()[1]);
        project.HomesTypes?.OtherHomesTypesValue.Should().Be(ValidOtherHomesTypes());
    }

    private string[] ValidHomesTypes_WithoutOtherSelected() => ["bungalows", "apartments"];

    private string[] ValidHomesTypes_WithOtherSelected() => ["bungalows", "other"];

    private string ValidOtherHomesTypes() => "ValidOtherHomeType";

    private string InvalidOtherHomesTypes() => string.Empty;
}
