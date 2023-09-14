using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;

public class FundingValidator : AbstractValidator<FundingViewModel>
{
    public FundingValidator()
    {
        RuleSet(FundingView.GDV, () => When(
                item => item.GrossDevelopmentValue != null,
                () => RuleFor(item => item.GrossDevelopmentValue)
                          .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                          .WithMessage(ValidationErrorMessage.EstimatedPoundInput("GDV"))));

        RuleSet(FundingView.TotalCosts, () => When(
                item => item.TotalCosts != null,
                () => RuleFor(item => item.TotalCosts)
                        .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                        .WithMessage(ValidationErrorMessage.EstimatedPoundInput("total cost"))));

        RuleSet(FundingView.PrivateSectorFunding, () =>
        {
            When(
                item => item.PrivateSectorFunding == CommonResponse.Yes,
                () => RuleFor(item => item.PrivateSectorFundingResult)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.EnterMoreDetails));

            When(
                item => item.PrivateSectorFunding == CommonResponse.No,
                () => RuleFor(item => item.PrivateSectorFundingReason)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.EnterMoreDetails));

            When(
                    item => item.PrivateSectorFundingResult != null,
                    () => RuleFor(item => item.PrivateSectorFundingResult)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));

            When(
                    item => item.PrivateSectorFundingReason != null,
                    () => RuleFor(item => item.PrivateSectorFundingReason)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));
        });

        RuleSet(FundingView.AbnormalCosts, () =>
        {
            When(
                    item => item.AbnormalCosts == CommonResponse.Yes,
                    () => RuleFor(item => item.AbnormalCostsInfo)
                            .NotEmpty()
                            .WithMessage(ValidationErrorMessage.EnterMoreDetails));

            When(
                    item => item.AbnormalCostsInfo != null,
                    () => RuleFor(item => item.AbnormalCostsInfo)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));
        });

        RuleSet(FundingView.Refinance, () =>
        {
            When(
                item => item.Refinance == FundingFormOption.Refinance,
                () => RuleFor(item => item.RefinanceInfo)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.EnterMoreDetailsForRefinanceExitStrategy));

            When(
                    item => item.RefinanceInfo != null,
                    () => RuleFor(item => item.RefinanceInfo)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));
        });

        RuleSet(FundingView.CheckAnswers, () =>
        {
            RuleFor(item => item.CheckAnswers)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.SecurityCheckAnswers);

            When(item => item.CheckAnswers == CommonResponse.Yes, () => RuleFor(m => m)
                    .Must(x =>
                        !string.IsNullOrEmpty(x.GrossDevelopmentValue) &&
                        !string.IsNullOrEmpty(x.TotalCosts) &&
                        !string.IsNullOrEmpty(x.PrivateSectorFunding) &&
                        !string.IsNullOrEmpty(x.AdditionalProjects) &&
                        !string.IsNullOrEmpty(x.AbnormalCosts) &&
                        !string.IsNullOrEmpty(x.Refinance))
                    .WithMessage(ValidationErrorMessage.CheckAnswersOption)
                    .OverridePropertyName(nameof(FundingViewModel.CheckAnswers)));
        });
    }
}
