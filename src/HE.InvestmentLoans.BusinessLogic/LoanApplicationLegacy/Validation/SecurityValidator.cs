using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;

public class SecurityValidator : AbstractValidator<SecurityViewModel>
{
    public SecurityValidator()
    {
        RuleSet(SecurityView.ChargesDebtCompany, () =>
        {
            When(
                item => item.ChargesDebtCompany == CommonResponse.Yes,
                () => RuleFor(item => item.ChargesDebtCompanyInfo)
                .NotEmpty()
                .WithMessage(ValidationErrorMessage.EnterMoreDetails));

            When(
                    item => item.ChargesDebtCompanyInfo != null,
                    () => RuleFor(item => item.ChargesDebtCompanyInfo)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.InputLongerThanThousandCharacters));
        });

        RuleSet(SecurityView.DirLoansSub, () =>
        {
            When(
                    item => item.DirLoansSub == CommonResponse.No,
                    () => RuleFor(item => item.DirLoansSubMore)
                    .NotEmpty()
                    .WithMessage(ValidationErrorMessage.EnterMoreDetails));

            When(
                    item => item.DirLoansSubMore != null,
                    () => RuleFor(item => item.DirLoansSubMore)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.InputLongerThanThousandCharacters));
        });

        RuleSet(SecurityView.CheckAnswers, () =>
        {
            RuleFor(item => item.CheckAnswers)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.SecurityCheckAnswers);

            When(item => item.CheckAnswers == CommonResponse.Yes, () => RuleFor(m => m).Must(x =>
                !string.IsNullOrEmpty(x.ChargesDebtCompany) &&
                !string.IsNullOrEmpty(x.DirLoans) &&
                    (x.DirLoans == CommonResponse.No || !string.IsNullOrEmpty(x.DirLoansSub)))
                .WithMessage(ValidationErrorMessage.CheckAnswersOption)
                .OverridePropertyName(nameof(SecurityViewModel.CheckAnswers)));
        });
    }
}
