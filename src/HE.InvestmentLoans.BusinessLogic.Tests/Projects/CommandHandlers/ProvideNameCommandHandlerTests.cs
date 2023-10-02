using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class ProvideNameCommandHandlerTests : TestBase<ChangeProjectNameCommandHandler>
{
    private ChangeProjectNameCommand _command;

    public ProvideNameCommandHandlerTests()
    {
        LoanUserContextTestBuilder.New().Register(this);
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
            .Returns(applicationProjects));

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, ProjectIdTestData.AnyProjectId, ValidProjectName());

        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
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
            .Returns(applicationProjects));

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

        var projectId = applicationProjects.Projects.Single().Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

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
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        _command = new ChangeProjectNameCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId!, ValidProjectName());

        await TestCandidate.Handle(_command, CancellationToken.None);

        project.Name.Should().Be(new ProjectName(TextTestData.TextThatNotExceedsShortInputLimit));
    }

    private string ValidProjectName() => TextTestData.TextThatNotExceedsShortInputLimit;
}
