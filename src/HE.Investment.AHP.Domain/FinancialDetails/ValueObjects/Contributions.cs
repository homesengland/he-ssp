using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    public int TotalContributions =>
        (RentalIncomeBorrowing ?? 0) +
        (SalesOfHomesOnThisScheme ?? 0) +
        (SalesOfHomesOnOtherSchemes ?? 0) +
        (OwnResources ?? 0) +
        (RCGFContributions ?? 0) +
        (OtherCapitalSources ?? 0) +
        (SharedOwnershipSales ?? 0) +
        (HomesTransferValue ?? 0);

    public bool IsAnyValueNotNull =>
        RentalIncomeBorrowing.HasValue ||
        SalesOfHomesOnThisScheme.HasValue ||
        SalesOfHomesOnOtherSchemes.HasValue ||
        OwnResources.HasValue ||
        RCGFContributions.HasValue ||
        OtherCapitalSources.HasValue ||
        SharedOwnershipSales.HasValue ||
        HomesTransferValue.HasValue;

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
            allowNulls,
            operationResult);

        SalesOfHomesOnThisScheme = CheckNullableIntValue(
            salesOfHomesOnThisScheme,
            FinancialDetailsValidationFieldNames.SaleOfHomesOnThisScheme,
            allowNulls,
            operationResult);

        SalesOfHomesOnOtherSchemes = CheckNullableIntValue(
            salesOfHomesOnOtherSchemes,
            FinancialDetailsValidationFieldNames.SaleOfHomesOnOtherSchemes,
            allowNulls,
            operationResult);

        OwnResources = CheckNullableIntValue(
            ownResources,
            FinancialDetailsValidationFieldNames.OwnResources,
            allowNulls,
            operationResult);

        RCGFContributions = CheckNullableIntValue(
            rCGFContributions,
            FinancialDetailsValidationFieldNames.RCGFContribution,
            allowNulls,
            operationResult);

        OtherCapitalSources = CheckNullableIntValue(
            otherCapitalSources,
            FinancialDetailsValidationFieldNames.OtherCapitalSources,
            allowNulls,
            operationResult);

        SharedOwnershipSales = CheckNullableIntValue(
            sharedOwnershipSales,
            FinancialDetailsValidationFieldNames.SharedOwnershipSales,
            allowNulls,
            operationResult);

        HomesTransferValue = CheckNullableIntValue(
            homesTransferValue,
            FinancialDetailsValidationFieldNames.HomesTransferValue,
            allowNulls,
            operationResult);

        return operationResult;
    }

    private int? CheckNullableIntValue(string? value, string fieldName, bool allowNull, OperationResult operationResult, string errorMsg = FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
        return value.TryParseNullableIntAndValidate(fieldName, errorMsg, allowNull, 0, 999999999, operationResult);
    }
}
