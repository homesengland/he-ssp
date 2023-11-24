using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

public class CompanyPurposeCtorTests
{
    [Fact]
    public void ShouldCompanyPurposeAsSpv_WhenStringIsYes()
    {
        // given
        var companyPurposeAsString = CommonResponse.Yes;

        // when
        var companyPurpose = CompanyPurpose.FromString(companyPurposeAsString);

        // then
        companyPurpose.IsSpv.Should().BeTrue();
    }

    [Fact]
    public void ShouldCompanyPurposeAsSpvSetToFalse_WhenStringIsNo()
    {
        // given
        var companyPurposeAsString = CommonResponse.No;

        // when
        var companyPurpose = CompanyPurpose.FromString(companyPurposeAsString);

        // then
        companyPurpose.IsSpv.Should().BeFalse();
    }
}
