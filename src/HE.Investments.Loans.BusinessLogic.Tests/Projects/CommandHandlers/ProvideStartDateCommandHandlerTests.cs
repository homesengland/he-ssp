using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using static HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData.ProjectDateTestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideStartDateCommandHandlerTests : TestBase<ProvideStartDateCommandHandler>
{
    private ProvideStartDateCommand _command;

    public ProvideStartDateCommandHandlerTests()
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

        var (day, month, year) = CorrectDateAsStrings;
        _command = new ProvideStartDateCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            CommonResponse.Yes,
            new DateDetails(day, month, year));

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

        var (day, month, year) = CorrectDateAsStrings;
        _command = new ProvideStartDateCommand(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            ProjectIdTestData.AnyProjectId,
            CommonResponse.Yes,
            new DateDetails(day, month, year));

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        (await action.Should().ThrowExactlyAsync<NotFoundException>()).ForEntity(nameof(ApplicationProjects));
    }

    [Fact]
    public async Task ShouldFail_WhenStartDateExistsBuIsIncorrect()
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

        var (day, month, year) = IncorrectDateAsStrings;

        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, project.Id, CommonResponse.Yes, new DateDetails(day, month, year));

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage("The project start date must be a real date");
    }

    [Fact]
    public async Task ShouldSetStartDate_WhenCorrectDateIsProvided()
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

        var (year, month, day) = CorrectDateAsStrings;

        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, project.Id, CommonResponse.Yes, new DateDetails(day, month, year));

        // when
        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();

        project.StartDate!.Value.HasValue.Should().BeTrue();
        project.StartDate.Should().Be(StartDateTestData.CorrectDate);
    }

    [Fact]
    public async Task StartDateShouldNotExist_WhenNoIsProvided()
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

        var (day, month, year) = CorrectDateAsStrings;

        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, project.Id, CommonResponse.No, new DateDetails(day, month, year));

        // when
        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();

        project.StartDate!.Value.HasValue.Should().BeFalse();
    }
}
