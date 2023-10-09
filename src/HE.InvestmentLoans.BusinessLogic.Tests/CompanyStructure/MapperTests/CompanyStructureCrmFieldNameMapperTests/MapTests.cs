using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.MapperTests.CompanyStructureCrmFieldNameMapperTests;

public class MapTests
{
    [Fact]
    public void ShouldReturnExternalStatusAndCompanyStructureCompletionStatusWhenCompanyStructureFieldsSetIsGetEmpty()
    {
        // given
        var getEmpty = CompanyStructureFieldsSet.GetEmpty;

        // when
        var result = CompanyStructureCrmFieldNameMapper.Map(getEmpty);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnCompanyPurposeAndCompanyStructureCompletionStatusWhenCompanyStructureFieldsSetIsCompanyPurpose()
    {
        // given
        var companyPurpose = CompanyStructureFieldsSet.CompanyPurpose;

        // when
        var result = CompanyStructureCrmFieldNameMapper.Map(companyPurpose);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_CompanyPurpose).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnCompanyExperienceAndCompanyStructureCompletionStatusWhenCompanyStructureFieldsSetIsHomesBuilt()
    {
        // given
        var homesBuilt = CompanyStructureFieldsSet.HomesBuilt;

        // when
        var result = CompanyStructureCrmFieldNameMapper.Map(homesBuilt);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_CompanyExperience).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnCompanyStructureInformationAndCompanyStructureCompletionStatusWhenCompanyStructureFieldsSetIsMoreInformationAboutOrganization()
    {
        // given
        var moreInformationAboutOrganization = CompanyStructureFieldsSet.MoreInformationAboutOrganization;

        // when
        var result = CompanyStructureCrmFieldNameMapper.Map(moreInformationAboutOrganization);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Companystructureinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAllFieldsWhenCompanyStructureFieldsSetIsGetAllFields()
    {
        // given
        var getAllFields = CompanyStructureFieldsSet.GetAllFields;

        // when
        var result = CompanyStructureCrmFieldNameMapper.Map(getAllFields);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_CompanyPurpose).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Companystructureinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_CompanyExperience).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAllFieldsExceptExternalStatusWhenCompanyStructureFieldsSetIsSaveAllFields()
    {
        // given
        var saveAllFields = CompanyStructureFieldsSet.SaveAllFields;

        // when
        var result = CompanyStructureCrmFieldNameMapper.Map(saveAllFields);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_CompanyPurpose).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Companystructureinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_CompanyExperience).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_companystructureandexperiencecompletionst).ToLowerInvariant()}");
    }
}
