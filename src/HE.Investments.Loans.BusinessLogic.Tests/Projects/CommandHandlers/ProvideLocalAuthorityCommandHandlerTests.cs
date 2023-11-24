using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideLocalAuthorityCommandHandlerTests : TestBase<ProvideLocalAuthorityCommandHandler>
{
    public ProvideLocalAuthorityCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldBeNull_WhenLocalAuthorityIsNotProvided()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;
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
        var result = await TestCandidate.Handle(new ProvideLocalAuthorityCommand(loanApplicationId, projectId, null, null), CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        project.LocalAuthority?.Id.Should().BeNull();
        project.LocalAuthority?.Name.Should().BeNull();
    }

    [Fact]
    public async Task ShouldSaveLocalAuthority()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;
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

        var localAuthority = LocalAuthorityTestData.LocalAuthorityOne;

        // when
        var result = await TestCandidate.Handle(new ProvideLocalAuthorityCommand(loanApplicationId, projectId, localAuthority.Id, localAuthority.Name), CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        project.LocalAuthority?.Id.Should().Be(localAuthority.Id);
        project.LocalAuthority?.Name.Should().Be(localAuthority.Name);
    }
}
