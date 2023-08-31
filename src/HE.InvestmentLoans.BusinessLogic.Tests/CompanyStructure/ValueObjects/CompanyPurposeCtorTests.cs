using FluentAssertions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

[TestClass]
public class CompanyPurposeCtorTests
{
    [TestMethod]
    public void ShouldCompanyPurposeAsSpv_WhenStringIsYes()
    {
        // given
        var companyPurposeAsString = CommonResponse.Yes;

        // when
        var companyPurpose = CompanyPurpose.FromString(companyPurposeAsString);

        // then
        companyPurpose.IsSpv.Should().BeTrue();
    }

    [TestMethod]
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
