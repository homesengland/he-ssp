using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class SiteValidator : AbstractValidator<SiteViewModel>
    {
        public SiteValidator()
        {
            RuleSet("Name", () =>
            {
                RuleFor(item => item.Name)
                .NotEmpty()
                
                .WithMessage(ErrorMessages.EnterMoreDetails.ToString());
            });

            RuleSet("ManyHomes", () =>
            {
                RuleFor(item => item.ManyHomes)
                .NotEmpty()
                .WithMessage(ErrorMessages.ManyHomesAmount.ToString())
                .Matches(@"^(?!0)[1-9]\d{0,3}$|^9999$")
                .WithMessage(ErrorMessages.ManyHomesAmount.ToString());
            });

            RuleSet("TypeHomes", () =>
            {
                RuleFor(item => item.TypeHomes)
                .NotEmpty()
                .WithMessage(ErrorMessages.CheckboxOption.ToString());
                When(item => item.TypeHomes != null && item.TypeHomes.Contains("other"), () => RuleFor(item => item.TypeHomesOther).NotEmpty().WithMessage(ErrorMessages.TypeHomesOtherType.ToString()));
            });

            RuleSet("Type", () =>
            {
                RuleFor(item => item.Type)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("Location", () =>
            {
                RuleFor(item => item.LocationOption)
                    .NotEmpty()
                    .WithMessage(ErrorMessages.RadioOption.ToString());

                When(item => item.LocationOption == "coordinates", () =>
                {
                    RuleFor(item => item.LocationCoordinates)
                        .NotEmpty()
                        .WithMessage(ErrorMessages.EnterMoreDetails.ToString());

                    When(item => item.LocationCoordinates != null, () =>
                    {
                        RuleFor(item => item.LocationCoordinates)
                        .Must(coordinates =>
                        {
                            return ValidateCoordinates(coordinates, out string invalidCharacters);
                        })
                        .WithMessage((item, coordinates) =>
                        {
                            ValidateCoordinates(coordinates, out string invalidCharacters);
                            return ErrorMessages.InvalidXYCoordinates(invalidCharacters).ToString();
                        });
                    });
                });

                When(item => item.LocationOption == "landRegistryTitleNumber", () =>
                {
                    RuleFor(item => item.LocationLandRegistry)
                        .NotEmpty()
                        .WithMessage(ErrorMessages.EnterMoreDetails.ToString());
                });
            });

            RuleSet("PlanningRef", () =>
            {
                RuleFor(item => item.PlanningRef)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("PlanningRefEnter", () =>
            {
                RuleFor(item => item.PlanningRefEnter)
                .NotEmpty()
                .WithMessage(ErrorMessages.InvalidReferenceNumber.ToString())
                .Matches(@"^\d{2}/[A-Z\d]+/[A-Z\d]+$")
                .WithMessage(ErrorMessages.InvalidReferenceNumber.ToString());
            });

            RuleSet("Ownership", () =>
            {
                RuleFor(item => item.Ownership)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("GrantFunding", () =>
            {
                RuleFor(item => item.GrantFunding)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("GrantFundingMore", () =>
            {
                When(item => item.GrantFundingSource == null,
                    () => RuleFor(item => item.GrantFundingSource)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );

                When(item => item.GrantFundingAmount == null,
                    () => RuleFor(item => item.GrantFundingAmount)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.AmountPoundInput("funding").ToString())
                            .Matches(@"^[0-9]*$")
                            .WithMessage(ErrorMessages.AmountPoundInput("funding").ToString())
                        );

                When(item => item.GrantFundingName == null,
                    () => RuleFor(item => item.GrantFundingName)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );

                When(item => item.GrantFundingPurpose == null,
                    () => RuleFor(item => item.GrantFundingPurpose)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );
            });

            RuleSet("ChargesDebt", () =>
            {
                RuleFor(item => item.ChargesDebt)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());

                When(item => item.ChargesDebt == "Yes",
                    () => RuleFor(item => item.ChargesDebtInfo)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterExistingLegal.ToString())
                        );
            });

            RuleSet("AffordableHomes", () =>
            {
                RuleFor(item => item.AffordableHomes)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("CheckAnswers", () =>
            {
                RuleFor(item => item.CheckAnswers)
                .NotEmpty()
                .WithMessage(ErrorMessages.CheckAnswersOption.ToString());
            });

            RuleSet("Additional", () =>
            {
                When(item => item.Cost == null,
                    () => RuleFor(item => item.Cost)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.PoundInput("The purchase value of the project").ToString())
                            .Matches(@"^[0-9]*$")
                            .WithMessage(ErrorMessages.PoundInput("The purchase value of the project").ToString())
                        );

                When(item => item.Value == null,
                    () => RuleFor(item => item.Value)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.PoundInput("The current value of the project").ToString())
                            .Matches(@"^[0-9]*$")
                            .WithMessage(ErrorMessages.PoundInput("The current value of the project").ToString())
                        );

                When(item => item.Source == null,
                    () => RuleFor(item => item.Source)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );
            });
        }

        private bool ValidateCoordinates(string coordinates, out string invalidCharacters)
        {
            var validCharactersRegex = new Regex(@"[^-\d.,]");
            var invalidCharactersList = validCharactersRegex.Matches(coordinates)
                .Select(match => match.Value[0])
                .ToList();

            invalidCharacters = new string(invalidCharactersList.ToArray());
            return invalidCharactersList.Count == 0;
        }
    }
}
