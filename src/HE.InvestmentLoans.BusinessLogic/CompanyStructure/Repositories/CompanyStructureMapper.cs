using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;

public static class CompanyStructureMapper
{
    public static CompanyPurpose? MapCompanyPurpose(string? companyPurpose)
    {
        if (string.IsNullOrEmpty(companyPurpose))
        {
            return null;
        }

        return CompanyPurpose.New(companyPurpose.Equals(CommonResponse.Yes, StringComparison.OrdinalIgnoreCase));
    }

    public static string? MapCompanyPurpose(CompanyPurpose? companyPurpose)
    {
        if (companyPurpose is null)
        {
            return null;
        }

        return companyPurpose.IsSpv switch
        {
            true => CommonResponse.Yes,
            false => CommonResponse.No,
        };
    }
}
