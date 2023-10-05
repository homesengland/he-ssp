using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using static HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData.LocationTestData;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class ProvideLocationCommandHandlerTests : TestBase<ProvideLocationCommandHandler>
{
    private ProvideLocationCommand _command;

    public ProvideLocationCommandHandlerTests()
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
            .Returns(applicationProjects));

        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, null!, null!, null!);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        (await action.Should().ThrowExactlyAsync<NotFoundException>()).ForEntity(nameof(Project));
    }

    [Fact]
    public async Task ShouldFail_WhenCoordinatesWasChosenAndNoCoordinatesValueWasProvided()
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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, ProjectFormOption.Coordinates, CorrectCoordinates, null!);

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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, ProjectFormOption.LandRegistryTitleNumber, null!, NoLandRegistryNumber);

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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, ProjectFormOption.LandRegistryTitleNumber, null!, CorrectLandRegistryNumber);

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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

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

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        // when
        _command = new ProvideLocationCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, IncorrectLocationType, CorrectCoordinates, CorrectLandRegistryNumber);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotImplementedException>();
    }
}
