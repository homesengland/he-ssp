using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Validation;

public class SecurityValidator : AbstractValidator<SecurityViewModel>
{
    public SecurityValidator()
    {
        RuleSet("ChargesDebtCompany", () => When(
                item => item.ChargesDebtCompany == "Yes",
                () => RuleFor(item => item.ChargesDebtCompanyInfo)
                .NotEmpty()
                .WithMessage(ErrorMessages.EnterMoreDetails.ToString())));

        RuleSet("DirLoansSub", () => When(
                item => item.DirLoansSub == "No",
                () => RuleFor(item => item.DirLoansSubMore)
                .NotEmpty()
                .WithMessage(ErrorMessages.EnterMoreDetails.ToString())));

        RuleSet("CheckAnswers", () =>
        {
            RuleFor(item => item.CheckAnswers)
            .NotEmpty()
            .WithMessage(ErrorMessages.SecurityCheckAnswers.ToString());

            When(item => item.CheckAnswers == "Yes", () => RuleFor(m => m).Must(x =>
                !string.IsNullOrEmpty(x.ChargesDebtCompany) &&
                !string.IsNullOrEmpty(x.DirLoans) &&
                    (x.DirLoans == "No" || !string.IsNullOrEmpty(x.DirLoansSub)))
                .WithMessage(ErrorMessages.CheckAnswersOption.ToString()));
        });
    }
}
