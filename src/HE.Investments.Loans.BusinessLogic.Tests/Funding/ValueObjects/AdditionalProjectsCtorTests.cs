using HE.Investments.Common.Contract.Constants;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.ValueObjects;
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
