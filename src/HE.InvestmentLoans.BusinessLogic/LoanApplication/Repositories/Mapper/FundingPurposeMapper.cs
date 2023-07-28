using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class FundingPurposeMapper
{
    public static FundingPurpose? Map(string? fundingPurposeAsString)
    {
        return fundingPurposeAsString switch
        {
            FundingPurposeString.BuildingNewHomes => FundingPurpose.BuildingNewHomes,
            FundingPurposeString.BuildingInfrastructureOnly => FundingPurpose.BuildingInfrastructure,
            FundingPurposeString.Other => FundingPurpose.Other,
            _ => null,
        };
    }

    public static string Map(FundingPurpose? fundingPurpose)
    {
        return fundingPurpose switch
        {
            FundingPurpose.BuildingNewHomes => FundingPurposeString.BuildingNewHomes,
            FundingPurpose.BuildingInfrastructure => FundingPurposeString.BuildingInfrastructureOnly,
            FundingPurpose.Other => FundingPurposeString.Other,
            _ => string.Empty,
        };
    }
}
