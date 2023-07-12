using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Validation;

public class FundingValidator : AbstractValidator<FundingViewModel>
{
    public FundingValidator()
    {
        RuleSet("GDV", () => When(
                item => item.GrossDevelopmentValue != null,
                () => RuleFor(item => item.GrossDevelopmentValue)
                          .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                          .WithMessage(ErrorMessages.EstimatedPoundInput("GDV").ToString())));

        RuleSet("TotalCosts", () => When(
                item => item.TotalCosts != null,
                () => RuleFor(item => item.TotalCosts)
                        .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                        .WithMessage(ErrorMessages.EstimatedPoundInput("total cost").ToString())));

        RuleSet("PrivateSectorFunding", () =>
        {
            When(
                item => item.PrivateSectorFunding == "Yes",
                () => RuleFor(item => item.PrivateSectorFundingResult)
                        .NotEmpty()
                        .WithMessage(ErrorMessages.EnterMoreDetails.ToString()));

            When(
                item => item.PrivateSectorFunding == "No",
                () => RuleFor(item => item.PrivateSectorFundingReason)
                        .NotEmpty()
                        .WithMessage(ErrorMessages.EnterMoreDetails.ToString()));
        });

        RuleSet("AbnormalCosts", () => When(
                item => item.AbnormalCosts == "Yes",
                () => RuleFor(item => item.AbnormalCostsInfo)
                        .NotEmpty()
                        .WithMessage(ErrorMessages.EnterMoreDetails.ToString())));

        RuleSet("Refinance", () => When(
                item => item.Refinance == "refinance",
                () => RuleFor(item => item.RefinanceInfo)
                        .NotEmpty()
                        .WithMessage("Enter more detail about your refinance exit strategy")));

        RuleSet("CheckAnswers", () =>
        {
            RuleFor(item => item.CheckAnswers)
            .NotEmpty()
            .WithMessage(ErrorMessages.SecurityCheckAnswers.ToString());

            When(item => item.CheckAnswers == "Yes", () => RuleFor(m => m).Must(x =>
                !string.IsNullOrEmpty(x.GrossDevelopmentValue) &&
                !string.IsNullOrEmpty(x.TotalCosts) &&
                !string.IsNullOrEmpty(x.PrivateSectorFunding) &&
                !string.IsNullOrEmpty(x.AdditionalProjects) &&
                !string.IsNullOrEmpty(x.AbnormalCosts) &&
                !string.IsNullOrEmpty(x.Refinance))
                .WithMessage(ErrorMessages.CheckAnswersOption.ToString()));
        });
    }
}
