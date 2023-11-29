using System.ComponentModel;
using FluentAssertions;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Account.WWW.Tests.Utils.ProgrammesTests;

public class GetProgrammeTests : TestBase<Programmes>
{
    [Fact]
    public void ShouldReturnAhpProgramme()
    {
        // given
        var programmeConfig = new ProgrammeUrlConfig { Loans = "https://loans.com", Ahp = "https://ahp.com" };
        RegisterDependency(programmeConfig);

        // when
        var result = TestCandidate.GetProgramme(ProgrammeType.Ahp);

        // then
        result.Name.Should().Be("Affordable Homes Programme 2021-2026 Continuous Market Engagement");
        result.Description.Should()
            .Be("You can start a new Affordable Homes Programme application here. This will not affect any of your previous applications.");
        result.CreateApplicationUrl.Should().Be("https://ahp.com/application/name");
        result.ViewAllApplicationsUrl.Should().Be("https://ahp.com/application");
    }

    [Fact]
    public void ShouldReturnLoansProgramme()
    {
        // given
        var programmeConfig = new ProgrammeUrlConfig { Loans = "https://loans.com", Ahp = "https://ahp.com" };
        RegisterDependency(programmeConfig);

        // when
        var result = TestCandidate.GetProgramme(ProgrammeType.Loans);

        // then
        result.Name.Should().Be("Levelling up Home Building Fund");
        result.Description.Should()
            .Be("You can start a new Levelling Up Home Building Fund application here. This will not affect any of your previous applications.");
        result.CreateApplicationUrl.Should().Be("https://loans.com/application");
        result.ViewAllApplicationsUrl.Should().Be("https://loans.com/dashboard");
    }

    [Fact]
    public void ShouldThrowExceptionWhenProgrammeDoesNotExist()
    {
        // given & when
        Action action = () => TestCandidate.GetProgramme((ProgrammeType)3);

        // then
        action.Should().Throw<InvalidEnumArgumentException>().WithMessage("Programme for 3 does not exist.");
    }
}
