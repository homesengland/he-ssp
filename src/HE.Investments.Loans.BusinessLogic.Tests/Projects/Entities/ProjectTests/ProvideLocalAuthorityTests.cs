extern alias Org;

using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using Xunit;
using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Entities.ProjectTests;

public class ProvideLocalAuthorityTests
{
    [Fact]
    public void ShouldSetLocalAuthority()
    {
        // given
        var project = new Project();

        var localAuthority = LocalAuthority.New("1", "York");

        // when
        project.ProvideLocalAuthority(localAuthority);

        // then
        project.LocalAuthority.Should().Be(localAuthority);
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        var localAuthority = LocalAuthority.New("1", "York");

        // when
        project.ProvideLocalAuthority(localAuthority);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
