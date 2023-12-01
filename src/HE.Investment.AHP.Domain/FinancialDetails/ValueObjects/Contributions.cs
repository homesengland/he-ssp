using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class Contributions : ValueObject
{
    public Contributions(
        string? rentalIncomeBorrowing,
        string? salesOfHomesOnThisScheme,
        string? salesOfHomesOnOtherSchemes,
        string? ownResources,
        string? rCGFContributions,
        string? otherCapitalSources,
        string? sharedOwnershipSales,
        string? homesTransferValue)
    {
        SetValues(
            rentalIncomeBorrowing?.ToString(CultureInfo.InvariantCulture),
            salesOfHomesOnThisScheme?.ToString(CultureInfo.InvariantCulture),
            salesOfHomesOnOtherSchemes?.ToString(CultureInfo.InvariantCulture),
            ownResources?.ToString(CultureInfo.InvariantCulture),
            rCGFContributions?.ToString(CultureInfo.InvariantCulture),
            otherCapitalSources?.ToString(CultureInfo.InvariantCulture),
            sharedOwnershipSales?.ToString(CultureInfo.InvariantCulture),
            homesTransferValue?.ToString(CultureInfo.InvariantCulture))
            .CheckErrors();
    }

    public int? RentalIncomeBorrowing { get; private set; }

    public int? SalesOfHomesOnThisScheme { get; private set; }

    public int? SalesOfHomesOnOtherSchemes { get; private set; }

    public int? OwnResources { get; private set; }

    public int? RCGFContributions { get; private set; }

    public int? OtherCapitalSources { get; private set; }

    public int? SharedOwnershipSales { get; private set; }

    public int? HomesTransferValue { get; private set; }

    public static Contributions From(
        decimal? rentalIncomeBorrowing,
        decimal? salesOfHomesOnThisScheme,
        decimal? salesOfHomesOnOtherSchemes,
        decimal? ownResources,
        decimal? rCGFContributions,
        decimal? otherCapitalSources,
        decimal? sharedOwnershipSales,
        decimal? homesTransferValue)
    {
        return new Contributions(
            rentalIncomeBorrowing.ToWholeNumberString(),
            salesOfHomesOnThisScheme.ToWholeNumberString(),
            salesOfHomesOnOtherSchemes.ToWholeNumberString(),
            ownResources.ToWholeNumberString(),
            rCGFContributions.ToWholeNumberString(),
            otherCapitalSources.ToWholeNumberString(),
            sharedOwnershipSales.ToWholeNumberString(),
            homesTransferValue.ToWholeNumberString());
    }

    public void CheckErrors()
    {
        SetValues(
            RentalIncomeBorrowing?.ToString(CultureInfo.InvariantCulture),
            SalesOfHomesOnThisScheme?.ToString(CultureInfo.InvariantCulture),
            SalesOfHomesOnOtherSchemes?.ToString(CultureInfo.InvariantCulture),
            OwnResources?.ToString(CultureInfo.InvariantCulture),
            RCGFContributions?.ToString(CultureInfo.InvariantCulture),
            OtherCapitalSources?.ToString(CultureInfo.InvariantCulture),
            SharedOwnershipSales?.ToString(CultureInfo.InvariantCulture),
            HomesTransferValue?.ToString(CultureInfo.InvariantCulture)).CheckErrors();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return RentalIncomeBorrowing ?? null!;
        yield return SalesOfHomesOnThisScheme ?? null!;
        yield return SalesOfHomesOnOtherSchemes ?? null!;
        yield return OwnResources ?? null!;
        yield return RCGFContributions ?? null!;
        yield return OtherCapitalSources ?? null!;
        yield return SharedOwnershipSales ?? null!;
        yield return HomesTransferValue ?? null!;
    }

    private OperationResult SetValues(
        string? rentalIncomeBorrowing,
        string? salesOfHomesOnThisScheme,
        string? salesOfHomesOnOtherSchemes,
        string? ownResources,
        string? rCGFContributions,
        string? otherCapitalSources,
        string? sharedOwnershipSales,
        string? homesTransferValue,
        bool allowNulls = true)
    {
        var operationResult = OperationResult.New();

        RentalIncomeBorrowing = NumericValidator
            .For(rentalIncomeBorrowing, FinancialDetailsValidationFieldNames.RentalIncomeBorrowing, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        SalesOfHomesOnThisScheme = NumericValidator
            .For(salesOfHomesOnThisScheme, FinancialDetailsValidationFieldNames.SaleOfHomesOnThisScheme, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        SalesOfHomesOnOtherSchemes = NumericValidator
            .For(salesOfHomesOnOtherSchemes, FinancialDetailsValidationFieldNames.SaleOfHomesOnOtherSchemes, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        OwnResources = NumericValidator
            .For(ownResources, FinancialDetailsValidationFieldNames.OwnResources, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        RCGFContributions = NumericValidator
            .For(rCGFContributions, FinancialDetailsValidationFieldNames.RCGFContribution, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        OtherCapitalSources = NumericValidator
            .For(otherCapitalSources, FinancialDetailsValidationFieldNames.OtherCapitalSources, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        SharedOwnershipSales = NumericValidator
            .For(sharedOwnershipSales, FinancialDetailsValidationFieldNames.SharedOwnershipSales, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        HomesTransferValue = NumericValidator
            .For(homesTransferValue, FinancialDetailsValidationFieldNames.HomesTransferValue, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        return operationResult;
    }
}
