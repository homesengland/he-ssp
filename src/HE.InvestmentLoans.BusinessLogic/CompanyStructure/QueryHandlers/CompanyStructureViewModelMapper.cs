using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;

public static class CompanyStructureViewModelMapper
{
    public static CompanyStructureViewModel Map(CompanyStructureEntity companyStructureEntity)
    {
        return new CompanyStructureViewModel
        {
            Purpose = MapCompanyPurpose(companyStructureEntity.Purpose),
            LoanApplicationId = companyStructureEntity.LoanApplicationId.Value,
            ExistingCompany = companyStructureEntity.MoreInformation.IsProvided ? companyStructureEntity.MoreInformation.Value.Information : null,
        };
    }

    public static string? MapCompanyPurpose(Providable<CompanyPurpose> companyPurpose)
    {
        if (companyPurpose.IsNotProvided)
        {
            return null;
        }

        return companyPurpose.Value.IsSpv ? CommonResponse.Yes : CommonResponse.No;
    }

    public static Providable<CompanyPurpose> MapCompanyPurpose(string? companyPurpose)
    {
        return companyPurpose is null ? CompanyPurpose.NotProvided() : CompanyPurpose.New(companyPurpose == CommonResponse.Yes);
    }

    public static Providable<OrganisationMoreInformation> MapOrganisationMoreInformation(string? organisationMoreInformation)
    {
        return organisationMoreInformation is null ?
            OrganisationMoreInformation.NotProvided() :
            new Providable<OrganisationMoreInformation>(new OrganisationMoreInformation(organisationMoreInformation));
    }

    public static Providable<OrganisationMoreInformationFile> MapOrganisationMoreInformationFile(string? fileName, byte[]? content)
    {
        return fileName is null || content is null ?
            OrganisationMoreInformationFile.NotProvided() :
            new Providable<OrganisationMoreInformationFile>(new OrganisationMoreInformationFile(fileName, content));
    }
}
