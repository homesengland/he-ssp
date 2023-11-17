using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;

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
