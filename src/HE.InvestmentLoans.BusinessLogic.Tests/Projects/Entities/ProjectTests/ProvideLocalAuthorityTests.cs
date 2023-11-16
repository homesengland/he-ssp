using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using HE.Investments.Common.Domain;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;

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
