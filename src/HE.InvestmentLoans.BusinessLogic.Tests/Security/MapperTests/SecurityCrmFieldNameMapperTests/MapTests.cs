using HE.InvestmentLoans.BusinessLogic.Security.Mappers;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.MapperTests.SecurityCrmFieldNameMapperTests;

public class MapTests
{
    [Theory]
    [InlineData(SecurityFieldsSet.GetEmpty, "invln_externalstatus,")]
    [InlineData(SecurityFieldsSet.ChargesDebtCompany, "invln_debentureholder,", "invln_outstandinglegalchargesordebt,")]
    [InlineData(SecurityFieldsSet.DirLoans, "invln_directorloans,")]
    [InlineData(SecurityFieldsSet.DirLoansSub, "invln_confirmationdirectorloanscanbesubordinated,", "invln_reasonfordirectorloannotsubordinated,")]
    [InlineData(
        SecurityFieldsSet.GetAllFields,
        "invln_externalstatus,",
        "invln_debentureholder,",
        "invln_outstandinglegalchargesordebt,",
        "invln_directorloans,",
        "invln_confirmationdirectorloanscanbesubordinated,",
        "invln_reasonfordirectorloannotsubordinated,")]
    [InlineData(
        SecurityFieldsSet.SaveAllFields,
        "invln_debentureholder,",
        "invln_outstandinglegalchargesordebt,",
        "invln_directorloans,",
        "invln_confirmationdirectorloanscanbesubordinated,",
        "invln_reasonfordirectorloannotsubordinated,")]
    public void ShouldReturnAllRequiredCrmFieldsWhenSpecificSecurityFieldsSetIsProvided(
        SecurityFieldsSet fieldSet,
        params string[] crmFields)
    {
        // given && when
        var result = SecurityCrmFieldNameMapper.Map(fieldSet);

        // then
        result.Should()
            .Contain($"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}")
            .And.ContainAll(crmFields.Select(field => field));
    }
}
