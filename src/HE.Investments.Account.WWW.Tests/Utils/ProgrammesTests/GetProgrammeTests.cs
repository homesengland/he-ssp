using System.ComponentModel;
using FluentAssertions;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Account.WWW.Tests.Utils.ProgrammesTests;

public class GetProgrammeTests : TestBase<Programmes>
{
    [Fact]
    public void ShouldReturnAhpProgramme()
    {
        // given
        RegisterDependency(BuildProgrammeConfig("https://loans.com", "https://ahp.com"));
        RegisterDependency(BuildAccountUserContext());

        // when
        var result = TestCandidate.GetProgramme(ProgrammeType.Ahp);

        // then
        result.Name.Should().Be("Affordable Homes Programme Continuous Market Engagement 2021-2026");
        result.Description.Should()
            .Be("Start a new Affordable Homes Programme application. This will not affect any of your previous applications.");
        result.ViewAllAppliancesUrl.Should().Be($"https://ahp.com/{UserAccountTestData.UserAccountOne.Organisation!.OrganisationId}/projects");
    }

    [Fact]
    public void ShouldReturnLoansProgramme()
    {
        // given
        RegisterDependency(BuildProgrammeConfig("https://loans.com", "https://ahp.com"));
        RegisterDependency(BuildAccountUserContext());

        // when
        var result = TestCandidate.GetProgramme(ProgrammeType.Loans);

        // then
        result.Name.Should().Be("Levelling Up Home Building Fund");
        result.Description.Should()
            .Be("Start a new Levelling Up Home Building Fund application. This will not affect any of your previous applications.");
        result.ViewAllAppliancesUrl.Should().Be($"https://loans.com/{UserAccountTestData.UserAccountOne.Organisation!.OrganisationId}/dashboard");
    }

    [Fact]
    public void ShouldThrowExceptionWhenProgrammeDoesNotExist()
    {
        // given
        RegisterDependency(BuildAccountUserContext());

        // when
        var action = () => TestCandidate.GetProgramme((ProgrammeType)3);

        // then
        action.Should().Throw<InvalidEnumArgumentException>().WithMessage("Programme for 3 does not exist.");
    }

    private static ProgrammeUrlConfig BuildProgrammeConfig(string loansUrl, string ahpUrl)
    {
        return new ProgrammeUrlConfig { ProgrammeUrl = new Dictionary<string, string> { { "Loans", loansUrl }, { "Ahp", ahpUrl } } };
    }

    private static IAccountUserContext BuildAccountUserContext()
    {
        var accountUserContext = new Mock<IAccountUserContext>();
        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(UserAccountTestData.UserAccountOne);
        return accountUserContext.Object;
    }
}
