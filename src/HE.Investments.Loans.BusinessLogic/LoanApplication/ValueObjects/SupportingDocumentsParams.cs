using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;

public record SupportingDocumentsParams(LoanApplicationId LoanApplicationId)
{
    public static SupportingDocumentsParams New(LoanApplicationId loanApplicationId) => new(loanApplicationId);
}
