using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;

public static class CompanyStructureMapper
{
    public static Providable<CompanyPurpose> MapCompanyPurpose(string? companyPurpose)
    {
        if (string.IsNullOrEmpty(companyPurpose))
        {
            return CompanyPurpose.NotProvided();
        }

        return CompanyPurpose.New(companyPurpose.Equals(CommonResponse.Yes, StringComparison.OrdinalIgnoreCase));
    }

    public static string? MapCompanyPurpose(Providable<CompanyPurpose> companyPurpose)
    {
        if (companyPurpose.IsNotProvided)
        {
            return null;
        }

        return companyPurpose.Value.IsSpv switch
        {
            true => CommonResponse.Yes,
            false => CommonResponse.No,
        };
    }
}
