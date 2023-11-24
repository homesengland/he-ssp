using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideLocalAuthoritySearchPhraseCommandHandlerTests : TestBase<ProvideLocalAuthoritySearchPhraseCommandHandler>
{
    [Fact]
    public async Task ShouldBeInValid_WhenPhraseIsNotProvided()
    {
        // given
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        // when
        var action = await TestCandidate.Handle(new ProvideLocalAuthoritySearchPhraseCommand(string.Empty), CancellationToken.None);

        // then
        action.IsValid.Should().BeFalse();
        action.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ShouldBeValid_WhenPhraseIsProvided()
    {
        // given
        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ReturnsNoProjects());

        // when
        var action = await TestCandidate.Handle(new ProvideLocalAuthoritySearchPhraseCommand("phrase"), CancellationToken.None);

        // then
        action.IsValid.Should().BeTrue();
        action.Errors.Count.Should().Be(0);
    }
}
