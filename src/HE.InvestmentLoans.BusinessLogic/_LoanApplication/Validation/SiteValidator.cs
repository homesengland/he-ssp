using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class SiteValidator : AbstractValidator<SiteViewModel>
    {
        public SiteValidator()
        {
            RuleSet("ManyHomes", () =>
            {
                RuleFor(item => item.ManyHomes)
                .Matches(@"^(?!0)[1-9]\d{0,3}$|^9999$")
                .WithMessage(ErrorMessages.ManyHomesAmount.ToString());
            });

            RuleSet("StartDate", () =>
            {
                When(item => item.HasEstimatedStartDate == "Yes" 
                    && (item.EstimatedStartDay == null || item.EstimatedStartMonth == null || item.EstimatedStartYear == null),
                    () => RuleFor(item => item.EstimatedStartDay)
                    .NotEmpty()
                    .WithMessage(ErrorMessages.NoStartDate.ToString())
                );

                When(item => item.HasEstimatedStartDate == "Yes"
                    && (item.EstimatedStartDay != null || item.EstimatedStartMonth != null || item.EstimatedStartYear != null),
                    () => RuleFor(item => item)
                    .Must(model =>
                    {
                        try
                        {
                            var dateString = $"{model.EstimatedStartDay}/{model.EstimatedStartMonth}/{model.EstimatedStartYear}";
                            return DateTime.TryParseExact(dateString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage(ErrorMessages.InvalidStartDate.ToString())
                    .WithName("EstimatedStartDay")
                );

                When(item => item.PurchaseDay != null && item.PurchaseMonth != null && item.PurchaseYear != null,
                    () =>
                    {
                        RuleFor(item => item)
                        .Must(model =>
                        {
                            try
                            {
                                var dateString = $"{model.PurchaseDay}/{model.PurchaseMonth}/{model.PurchaseYear}";
                                return DateTime.TryParseExact(dateString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                            }
                            catch
                            {
                                return false;
                            }
                        })
                        .WithMessage(ErrorMessages.IncorrectPurchaseDate.ToString())
                        .WithName("PurchaseDate");
                    });
            });

            RuleSet("TypeHomes", () =>
            {
                When(item => item.TypeHomes != null && item.TypeHomes.Contains("other"), 
                    () => RuleFor(item => item.TypeHomesOther)
                    .NotEmpty()
                    .WithMessage(ErrorMessages.TypeHomesOtherType.ToString())
                );
            });

            RuleSet("Location", () =>
            {
                When(item => item.LocationOption == "coordinates", () =>
                {
                    RuleFor(item => item.LocationCoordinates)
                        .NotEmpty()
                        .WithMessage("Enter your XY coordinates");
                });

                When(item => item.LocationOption == "landRegistryTitleNumber", () =>
                {
                    RuleFor(item => item.LocationLandRegistry)
                        .NotEmpty()
                        .WithMessage("Enter your Land Registry title number");
                });
            });

            RuleSet("PlanningRefEnter", () =>
            {
                RuleFor(item => item.PlanningRefEnter)
                .Matches(@"^\d{2}/[A-Z\d]+/[A-Z\d]+$")
                .WithMessage(ErrorMessages.InvalidReferenceNumber.ToString());
            });

            RuleSet("GrantFundingMore", () =>
            {
                When(item => item.GrantFundingAmount != null,
                    () => RuleFor(item => item.GrantFundingAmount)
                            .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                            .WithMessage(ErrorMessages.AmountPoundInput("funding").ToString())
                        );
            });

            RuleSet("ChargesDebt", () =>
            {
                When(item => item.ChargesDebt == "Yes",
                    () => RuleFor(item => item.ChargesDebtInfo)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterExistingLegal.ToString())
                        );
            });

            RuleSet("Additional", () =>
            {
                When(item => item.PurchaseDay == null && item.PurchaseMonth == null && item.PurchaseYear == null,
                    () => RuleFor(item => item.PurchaseDate)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.NoPurchaseDate.ToString())
                        );

                When(item => item.PurchaseDay == null && (item.PurchaseMonth != null || item.PurchaseYear != null),
                    () => RuleFor(item => item.PurchaseDay)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.NoPurchaseDay.ToString())
                        );

                When(item => item.PurchaseMonth == null && (item.PurchaseDay != null || item.PurchaseYear != null),
                    () => RuleFor(item => item.PurchaseMonth)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.NoPurchaseMonth.ToString())
                        );

                When(item => item.PurchaseYear == null && (item.PurchaseDay != null || item.PurchaseMonth != null),
                    () => RuleFor(item => item.PurchaseYear)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.NoPurchaseYear.ToString())
                        );

                When(item => item.PurchaseDay != null && item.PurchaseMonth != null && item.PurchaseYear != null, 
                    () =>
                {
                    RuleFor(item => item)
                    .Must(model =>
                    {
                        try
                        {
                            var dateString = $"{model.PurchaseDay}/{model.PurchaseMonth}/{model.PurchaseYear}";
                            return DateTime.TryParseExact(dateString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage(ErrorMessages.IncorrectPurchaseDate.ToString())
                    .WithName("PurchaseDate")
                    .DependentRules(() =>
                    {
                        RuleFor(item => item).Must(model =>
                        {
                            var dateString = $"{model.PurchaseDay}/{model.PurchaseMonth}/{model.PurchaseYear}";
                            var providedDate = DateTime.ParseExact(dateString, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

                            return providedDate.Date.IsBeforeOrEqualTo(DateTime.UtcNow.Date);
                        })
                        .WithMessage(ErrorMessages.FuturePurchaseDate.ToString())
                        .WithName("PurchaseDate");
                    });
                });

                When(item => item.Cost == null,
                    () => RuleFor(item => item.Cost)
                    .NotEmpty()
                            .WithMessage(ErrorMessages.PoundInput("The purchase value of the land").ToString())
                        );

                When(item => item.Cost != null,
                    () => RuleFor(item => item.Cost)
                            .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                            .WithMessage(ErrorMessages.PoundInput("The purchase value of the land").ToString())
                        );

                When(item => item.Value == null,
                    () => RuleFor(item => item.Value)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.PoundInput("The current value of the land").ToString())
                        );

                When(item => item.Value != null,
                    () => RuleFor(item => item.Value)
                            .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                            .WithMessage(ErrorMessages.PoundInput("The current value of the land").ToString())
                        );

                When(item => item.Source == null,
                    () => RuleFor(item => item.Source)
                            .NotEmpty()
                            .WithMessage(ErrorMessages.EnterMoreDetails.ToString())
                        );
            });

            RuleSet("CheckAnswers", () =>
            {
                RuleFor(item => item.CheckAnswers)
                .NotEmpty()
                .WithMessage(ErrorMessages.SecurityCheckAnswers.ToString());

                When(item => item.CheckAnswers == "Yes", () =>
                {
                    RuleFor(m => m).Must(x =>
                    !string.IsNullOrEmpty(x.Name) &&
                    !string.IsNullOrEmpty(x.ManyHomes) &&
                    !string.IsNullOrEmpty(x.HasEstimatedStartDate) &&
                    !(x.TypeHomes != null && x.TypeHomes.Length > 0) &&
                    !string.IsNullOrEmpty(x.Type) &&
                    !string.IsNullOrEmpty(x.Location) &&
                    !string.IsNullOrEmpty(x.PlanningRef) &&
                    (x.PlanningRef == "No" || !string.IsNullOrEmpty(x.PlanningRefEnter)) &&
                    !string.IsNullOrEmpty(x.GrantFunding) &&
                    (x.GrantFunding == "No" || !string.IsNullOrEmpty(x.Ownership)) &&
                    (x.GrantFunding == "No" || !string.IsNullOrEmpty(x.ChargesDebt)) &&
                    !string.IsNullOrEmpty(x.AffordableHomes) &&
                    (x.Ownership == "No" || (!string.IsNullOrEmpty(x.PurchaseYear)))
                    ).WithMessage(ErrorMessages.CheckAnswersOption.ToString());
                });
            });
        }
    }
}
