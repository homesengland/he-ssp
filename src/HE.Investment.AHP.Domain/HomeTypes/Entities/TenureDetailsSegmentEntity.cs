using System.Globalization;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.TenureDetails)]
public class TenureDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker;

    public TenureDetailsSegmentEntity(
        MarketValue? marketValue = null,
        MarketRent? marketRent = null,
        ProspectiveRent? prospectiveRent = null,
        Percentage? prospectiveRentAsPercentageOfMarketRent = null,
        YesNoType targetRentExceedMarketRent = YesNoType.Undefined,
        YesNoType exemptFromTheRightToSharedOwnership = YesNoType.Undefined,
        MoreInformation? exemptionJustification = null,
        InitialSale? initialSale = null,
        ExpectedFirstTranche? expectedFirstTranche = null,
        Percentage? sharedOwnershipRentAsPercentageOfTheUnsoldShare = null)
    {
        _modificationTracker = new ModificationTracker(() => SegmentModified?.Invoke());
        MarketValue = marketValue;
        MarketRent = marketRent;
        ProspectiveRent = prospectiveRent;
        ProspectiveRentAsPercentageOfMarketRent = prospectiveRentAsPercentageOfMarketRent;
        TargetRentExceedMarketRent = new TargetRentExceedMarketRent(targetRentExceedMarketRent);
        ExemptFromTheRightToSharedOwnership = exemptFromTheRightToSharedOwnership;
        ExemptionJustification = exemptionJustification;
        InitialSale = initialSale;
        ExpectedFirstTranche = expectedFirstTranche;
        SharedOwnershipRentAsPercentageOfTheUnsoldShare = sharedOwnershipRentAsPercentageOfTheUnsoldShare;
    }

    public event EntityModifiedEventHandler? SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public MarketValue? MarketValue { get; private set; }

    public MarketRent? MarketRent { get; private set; }

    public ProspectiveRent? ProspectiveRent { get; private set; }

    public Percentage? ProspectiveRentAsPercentageOfMarketRent { get; private set; }

    public TargetRentExceedMarketRent? TargetRentExceedMarketRent { get; private set; }

    public bool IsExceeding80PercentOfMarketRent { get; private set; }

    public YesNoType ExemptFromTheRightToSharedOwnership { get; private set; }

    public MoreInformation? ExemptionJustification { get; private set; }

    public InitialSale? InitialSale { get; private set; }

    public ExpectedFirstTranche? ExpectedFirstTranche { get; private set; }

    public Percentage? SharedOwnershipRentAsPercentageOfTheUnsoldShare { get; private set; }

    public bool IsExceeding3PercentOfUnsoldShare { get; private set; }

    public static decimal CalculateProspectiveRent(string? marketRent, string? prospectiveRent)
    {
        var result = 00.00m;

        if (decimal.TryParse(marketRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedMarketRent)
            && decimal.TryParse(prospectiveRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedProspectiveRent)
            && decimal.Round(parsedMarketRent, 2) == parsedMarketRent
            && decimal.Round(parsedProspectiveRent, 2) == parsedProspectiveRent)
        {
            result = parsedProspectiveRent / parsedMarketRent * 100;
            result = Math.Round(result, 2);
        }

        return result;
    }

    public static decimal CalculateExpectedFirstTranche(string? marketValue, string? initialSalePercentage)
    {
        var result = 00.00m;

        if (decimal.TryParse(marketValue!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedMarketValue)
            && decimal.TryParse(initialSalePercentage!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedInitialSalePercentage)
            && decimal.Round(parsedMarketValue, 2) == parsedMarketValue
            && decimal.Round(parsedInitialSalePercentage, 2) == parsedInitialSalePercentage)
        {
            result = parsedMarketValue * parsedInitialSalePercentage / 100;
            result = Math.Round(result, 2);
        }

        return result;
    }

    public static decimal CalculateProspectiveRentAsPercentageOfTheUnsoldShare(string? marketValue, string? prospectiveRent, string? initialSalePercentage)
    {
        const int weeksAYear = 52;
        var result = 00.00m;

        var expectedFirstTranche = CalculateExpectedFirstTranche(marketValue, initialSalePercentage);

        if (decimal.TryParse(marketValue!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedMarketValue)
            && decimal.TryParse(prospectiveRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedProspectiveRent)
            && decimal.Round(parsedMarketValue, 2) == parsedMarketValue
            && decimal.Round(parsedProspectiveRent, 2) == parsedProspectiveRent)
        {
            result = parsedProspectiveRent * weeksAYear / (parsedMarketValue - expectedFirstTranche);
            result = Math.Round(result, 2);
        }

        return result;
    }

    public void ChangeMarketValue(string? marketValue, bool isCalculation = false)
    {
        var newValue = marketValue.IsProvided() || isCalculation
            ? new MarketValue(marketValue, isCalculation)
            : null;

        MarketValue = _modificationTracker.Change(MarketValue, newValue);
    }

    public void ChangeMarketRent(string? marketRent, bool isCalculation = false)
    {
        var newValue = marketRent.IsProvided() || isCalculation
            ? new MarketRent(marketRent, isCalculation)
            : null;

        MarketRent = _modificationTracker.Change(MarketRent, newValue);
    }

    public void ChangeProspectiveRent(string? prospectiveRent, bool isCalculation = false)
    {
        var newValue = prospectiveRent.IsProvided() || isCalculation
            ? new ProspectiveRent(prospectiveRent, isCalculation)
            : null;

        ProspectiveRent = _modificationTracker.Change(ProspectiveRent, newValue);
    }

    public void ChangeProspectiveRentAsPercentageOfMarketRent(string? marketRent, string? prospectiveRent)
    {
        const int exceed80PercentOfMarketRent = 80;
        var result = CalculateProspectiveRent(marketRent, prospectiveRent);

        var newValue = new Percentage(result);
        ProspectiveRentAsPercentageOfMarketRent = _modificationTracker.Change(ProspectiveRentAsPercentageOfMarketRent, newValue);

        IsExceeding80PercentOfMarketRent = result > exceed80PercentOfMarketRent && TargetRentExceedMarketRent?.Value == YesNoType.No;
    }

    public void ChangeTargetRentExceedMarketRent(YesNoType targetRentExceedMarketRent, bool isCalculation = false)
    {
        var newValue = targetRentExceedMarketRent.IsProvided() || isCalculation
            ? new TargetRentExceedMarketRent(targetRentExceedMarketRent, isCalculation)
            : null;

        TargetRentExceedMarketRent = _modificationTracker.Change(TargetRentExceedMarketRent, newValue);
    }

    public void ChangeExemptFromTheRightToSharedOwnership(YesNoType exemptFromTheRightToSharedOwnership)
    {
        ExemptFromTheRightToSharedOwnership = _modificationTracker.Change(ExemptFromTheRightToSharedOwnership, exemptFromTheRightToSharedOwnership);
    }

    public void ChangeExemptionJustification(MoreInformation? newValue)
    {
        ExemptionJustification = _modificationTracker.Change(ExemptionJustification, newValue);
    }

    public void ChangeProspectiveRentAsPercentageOfTheUnsoldShare(string? marketValue, string? prospectiveRent, string? initialSalePercentage)
    {
        const int exceed3PercentOfUnsoldShare = 3;
        var result = CalculateProspectiveRentAsPercentageOfTheUnsoldShare(marketValue, prospectiveRent, initialSalePercentage);

        var newValue = new Percentage(result);
        SharedOwnershipRentAsPercentageOfTheUnsoldShare = _modificationTracker.Change(SharedOwnershipRentAsPercentageOfTheUnsoldShare, newValue);

        IsExceeding3PercentOfUnsoldShare = result > exceed3PercentOfUnsoldShare;
    }

    public void ChangeExpectedFirstTranche(string? marketValue, string? initialSalePercentage)
    {
        var result = CalculateExpectedFirstTranche(marketValue, initialSalePercentage);

        var newValue = new ExpectedFirstTranche(result);
        ExpectedFirstTranche = _modificationTracker.Change(ExpectedFirstTranche, newValue);
    }

    public void ChangeInitialSale(string? initialSale, bool isCalculation = false)
    {
        var newValue = initialSale.IsProvided() || isCalculation
            ? new InitialSale(initialSale, isCalculation)
            : null;

        InitialSale = _modificationTracker.Change(InitialSale, newValue);
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new TenureDetailsSegmentEntity(
            MarketValue,
            MarketRent,
            ProspectiveRent,
            ProspectiveRentAsPercentageOfMarketRent,
            TargetRentExceedMarketRent.IsNotProvided() ? YesNoType.Undefined : TargetRentExceedMarketRent!.Value,
            ExemptFromTheRightToSharedOwnership,
            ExemptionJustification);
    }

    public bool IsRequired(HousingType housingType)
    {
        return true;
    }

    public bool IsCompleted()
    {
        return MarketValue.IsProvided()
               && MarketRent.IsProvided()
               && ProspectiveRent.IsProvided()
               && ProspectiveRentAsPercentageOfMarketRent.IsProvided()
               && TargetRentExceedMarketRent?.Value != YesNoType.Undefined
               && ExemptFromTheRightToSharedOwnershipCompletion();
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
        if (targetHousingType is HousingType.Undefined or HousingType.General)
        {
            ChangeMarketValue(null);
            ChangeMarketRent(null);
            ChangeProspectiveRent(null);
            ChangeProspectiveRentAsPercentageOfMarketRent(MarketRent?.ToString(), ProspectiveRent?.ToString());
            ChangeTargetRentExceedMarketRent(YesNoType.Undefined);
            ChangeExemptFromTheRightToSharedOwnership(YesNoType.Undefined);
            ChangeExemptionJustification(null);
        }
    }

    private bool ExemptFromTheRightToSharedOwnershipCompletion()
    {
        return ExemptFromTheRightToSharedOwnership == YesNoType.No
            ? ExemptionJustification.IsProvided()
            : ExemptFromTheRightToSharedOwnership != YesNoType.Undefined;
    }
}
