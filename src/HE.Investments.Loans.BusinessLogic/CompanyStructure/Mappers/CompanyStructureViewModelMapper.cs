using HE.Investments.Common.Domain;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.CompanyStructure;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.Mappers;

public static class CompanyStructureViewModelMapper
{
    public static CompanyStructureViewModel Map(CompanyStructureEntity companyStructureEntity)
    {
        return new CompanyStructureViewModel
        {
            Purpose = MapCompanyPurpose(companyStructureEntity.Purpose),
            LoanApplicationId = companyStructureEntity.LoanApplicationId.Value,
            LoanApplicationStatus = companyStructureEntity.LoanApplicationStatus,
            OrganisationMoreInformation = companyStructureEntity.MoreInformation?.Information,
            HomesBuilt = companyStructureEntity.HomesBuilt?.ToString(),
            CheckAnswers = companyStructureEntity.Status == SectionStatus.Completed ? CommonResponse.Yes : null,
            State = companyStructureEntity.Status,
            AllowedFileExtensions = OrganisationMoreInformationFile.AllowedExtensions,
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
}
