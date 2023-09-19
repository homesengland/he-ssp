using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;

[SuppressMessage("Design", "CA1031", Justification = "Need to refactored in the fure")]
public class SiteValidator : AbstractValidator<SiteViewModel>
{
    public SiteValidator()
    {
        RuleSet(ProjectView.Name, () => When(
                item => item.Name != null,
                () => RuleFor(item => item.Name)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.ShortInputLengthExcedeed)));

        RuleSet(ProjectView.ManyHomes, () => RuleFor(item => item.ManyHomes)
            .Matches(@"^(?!0)[1-9]\d{0,3}$|^9999$")
            .WithMessage(ValidationErrorMessage.ManyHomesAmount));

        RuleSet(ProjectView.StartDate, () =>
        {
            When(
                item => item.HasEstimatedStartDate == CommonResponse.Yes
                && (item.EstimatedStartDay == null || item.EstimatedStartMonth == null || item.EstimatedStartYear == null),
                () => RuleFor(item => item.EstimatedStartDay)
                .NotEmpty()
                .WithMessage(ValidationErrorMessage.NoStartDate));

            When(
                item => item.HasEstimatedStartDate == CommonResponse.Yes
                && (item.EstimatedStartDay != null || item.EstimatedStartMonth != null || item.EstimatedStartYear != null),
                () => RuleFor(item => item)
                .Must(model =>
                {
                    try
                    {
                        var dateString = $"{model.EstimatedStartDay}/{model.EstimatedStartMonth}/{model.EstimatedStartYear}";
                        return DateTime.TryParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage(ValidationErrorMessage.InvalidStartDate)
                .WithName(ProjectFormOption.EstimatedStartDay));

            When(
                item => item.PurchaseDay != null && item.PurchaseMonth != null && item.PurchaseYear != null,
                () => RuleFor(item => item)
                    .Must(model =>
                    {
                        try
                        {
                            var dateString = $"{model.PurchaseDay}/{model.PurchaseMonth}/{model.PurchaseYear}";
                            return DateTime.TryParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage(ValidationErrorMessage.IncorrectPurchaseDate)
                    .WithName(ProjectFormOption.PurchaseDate));
        });

        RuleSet(ProjectView.TypeHomes, () =>
        {
            When(
                    item => item.TypeHomes != null && item.TypeHomes.Contains(CommonResponse.Other),
                    () => RuleFor(item => item.TypeHomesOther)
                    .NotEmpty()
                    .WithMessage(ValidationErrorMessage.TypeHomesOtherType));

            When(
                item => item.TypeHomesOther != null,
                () => RuleFor(item => item.TypeHomesOther)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.ShortInputLengthExcedeed));
        });

        RuleSet(ProjectView.Location, () =>
        {
            When(
                item => item.LocationOption == ProjectFormOption.Coordinates,
                () => RuleFor(item => item.LocationCoordinates)
                    .NotEmpty()
                    .WithMessage(ValidationErrorMessage.EnterCoordinates));

            When(
                    item => item.LocationCoordinates != null,
                    () => RuleFor(item => item.LocationCoordinates)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));

            When(
                item => item.LocationOption == ProjectFormOption.LandRegistryTitleNumber,
                () => RuleFor(item => item.LocationLandRegistry)
                    .NotEmpty()
                    .WithMessage(ValidationErrorMessage.EnterLandRegistryTitleNumber));

            When(
                    item => item.LocationLandRegistry != null,
                    () => RuleFor(item => item.LocationLandRegistry)
                        .Must(value => value!.Length <= MaximumInputLength.LongInput)
                        .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));
        });

        RuleSet(ProjectView.PlanningRefEnter, () => When(
                item => item.PlanningRefEnter != null,
                () => RuleFor(item => item.PlanningRefEnter)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.ShortInputLengthExcedeed)));

        RuleSet(ProjectView.GrantFundingMore, () =>
        {
            When(
                    item => item.GrantFundingAmount != null,
                    () => RuleFor(item => item.GrantFundingAmount)
                            .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                            .WithMessage(ValidationErrorMessage.IncorrectGrantFundingAmount));

            When(
                item => item.GrantFundingSource != null,
                () => RuleFor(item => item.GrantFundingSource)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.ShortInputLengthExcedeed));

            When(
                item => item.GrantFundingName != null,
                () => RuleFor(item => item.GrantFundingName)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.ShortInputLengthExcedeed));

            When(
                item => item.GrantFundingPurpose != null,
                () => RuleFor(item => item.GrantFundingPurpose)
                    .Must(value => value!.Length <= MaximumInputLength.LongInput)
                    .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));
        });

        RuleSet(ProjectView.ChargesDebt, () =>
        {
            When(
                    item => item.ChargesDebt == CommonResponse.Yes,
                    () => RuleFor(item => item.ChargesDebtInfo)
                            .NotEmpty()
                            .WithMessage(ValidationErrorMessage.EnterExistingLegal));

            When(
                item => item.ChargesDebtInfo != null,
                () => RuleFor(item => item.ChargesDebtInfo)
                    .Must(value => value!.Length <= MaximumInputLength.LongInput)
                    .WithMessage(ValidationErrorMessage.LongInputLengthExceeded));
        });

        RuleSet(ProjectView.Additional, () =>
        {
            When(
                item => item.PurchaseDay == null && item.PurchaseMonth == null && item.PurchaseYear == null,
                () => RuleFor(item => item.PurchaseDate)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.NoPurchaseDate));

            When(
                item => item.PurchaseDay == null && (item.PurchaseMonth != null || item.PurchaseYear != null),
                () => RuleFor(item => item.PurchaseDay)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.NoPurchaseDay));

            When(
                item => item.PurchaseMonth == null && (item.PurchaseDay != null || item.PurchaseYear != null),
                () => RuleFor(item => item.PurchaseMonth)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.NoPurchaseMonth));

            When(
                item => item.PurchaseYear == null && (item.PurchaseDay != null || item.PurchaseMonth != null),
                () => RuleFor(item => item.PurchaseYear)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.NoPurchaseYear));

            When(
                item => item.PurchaseDay != null && item.PurchaseMonth != null && item.PurchaseYear != null,
                () => RuleFor(item => item)
                .Must(model =>
                {
                    try
                    {
                        var dateString = $"{model.PurchaseDay}/{model.PurchaseMonth}/{model.PurchaseYear}";
                        return DateTime.TryParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage(ValidationErrorMessage.IncorrectPurchaseDate)
                .WithName(ProjectFormOption.PurchaseDate)
                .DependentRules(() => RuleFor(item => item).Must(model =>
                    {
                        var dateString = $"{model.PurchaseDay}/{model.PurchaseMonth}/{model.PurchaseYear}";
                        var providedDate = DateTime.ParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);

                        return providedDate.Date.IsBeforeOrEqualTo(DateTime.UtcNow.Date);
                    })
                    .WithMessage(ValidationErrorMessage.FuturePurchaseDate)
                    .WithName(ProjectFormOption.PurchaseDate)));

            When(
                item => item.Cost == null,
                () => RuleFor(item => item.Cost)
                .NotEmpty()
                        .WithMessage(ValidationErrorMessage.IncorrectProjectCost));

            When(
                item => item.Cost != null,
                () => RuleFor(item => item.Cost)
                        .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                        .WithMessage(ValidationErrorMessage.IncorrectProjectCost));

            When(
                item => item.Value == null,
                () => RuleFor(item => item.Value)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.IncorrectProjectValue));

            When(
                item => item.Value != null,
                () => RuleFor(item => item.Value)
                        .Matches(@"^[0-9]+([.,][0-9]{1,2})?$")
                        .WithMessage(ValidationErrorMessage.IncorrectProjectValue));

            When(
                item => item.Source == null,
                () => RuleFor(item => item.Source)
                        .NotEmpty()
                        .WithMessage(ValidationErrorMessage.EnterMoreDetails));
        });

        RuleSet(ProjectView.CheckAnswers, () =>
        {
            RuleFor(item => item.CheckAnswers)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.SecurityCheckAnswers);

            When(item => item.CheckAnswers == CommonResponse.Yes, () => RuleFor(m => m)
                    .Must(x => x.AllInformationIsProvided())
                    .WithMessage(ValidationErrorMessage.CheckAnswersOption)
                    .OverridePropertyName(nameof(SiteViewModel.CheckAnswers)));
        });
    }
}
