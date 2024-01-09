using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using static HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData.LocationTestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideLocationCommandHandlerTests : TestBase<ProvideLocationCommandHandler>
{
    private ProvideLocationCommand _command;

    public ProvideLocationCommandHandlerTests()
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

        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, null!, null!, null!);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        (await action.Should().ThrowExactlyAsync<NotFoundException>()).ForEntity(nameof(ApplicationProjects));
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

        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, null!, null!, null!);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        (await action.Should().ThrowExactlyAsync<NotFoundException>()).ForEntity(nameof(ApplicationProjects));
    }

    [Fact]
    public async Task ShouldFail_WhenCoordinatesWasChosenAndNoCoordinatesValueWasProvided()
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
        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, ProjectFormOption.Coordinates, NoCoordinates, null!);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainsOnlyOneErrorMessage(ValidationErrorMessage.EnterCoordinates);
    }

    [Fact]
    public async Task ShouldSetCoordinates_WhenCoordinatesWasChosen()
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
        _command = new ProvideLocationCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId,
            ProjectFormOption.Coordinates,
            CorrectCoordinates,
            null!);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.Coordinates!.Value.Should().Be(CorrectCoordinates);
    }

    [Fact]
    public async Task ShouldFail_WhenNumberWasChosenAndNoNumberValueWasProvided()
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
        _command = new ProvideLocationCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId,
            ProjectFormOption.LandRegistryTitleNumber,
            null!,
            NoLandRegistryNumber);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainsOnlyOneErrorMessage(ValidationErrorMessage.EnterLandRegistryTitleNumber);
    }

    [Fact]
    public async Task ShouldSetNumber_WhenNumberWasChosen()
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
        _command = new ProvideLocationCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId,
            ProjectFormOption.LandRegistryTitleNumber,
            null!,
            CorrectLandRegistryNumber);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.LandRegistryTitleNumber!.Value.Should().Be(CorrectLandRegistryNumber);
    }

    [Fact]
    public async Task ShouldChangeNothing_WhenNoChoiceIsSelected()
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
        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, null!, CorrectCoordinates, CorrectLandRegistryNumber);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.LandRegistryTitleNumber.Should().BeNull();
        project.Coordinates.Should().BeNull();
    }

    [Fact]
    public async Task ShouldFail_WhenIncorrectTypeOfLocationIsProvided()
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
        _command = new ProvideLocationCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            projectId,
            IncorrectLocationType,
            CorrectCoordinates,
            CorrectLandRegistryNumber);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotImplementedException>();
    }
}
