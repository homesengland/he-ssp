using FluentValidation;
using HE.InvestmentLoans.WWW.Models;
using HE.Investments.Common.Messages;

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
