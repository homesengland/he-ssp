using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
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
            OrganisationMoreInformation = companyStructureEntity.MoreInformation?.Information,
            HomesBuilt = companyStructureEntity.HomesBuilt?.ToString(),
            CheckAnswers = companyStructureEntity.Status == SectionStatus.Completed ? CommonResponse.Yes : null,
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
