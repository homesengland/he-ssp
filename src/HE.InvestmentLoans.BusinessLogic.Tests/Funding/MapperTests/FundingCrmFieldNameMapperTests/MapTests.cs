using HE.InvestmentLoans.BusinessLogic.Funding.Mappers;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.MapperTests.FundingCrmFieldNameMapperTests;

public class MapTests
{
    [Theory]
    [InlineData(FundingFieldsSet.GetEmpty, "invln_externalstatus,")]
    [InlineData(FundingFieldsSet.GDV, "invln_projectgdv")]
    [InlineData(FundingFieldsSet.EstimatedTotalCosts, "invln_projectestimatedtotalcost")]
    [InlineData(FundingFieldsSet.PrivateSectorFunding, "invln_privatesectorapproach", "invln_privatesectorapproachinformation")]
    [InlineData(FundingFieldsSet.AbnormalCosts, "invln_projectabnormalcosts", "invln_projectabnormalcostsinformation")]
    [InlineData(FundingFieldsSet.RepaymentSystem, "invln_refinancerepayment", "invln_refinancerepaymentdetails")]
    [InlineData(FundingFieldsSet.AdditionalProjects, "invln_additionalprojects")]
    [InlineData(
        FundingFieldsSet.GetAllFields,
        "invln_externalstatus,",
        "invln_projectgdv",
        "invln_projectestimatedtotalcost",
        "invln_privatesectorapproach",
        "invln_privatesectorapproachinformation",
        "invln_projectabnormalcosts",
        "invln_projectabnormalcostsinformation",
        "invln_refinancerepayment",
        "invln_refinancerepaymentdetails",
        "invln_additionalprojects")]
    [InlineData(
        FundingFieldsSet.SaveAllFields,
        "invln_projectgdv",
        "invln_projectestimatedtotalcost",
        "invln_privatesectorapproach",
        "invln_privatesectorapproachinformation",
        "invln_projectabnormalcosts",
        "invln_projectabnormalcostsinformation",
        "invln_refinancerepayment",
        "invln_refinancerepaymentdetails",
        "invln_additionalprojects")]
    public void ShouldReturnAllRequiredCrmFieldsWhenSpecificFundingFieldsSetIsProvided(
        FundingFieldsSet fieldSet,
        params string[] crmFields)
    {
        // given && when
        var result = FundingCrmFieldNameMapper.Map(fieldSet);

        // then
        result.Should()
            .ContainAll(crmFields.Select(field => field))
            .And.Contain($"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }
}
