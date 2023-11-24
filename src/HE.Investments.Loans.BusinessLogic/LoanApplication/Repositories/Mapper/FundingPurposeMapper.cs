using HE.Investments.Loans.Common.Utils.Constants;
using HE.Investments.Loans.Contract.Application.Enums;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class FundingPurposeMapper
{
    public static FundingPurpose Map(string? fundingPurposeAsString)
    {
        return fundingPurposeAsString switch
        {
            FundingPurposeString.BuildingNewHomes => FundingPurpose.BuildingNewHomes,
            FundingPurposeString.BuildingInfrastructureOnly => FundingPurpose.BuildingInfrastructure,
            FundingPurposeString.Other => FundingPurpose.Other,
            _ => FundingPurpose.BuildingNewHomes,
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
