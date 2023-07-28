using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;

public class LoanApplicationValidator : AbstractValidator<LoanApplicationViewModel>
{
    public LoanApplicationValidator()
    {
        RuleSet(LoanApplicationView.LoanPurpose, () => RuleFor(c => c.Purpose)
                .NotEmpty()
                .WithMessage(ValidationErrorMessage.LoanPurpose));
    }
}
