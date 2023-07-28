using FluentValidation;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.WWW.Models;

namespace HE.InvestmentLoans.WWW.Validators;

public class LoanPurposeModelValidator : AbstractValidator<LoanPurposeModel>
{
    public LoanPurposeModelValidator()
    {
        RuleFor(c => c.FundingPurpose)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.LoanPurpose);
    }
}
