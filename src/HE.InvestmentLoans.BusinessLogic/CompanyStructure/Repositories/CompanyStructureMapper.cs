using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;

public static class CompanyStructureMapper
{
    public static CompanyPurpose MapCompanyPurpose(string? companyPurpose)
    {
        return new CompanyPurpose(companyPurpose?.Equals(CommonResponse.Yes, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    public static string? MapCompanyPurpose(CompanyPurpose companyPurpose)
    {
        return companyPurpose.IsSpv switch
        {
            true => CommonResponse.Yes,
            false => CommonResponse.No,
            null => null,
        };
    }
}
