using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.Commands;
using Xunit;
using static HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData.StartDateTestData;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class ProvideStartDateCommandHandlerTests : TestBase<ProvideStartDateCommandHandler>
{
    private ProvideStartDateCommand _command;

    public ProvideStartDateCommandHandlerTests()
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

        var (day, month, year) = CorrectDateAsStrings;
        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, CommonResponse.Yes, day, month, year);

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

        var (day, month, year) = CorrectDateAsStrings;
        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, CommonResponse.Yes, day, month, year);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        (await action.Should().ThrowExactlyAsync<NotFoundException>()).ForEntity(nameof(Project));
    }

    [Fact]
    public async Task ShouldFail_WhenStartDateExistsBuIsIncorrect()
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

        var (day, month, year) = IncorrectDateAsStrings;

        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, project.Id!, CommonResponse.Yes, day, month, year);

        // when
        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.Errors.Should().ContainsOnlyOneErrorMessage(ValidationErrorMessage.InvalidStartDate);
    }

    [Fact]
    public async Task ShouldSetStartDate_WhenCorrectDateIsProvided()
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

        var (day, month, year) = CorrectDateAsStrings;

        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, project.Id!, CommonResponse.Yes, day, month, year);

        // when
        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();

        project.StartDate.Exists.Should().BeTrue();
        project.StartDate.Should().Be(CorrectDate);
    }

    [Fact]
    public async Task StartDateShouldNotExist_WhenNoIsProvided()
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

        var (day, month, year) = CorrectDateAsStrings;

        _command = new ProvideStartDateCommand(LoanApplicationIdTestData.LoanApplicationIdOne, project.Id!, CommonResponse.No, day, month, year);

        // when
        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();

        project.StartDate.Exists.Should().BeFalse();
    }
}
