using FluentValidation;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.WWW.Models;

namespace HE.Investments.Loans.WWW.Validators;

public class LoanPurposeModelValidator : AbstractValidator<LoanPurposeModel>
{
    public LoanPurposeModelValidator()
    {
        RuleFor(c => c.FundingPurpose)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.LoanPurpose);
    }
}
