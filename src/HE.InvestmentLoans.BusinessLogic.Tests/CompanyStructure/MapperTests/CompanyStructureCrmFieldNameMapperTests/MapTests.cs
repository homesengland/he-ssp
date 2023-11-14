using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.Investments.Common.CRM.Model;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.MapperTests.CompanyStructureCrmFieldNameMapperTests;

public class MapTests
{
    [Theory]
    [InlineData(CompanyStructureFieldsSet.GetEmpty, "invln_externalstatus")]
    [InlineData(CompanyStructureFieldsSet.CompanyPurpose, "invln_companypurpose")]
    [InlineData(CompanyStructureFieldsSet.HomesBuilt, "invln_companyexperience")]
    [InlineData(CompanyStructureFieldsSet.MoreInformationAboutOrganization, "invln_companystructureinformation")]
    [InlineData(
        CompanyStructureFieldsSet.GetAllFields,
        "invln_externalstatus",
        "invln_companypurpose",
        "invln_companyexperience",
        "invln_companystructureinformation")]
    [InlineData(
        CompanyStructureFieldsSet.SaveAllFields,
        "invln_companypurpose",
        "invln_companyexperience",
        "invln_companystructureinformation")]
    public void ShouldReturnAllRequiredCrmFieldsWhenSpecificCompanyStructureFieldsSetIsProvided(
        CompanyStructureFieldsSet fieldSet,
        params string[] crmFields)
    {
        // given && when
        var result = CompanyStructureCrmFieldNameMapper.Map(fieldSet);

        // then
        result.Should()
            .ContainAll(crmFields.Select(field => field))
            .And.Contain($"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }
}
