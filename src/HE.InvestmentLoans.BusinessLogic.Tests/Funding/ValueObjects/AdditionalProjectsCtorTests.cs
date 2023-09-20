using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.ValueObjects;
public class AdditionalProjectsCtorTests
{
    [Fact]
    public void ShouldSetAdditionalProjectsValueToTrue_WhenStringIsYes()
    {
        // given
        var additionalProjectsAsString = CommonResponse.Yes;

        // when
        var additionalProjects = AdditionalProjects.FromString(additionalProjectsAsString);

        // then
        additionalProjects.IsThereAnyAdditionalProject.Should().BeTrue();
    }

    [Fact]
    public void ShouldSetAdditionalProjectsValueToFalse_WhenStringIsNo()
    {
        // given
        var additionalProjectsAsString = CommonResponse.No;

        // when
        var additionalProjects = AdditionalProjects.FromString(additionalProjectsAsString);

        // then
        additionalProjects.IsThereAnyAdditionalProject.Should().BeFalse();
    }
}
