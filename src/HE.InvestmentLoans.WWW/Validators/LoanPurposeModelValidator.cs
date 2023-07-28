using FluentValidation;
using HE.InvestmentLoans.WWW.Models;

namespace HE.InvestmentLoans.WWW.Validators;

public class LoanPurposeModelValidator : AbstractValidator<LoanPurposeModel>
{
    public LoanPurposeModelValidator()
    {
        RuleFor(c => c.FundingPurpose)
            .NotEmpty()
            .WithMessage("Select what you need Homes England funding for");
    }
}
