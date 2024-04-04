using System.ComponentModel;
using FluentAssertions;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.Account.WWW.Tests.Utils.ProgrammesTests;

public class GetProgrammeTests : TestBase<Programmes>
{
    [Fact]
    public void ShouldReturnAhpProgramme()
    {
        // given
        RegisterDependency(BuildProgrammeConfig("https://loans.com", "https://ahp.com"));

        // when
        var result = TestCandidate.GetProgramme(ProgrammeType.Ahp);

        // then
        result.Name.Should().Be("Affordable Homes Programme 2021-2026 Continuous Market Engagement");
        result.Description.Should()
            .Be("Start a new Affordable Homes Programme application. This will not affect any of your previous applications.");
        result.ViewAllApplicationsUrl.Should().Be("https://ahp.com/application");
    }

    [Fact]
    public void ShouldReturnLoansProgramme()
    {
        // given
        RegisterDependency(BuildProgrammeConfig("https://loans.com", "https://ahp.com"));

        // when
        var result = TestCandidate.GetProgramme(ProgrammeType.Loans);

        // then
        result.Name.Should().Be("Levelling Up Home Building Fund");
        result.Description.Should()
            .Be("Start a new Levelling Up Home Building Fund application. This will not affect any of your previous applications.");
        result.ViewAllApplicationsUrl.Should().Be("https://loans.com/dashboard");
    }

    [Fact]
    public void ShouldThrowExceptionWhenProgrammeDoesNotExist()
    {
        // given & when
        var action = () => TestCandidate.GetProgramme((ProgrammeType)3);

        // then
        action.Should().Throw<InvalidEnumArgumentException>().WithMessage("Programme for 3 does not exist.");
    }

    private static ProgrammeUrlConfig BuildProgrammeConfig(string loansUrl, string ahpUrl)
    {
        return new ProgrammeUrlConfig { ProgrammeUrl = new Dictionary<string, string> { { "Loans", loansUrl }, { "Ahp", ahpUrl } } };
    }
}
