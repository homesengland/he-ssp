using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using Xunit;
using static FluentAssertions.FluentActions;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class CreateProjectCommandHandlerTests : TestBase<CreateProjectCommandHandler>
{
    private readonly CreateProjectCommand _command;

    public CreateProjectCommandHandlerTests()
    {
        _command = new CreateProjectCommand(LoanApplicationIdTestData.LoanApplicationIdOne);

        LoanUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldAddNewProjectToApplication()
    {
        var projects = ApplicationProjectsBuilder.EmptyProjects();

        Given(
            ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(projects));

        await TestCandidate.Handle(_command, CancellationToken.None);

        projects.Projects.Should().HaveCount(1);
    }

    [Fact]
    public async Task ShouldReturnNewProjectId_WhenProjectCreatedSuccessfully()
    {
        var projects = ApplicationProjectsBuilder.EmptyProjects();

        Given(
            ApplicationProjectsRepositoryBuilder
            .New()
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(projects));

        var response = await TestCandidate.Handle(_command, CancellationToken.None);

        projects.Projects.Single().Id.Should().Be(response.Result);
    }
}
