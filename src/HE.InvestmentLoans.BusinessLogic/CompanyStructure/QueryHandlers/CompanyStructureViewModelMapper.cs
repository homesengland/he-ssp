using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;

public static class CompanyStructureViewModelMapper
{
    public static CompanyStructureViewModel Map(CompanyStructureEntity companyStructureEntity)
    {
        return new CompanyStructureViewModel
        {
            Purpose = MapCompanyPurpose(companyStructureEntity.Purpose),
            LoanApplicationId = companyStructureEntity.LoanApplicationId.Value,
            ExistingCompany = companyStructureEntity.MoreInformation?.Information,
        };
    }

    public static string? MapCompanyPurpose(CompanyPurpose? companyPurpose)
    {
        if (companyPurpose is null)
        {
            return null;
        }

        return companyPurpose.IsSpv ? CommonResponse.Yes : CommonResponse.No;
    }

    public static CompanyPurpose? MapCompanyPurpose(string? companyPurpose)
    {
        return companyPurpose is null ? null : CompanyPurpose.New(companyPurpose == CommonResponse.Yes);
    }

    public static OrganisationMoreInformation? MapOrganisationMoreInformation(string? organisationMoreInformation)
    {
        return organisationMoreInformation is null ?
            null :
            new OrganisationMoreInformation(organisationMoreInformation);
    }

    public static OrganisationMoreInformationFile? MapOrganisationMoreInformationFile(string? fileName, byte[]? content)
    {
        return fileName is null || content is null ?
            null :
            new OrganisationMoreInformationFile(fileName, content);
    }
}
