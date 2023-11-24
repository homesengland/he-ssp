using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.BusinessLogic.Security.Mappers;
using HE.Investments.Loans.Common.Utils.Enums;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Security.MapperTests.SecurityCrmFieldNameMapperTests;

public class MapTests
{
    [Theory]
    [InlineData(SecurityFieldsSet.GetEmpty, "invln_externalstatus")]
    [InlineData(SecurityFieldsSet.ChargesDebtCompany, "invln_debentureholder", "invln_outstandinglegalchargesordebt")]
    [InlineData(SecurityFieldsSet.DirLoans, "invln_directorloans")]
    [InlineData(SecurityFieldsSet.DirLoansSub, "invln_confirmationdirectorloanscanbesubordinated", "invln_reasonfordirectorloannotsubordinated")]
    [InlineData(
        SecurityFieldsSet.GetAllFields,
        "invln_externalstatus",
        "invln_debentureholder",
        "invln_outstandinglegalchargesordebt",
        "invln_directorloans",
        "invln_confirmationdirectorloanscanbesubordinated",
        "invln_reasonfordirectorloannotsubordinated")]
    [InlineData(
        SecurityFieldsSet.SaveAllFields,
        "invln_debentureholder",
        "invln_outstandinglegalchargesordebt",
        "invln_directorloans",
        "invln_confirmationdirectorloanscanbesubordinated",
        "invln_reasonfordirectorloannotsubordinated")]
    public void ShouldReturnAllRequiredCrmFieldsWhenSpecificSecurityFieldsSetIsProvided(
        SecurityFieldsSet fieldSet,
        params string[] crmFields)
    {
        // given && when
        var result = SecurityCrmFieldNameMapper.Map(fieldSet);

        // then
        result.Should().ContainAll(crmFields.Select(field => field))
            .And.Contain($"{nameof(invln_Loanapplication.invln_securitydetailscompletionstatus).ToLowerInvariant()}");
    }
}
