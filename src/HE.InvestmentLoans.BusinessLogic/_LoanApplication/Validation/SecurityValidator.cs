using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class SecurityValidator : AbstractValidator<SecurityViewModel>
    {
        public SecurityValidator()
        {
            RuleSet("ChargesDebtCompany", () =>
            {
                RuleFor(item => item.ChargesDebtCompany).Must(val => val == "Yes" || val == "No").WithMessage(ErrorMessages.RadioOption.ToString());
                When(item => item.ChargesDebtCompany == "Yes", () => RuleFor(item => item.ChargesDebtCompanyInfo).NotEmpty().WithMessage(ErrorMessages.EnterMoreDetails.ToString()));
            });

            RuleSet("DirLoans", () =>
            {
                RuleFor(item => item.DirLoans).Must(val => val == "Yes" || val == "No").WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("DirLoansSub", () =>
            {
                RuleFor(item => item.DirLoansSub).Must(val => val == "Yes" || val == "No").WithMessage(ErrorMessages.RadioOption.ToString());

                When(item => item.DirLoansSub == "No", () => RuleFor(item => item.DirLoansSubMore).NotEmpty().WithMessage(ErrorMessages.EnterMoreDetails.ToString()));
            });

            RuleSet("CheckAnswers", () =>
            {
                RuleFor(item => item.CheckAnswers)
                .NotEmpty()
                .WithMessage(ErrorMessages.SecurityCheckAnswers.ToString());
            });
        }
    }
}
