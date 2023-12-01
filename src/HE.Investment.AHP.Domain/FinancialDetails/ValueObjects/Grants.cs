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

        CountyCouncil = NumericValidator
            .For(countyCouncil, FinancialDetailsValidationFieldNames.CountyCouncilGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        DHSCExtraCare = NumericValidator
            .For(dHSCExtraCare, FinancialDetailsValidationFieldNames.DHSCExtraCareGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        LocalAuthority = NumericValidator
            .For(localAuthority, FinancialDetailsValidationFieldNames.LocalAuthorityGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        SocialServices = NumericValidator
            .For(socialServices, FinancialDetailsValidationFieldNames.SocialServicesGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        HealthRelated = NumericValidator
            .For(healthRelated, FinancialDetailsValidationFieldNames.HeatlthRelatedGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        Lottery = NumericValidator
            .For(lottery, FinancialDetailsValidationFieldNames.LotteryGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        OtherPublicBodies = NumericValidator
            .For(otherPublicBodies, FinancialDetailsValidationFieldNames.OtherPublicBodiesGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(0, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsConditionallyRequired(!allowNulls);

        return operationResult;
    }
}
