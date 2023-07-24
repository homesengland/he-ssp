using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class FundingPurposeMapper
{
    public static FundingPurpose? Map(string? fundingPurposeAsString)
    {
        return fundingPurposeAsString switch
        {
            "Buildingnewhomes" => FundingPurpose.BuildingNewHomes,
            "Buildinginfrastructureonly" => FundingPurpose.BuildingInfrastructure,
            "Other" => FundingPurpose.Other,
            _ => null,
        };
    }
}
