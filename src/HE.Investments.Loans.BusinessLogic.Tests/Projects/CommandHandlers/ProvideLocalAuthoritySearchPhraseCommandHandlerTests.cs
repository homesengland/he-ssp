using HE.Investments.Common.Contract.Exceptions;
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
        var action = () => TestCandidate.Handle(new ProvideLocalAuthoritySearchPhraseCommand(string.Empty), CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage("Enter the name of the local authority");
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
