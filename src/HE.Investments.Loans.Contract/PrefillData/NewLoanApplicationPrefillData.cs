using HE.Investments.Loans.Contract.Application.Enums;

namespace HE.Investments.Loans.Contract.PrefillData;

public record NewLoanApplicationPrefillData(FundingPurpose? FundingPurpose, string? ApplicationName)
{
    public static NewLoanApplicationPrefillData Empty => new(null, null);
}
