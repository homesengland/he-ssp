using HE.Investment.AHP.Contract.Application;
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
        ProspectiveRentPercentage? prospectiveRentAsPercentageOfMarketRent = null,
        YesNoType targetRentExceedMarketRent = YesNoType.Undefined,
        YesNoType exemptFromTheRightToSharedOwnership = YesNoType.Undefined,
        MoreInformation? exemptionJustification = null,
        InitialSale? initialSale = null,
        ExpectedFirstTranche? expectedFirstTranche = null,
        ProspectiveRentPercentage? rentAsPercentageOfTheUnsoldShare = null)
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
        RentAsPercentageOfTheUnsoldShare = rentAsPercentageOfTheUnsoldShare;
    }

    public event EntityModifiedEventHandler? SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public MarketValue? MarketValue { get; private set; }

    public MarketRent? MarketRent { get; private set; }

    public ProspectiveRent? ProspectiveRent { get; private set; }

    public ProspectiveRentPercentage? ProspectiveRentAsPercentageOfMarketRent { get; private set; }

    public TargetRentExceedMarketRent? TargetRentExceedMarketRent { get; private set; }

    public bool IsProspectiveRentIneligible => (ProspectiveRentAsPercentageOfMarketRent?.Value > 80 && TargetRentExceedMarketRent?.Value == YesNoType.No)
                                               || RentAsPercentageOfTheUnsoldShare?.Value > 3;

    public YesNoType ExemptFromTheRightToSharedOwnership { get; private set; }

    public MoreInformation? ExemptionJustification { get; private set; }

    public InitialSale? InitialSale { get; private set; }

    public ExpectedFirstTranche? ExpectedFirstTranche { get; private set; }

    public ProspectiveRentPercentage? RentAsPercentageOfTheUnsoldShare { get; private set; }

    public void ChangeMarketValue(string? marketValue, bool isCalculation = false)
    {
        var newValue = new MarketValue(marketValue, isCalculation);

        MarketValue = _modificationTracker.Change(MarketValue, newValue);
    }

    public void ChangeMarketRent(string? marketRent, bool isCalculation = false)
    {
        var newValue = new MarketRent(marketRent, isCalculation);

        MarketRent = _modificationTracker.Change(MarketRent, newValue);
    }

    public void ChangeProspectiveRent(string? prospectiveRent, bool isCalculation = false)
    {
        var newValue = new ProspectiveRent(prospectiveRent, isCalculation);

        ProspectiveRent = _modificationTracker.Change(ProspectiveRent, newValue);
    }

    public void ChangeProspectiveRentAsPercentageOfMarketRent()
    {
        var result = CalculateProspectiveRent(MarketRent, ProspectiveRent);
        var newValue = new ProspectiveRentPercentage(result, nameof(ProspectiveRentAsPercentageOfMarketRent));

        ProspectiveRentAsPercentageOfMarketRent = _modificationTracker.Change(ProspectiveRentAsPercentageOfMarketRent, newValue);
    }

    public void ChangeTargetRentExceedMarketRent(YesNoType targetRentExceedMarketRent, bool isCalculation = false)
    {
        var newValue = new TargetRentExceedMarketRent(targetRentExceedMarketRent, isCalculation);

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

    public void ChangeProspectiveRentAsPercentageOfTheUnsoldShare()
    {
        var result = CalculateProspectiveRentAsPercentageOfTheUnsoldShare(MarketValue, ProspectiveRent, InitialSale);
        var newValue = new ProspectiveRentPercentage(result, nameof(RentAsPercentageOfTheUnsoldShare));

        RentAsPercentageOfTheUnsoldShare = _modificationTracker.Change(RentAsPercentageOfTheUnsoldShare, newValue);
    }

    public void ChangeExpectedFirstTranche()
    {
        var result = CalculateExpectedFirstTranche(MarketValue, InitialSale);

        var newValue = new ExpectedFirstTranche(result);
        ExpectedFirstTranche = _modificationTracker.Change(ExpectedFirstTranche, newValue);
    }

    public void ChangeInitialSale(string? initialSale, bool isCalculation = false)
    {
        var newValue = new InitialSale(initialSale, isCalculation);

        InitialSale = _modificationTracker.Change(InitialSale, newValue);
    }

    public decimal? CalculateProspectiveRent(MarketRent? marketRent, ProspectiveRent? prospectiveRent)
    {
        if (marketRent.IsNotProvided() || prospectiveRent.IsNotProvided())
        {
            return null;
        }

        if (marketRent!.Value == 0)
        {
            return 0;
        }

        var result = prospectiveRent!.Value / marketRent.Value * 100;
        result = Math.Round(result, 0);

        return result;
    }

    public decimal? CalculateExpectedFirstTranche(MarketValue? marketValue, InitialSale? initialSale)
    {
        if (marketValue.IsNotProvided() || initialSale.IsNotProvided())
        {
            return null;
        }

        return (marketValue!.Value * initialSale!.Value).RoundToTwoDecimalPlaces();
    }

    public decimal? CalculateProspectiveRentAsPercentageOfTheUnsoldShare(MarketValue? marketValue, ProspectiveRent? prospectiveRent, InitialSale? initialSale)
    {
        const int weeksAYear = 52;
        if (marketValue.IsNotProvided() || prospectiveRent.IsNotProvided() || initialSale.IsNotProvided())
        {
            return null;
        }

        if (marketValue!.Value == 0)
        {
            return 0;
        }

        var expectedFirstTranche = CalculateExpectedFirstTranche(marketValue, initialSale);

        var result = prospectiveRent!.Value * weeksAYear / (marketValue.Value - expectedFirstTranche!.Value) * 100m;
        result = Math.Round(result, 2);

        return result;
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
            ExemptionJustification,
            InitialSale,
            ExpectedFirstTranche,
            RentAsPercentageOfTheUnsoldShare);
    }

    public bool IsRequired(HousingType housingType)
    {
        return true;
    }

    public bool IsCompleted(HousingType housingType, Tenure tenure)
    {
        return tenure switch
        {
            Tenure.AffordableRent => IsAffordableRentTenureCompleted(),
            Tenure.SocialRent => IsSocialRentTenureCompleted(),
            Tenure.SharedOwnership => IsSharedOwnershipTenureCompleted(),
            Tenure.RentToBuy => IsRentToBuyTenureCompleted(),
            Tenure.HomeOwnershipLongTermDisabilities => IsHomeOwnershipDisabilitiesTenureCompleted(),
            Tenure.OlderPersonsSharedOwnership => IsOlderPersonsSharedOwnershipTenureCompleted(),
            _ => throw new ArgumentOutOfRangeException(nameof(tenure), tenure, "Not supported tenure"),
        };
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
    }

    public void ClearValuesForNewCalculation()
    {
        MarketValue = null;
        MarketRent = null;
        ProspectiveRent = null;
        InitialSale = null;
        ExpectedFirstTranche = null;
        ProspectiveRentAsPercentageOfMarketRent = null;
        RentAsPercentageOfTheUnsoldShare = null;
    }

    private bool IsExemptJustificationProvided()
    {
        if (ExemptFromTheRightToSharedOwnership == YesNoType.Yes)
        {
            return ExemptionJustification.IsProvided();
        }

        return true;
    }

    private bool IsAffordableRentTenureCompleted()
    {
        return MarketValue.IsProvided()
               && MarketRent.IsProvided()
               && ProspectiveRent.IsProvided()
               && TargetRentExceedMarketRent?.Value != YesNoType.Undefined
               && ExemptFromTheRightToSharedOwnership != YesNoType.Undefined
               && !IsProspectiveRentIneligible
               && IsExemptJustificationProvided();
    }

    private bool IsSocialRentTenureCompleted()
    {
        return MarketValue.IsProvided()
            && ProspectiveRent.IsProvided()
            && ExemptFromTheRightToSharedOwnership != YesNoType.Undefined
            && IsExemptJustificationProvided();
    }

    private bool IsSharedOwnershipTenureCompleted()
    {
        return MarketValue.IsProvided()
            && InitialSale.IsProvided()
            && ExpectedFirstTranche.IsProvided()
            && ProspectiveRent.IsProvided()
            && !IsProspectiveRentIneligible;
    }

    private bool IsRentToBuyTenureCompleted()
    {
        return MarketValue.IsProvided()
               && MarketRent.IsProvided()
               && ProspectiveRent.IsProvided()
               && TargetRentExceedMarketRent?.Value != YesNoType.Undefined
               && !IsProspectiveRentIneligible;
    }

    private bool IsHomeOwnershipDisabilitiesTenureCompleted()
    {
        return MarketValue.IsProvided()
               && InitialSale.IsProvided()
               && ExpectedFirstTranche.IsProvided()
               && ProspectiveRent.IsProvided()
               && !IsProspectiveRentIneligible;
    }

    private bool IsOlderPersonsSharedOwnershipTenureCompleted()
    {
        return MarketValue.IsProvided()
               && InitialSale.IsProvided()
               && ExpectedFirstTranche.IsProvided()
               && ProspectiveRent.IsProvided()
               && !IsProspectiveRentIneligible;
    }
}
