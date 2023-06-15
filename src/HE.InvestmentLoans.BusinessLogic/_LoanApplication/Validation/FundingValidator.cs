using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class FundingValidator : AbstractValidator<FundingViewModel>
    {
        public FundingValidator()
        {
            RuleSet("GDV", () =>
            {
                RuleFor(item => item.GrossDevelopmentValue)
                .NotEmpty()
                .WithMessage(ErrorMessages.EstimatedPoundInput("GDV").ToString())
                .Matches(@"^\d+$|^\d+\.\d{2}$")
                .WithMessage(ErrorMessages.EstimatedPoundInput("GDV").ToString());
            });

            RuleSet("TotalCosts", () =>
            {
                RuleFor(item => item.TotalCosts)
                .NotEmpty()
                .WithMessage(ErrorMessages.EstimatedPoundInput("total cost").ToString())
                .Matches(@"^\d+$|^\d+\.\d{2}$")
                .WithMessage(ErrorMessages.EstimatedPoundInput("total cost").ToString());

            });

            RuleSet("PrivateSectorFunding", () =>
            {
                RuleFor(item => item.PrivateSectorFunding)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());

                When(item => item.PrivateSectorFunding == "Yes",
                    () => RuleFor(item => item.PrivateSectorFundingResult)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );

                When(item => item.PrivateSectorFunding == "No",
                    () => RuleFor(item => item.PrivateSectorFundingReason)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );
            });

            RuleSet("AbnormalCosts", () =>
            {
                RuleFor(item => item.AbnormalCosts)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());

                When(item => item.AbnormalCosts == "Yes",
                    () => RuleFor(item => item.AbnormalCostsInfo)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );
            });

            RuleSet("Refinance", () =>
            {
                RuleFor(item => item.Refinance)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());

                When(item => item.Refinance == "refinance",
                    () => RuleFor(item => item.RefinanceInfo)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );
            });

            RuleSet("AdditionalProjects", () =>
            {
                RuleFor(item => item.AdditionalProjects)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("CheckAnswers", () =>
            {
                RuleFor(item => item.CheckAnswers)
                .NotEmpty()
                .WithMessage(ErrorMessages.CheckAnswersOption.ToString());
            });
        }
    }
}
