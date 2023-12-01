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

        RentalIncomeBorrowing = CheckNullableIntValue(
            rentalIncomeBorrowing,
            FinancialDetailsValidationFieldNames.RentalIncomeBorrowing,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        SalesOfHomesOnThisScheme = CheckNullableIntValue(
            salesOfHomesOnThisScheme,
            FinancialDetailsValidationFieldNames.SaleOfHomesOnThisScheme,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        SalesOfHomesOnOtherSchemes = CheckNullableIntValue(
            salesOfHomesOnOtherSchemes,
            FinancialDetailsValidationFieldNames.SaleOfHomesOnOtherSchemes,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        OwnResources = CheckNullableIntValue(
            ownResources,
            FinancialDetailsValidationFieldNames.OwnResources,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        RCGFContributions = CheckNullableIntValue(
            rCGFContributions,
            FinancialDetailsValidationFieldNames.RCGFContribution,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        OtherCapitalSources = CheckNullableIntValue(
            otherCapitalSources,
            FinancialDetailsValidationFieldNames.OtherCapitalSources,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        SharedOwnershipSales = CheckNullableIntValue(
            sharedOwnershipSales,
            FinancialDetailsValidationFieldNames.SharedOwnershipSales,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        HomesTransferValue = CheckNullableIntValue(
            homesTransferValue,
            FinancialDetailsValidationFieldNames.HomesTransferValue,
            FinancialDetailsValidationErrors.GenericAmountValidationError,
            allowNulls,
            operationResult);

        return operationResult;
    }

    private int? CheckNullableIntValue(string? value, string fieldName, string errorMsg, bool allowNull, OperationResult operationResult)
    {
        return value.TryParseNullableIntAndValidate(fieldName, errorMsg, allowNull, 0, 999999999, operationResult);
    }
}
