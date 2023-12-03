using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class Grants : ValueObject
{
    public Grants(
        string? countyCouncil,
        string? dHSCExtraCare,
        string? localAuthority,
        string? socialServices,
        string? healthRelated,
        string? lottery,
        string? otherPublicBodies)
    {
        SetValues(
            countyCouncil?.ToString(CultureInfo.InvariantCulture),
            dHSCExtraCare?.ToString(CultureInfo.InvariantCulture),
            localAuthority?.ToString(CultureInfo.InvariantCulture),
            socialServices?.ToString(CultureInfo.InvariantCulture),
            healthRelated?.ToString(CultureInfo.InvariantCulture),
            lottery?.ToString(CultureInfo.InvariantCulture),
            otherPublicBodies?.ToString(CultureInfo.InvariantCulture))
            .CheckErrors();
    }

    public int? CountyCouncil { get; private set; }

    public int? DHSCExtraCare { get; private set; }

    public int? LocalAuthority { get; private set; }

    public int? SocialServices { get; private set; }

    public int? HealthRelated { get; private set; }

    public int? Lottery { get; private set; }

    public int? OtherPublicBodies { get; private set; }

    public int TotalGrants =>
        (CountyCouncil ?? 0) +
        (DHSCExtraCare ?? 0) +
        (LocalAuthority ?? 0) +
        (SocialServices ?? 0) +
        (HealthRelated ?? 0) +
        (Lottery ?? 0) +
        (OtherPublicBodies ?? 0);

    public bool IsAnyValueNotNull =>
        CountyCouncil.HasValue ||
        DHSCExtraCare.HasValue ||
        LocalAuthority.HasValue ||
        SocialServices.HasValue ||
        HealthRelated.HasValue ||
        Lottery.HasValue ||
        OtherPublicBodies.HasValue;

    public static Grants From(
        decimal? countyCouncil,
        decimal? dHSCExtraCare,
        decimal? localAuthority,
        decimal? socialServices,
        decimal? healthRelated,
        decimal? lottery,
        decimal? otherPublicBodies)
    {
        return new Grants(
            countyCouncil.ToWholeNumberString(),
            dHSCExtraCare.ToWholeNumberString(),
            localAuthority.ToWholeNumberString(),
            socialServices.ToWholeNumberString(),
            healthRelated.ToWholeNumberString(),
            lottery.ToWholeNumberString(),
            otherPublicBodies.ToWholeNumberString());
    }

    public void CheckErrors()
    {
        SetValues(
            CountyCouncil?.ToString(CultureInfo.InvariantCulture),
            DHSCExtraCare?.ToString(CultureInfo.InvariantCulture),
            LocalAuthority?.ToString(CultureInfo.InvariantCulture),
            SocialServices?.ToString(CultureInfo.InvariantCulture),
            HealthRelated?.ToString(CultureInfo.InvariantCulture),
            Lottery?.ToString(CultureInfo.InvariantCulture),
            OtherPublicBodies?.ToString(CultureInfo.InvariantCulture),
            false).CheckErrors();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return CountyCouncil ?? null!;
        yield return DHSCExtraCare ?? null!;
        yield return LocalAuthority ?? null!;
        yield return SocialServices ?? null!;
        yield return HealthRelated ?? null!;
        yield return Lottery ?? null!;
        yield return OtherPublicBodies ?? null!;
    }

    private OperationResult SetValues(
        string? countyCouncil,
        string? dHSCExtraCare,
        string? localAuthority,
        string? socialServices,
        string? healthRelated,
        string? lottery,
        string? otherPublicBodies,
        bool allowNulls = true)
    {
        var operationResult = OperationResult.New();

        CountyCouncil = CheckNullableIntValue(
            countyCouncil,
            FinancialDetailsValidationFieldNames.CountyCouncilGrants,
            allowNulls,
            operationResult);

        DHSCExtraCare = CheckNullableIntValue(
            dHSCExtraCare,
            FinancialDetailsValidationFieldNames.DHSCExtraCareGrants,
            allowNulls,
            operationResult);

        LocalAuthority = CheckNullableIntValue(
            localAuthority,
            FinancialDetailsValidationFieldNames.LocalAuthorityGrants,
            allowNulls,
            operationResult);

        SocialServices = CheckNullableIntValue(
            socialServices,
            FinancialDetailsValidationFieldNames.SocialServicesGrants,
            allowNulls,
            operationResult);

        HealthRelated = CheckNullableIntValue(
            healthRelated,
            FinancialDetailsValidationFieldNames.HeatlthRelatedGrants,
            allowNulls,
            operationResult);

        Lottery = CheckNullableIntValue(
            lottery,
            FinancialDetailsValidationFieldNames.LotteryGrants,
            allowNulls,
            operationResult);

        OtherPublicBodies = CheckNullableIntValue(
            otherPublicBodies,
            FinancialDetailsValidationFieldNames.OtherPublicBodiesGrants,
            allowNulls,
            operationResult);

        return operationResult;
    }

    private int? CheckNullableIntValue(string? value, string fieldName, bool allowNull, OperationResult operationResult, string errorMsg = FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
        return value.TryParseNullableIntAndValidate(fieldName, errorMsg, allowNull, 0, 999999999, operationResult);
    }
}
