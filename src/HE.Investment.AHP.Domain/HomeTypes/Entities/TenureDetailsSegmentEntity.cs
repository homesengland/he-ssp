using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.TenureDetails)]
public class TenureDetailsSegmentEntity : DomainEntity, IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker;

    public TenureDetailsSegmentEntity(
        MarketValue? marketValue = null,
        MarketRentPerWeek? marketRentPerWeek = null,
        RentPerWeek? rentPerWeek = null,
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
        MarketRentPerWeek = marketRentPerWeek;
        RentPerWeek = rentPerWeek;
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

    public MarketRentPerWeek? MarketRentPerWeek { get; private set; }

    public RentPerWeek? RentPerWeek { get; private set; }

    public ProspectiveRentPercentage? ProspectiveRentAsPercentageOfMarketRent { get; private set; }

    public TargetRentExceedMarketRent? TargetRentExceedMarketRent { get; private set; }

    public bool IsProspectiveRentIneligible => (ProspectiveRentAsPercentageOfMarketRent?.Value > 0.8m && TargetRentExceedMarketRent?.Value == YesNoType.No)
                                               || RentAsPercentageOfTheUnsoldShare?.Value > 0.03m;

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

    public void ChangeMarketRentPerWeek(string? marketRentPerWeek, bool isCalculation = false)
    {
        var newValue = new MarketRentPerWeek(marketRentPerWeek, isCalculation);

        MarketRentPerWeek = _modificationTracker.Change(MarketRentPerWeek, newValue);
    }

    public void ChangeRentPerWeek(string? rentPerWeek, string? rentType = null, bool isCalculation = false)
    {
        var newValue = new RentPerWeek(rentPerWeek, isCalculation, rentType);

        RentPerWeek = _modificationTracker.Change(RentPerWeek, newValue);
    }

    public void ChangeRentAsPercentageOfMarketRent()
    {
        var result = CalculateRentAsPercentageOfMarketRent(MarketRentPerWeek, RentPerWeek);
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
        ExemptFromTheRightToSharedOwnership = _modificationTracker.Change(
            ExemptFromTheRightToSharedOwnership,
            exemptFromTheRightToSharedOwnership,
            onChangedWithParamList: ClearExemptJustification);
    }

    public void ChangeExemptionJustification(MoreInformation? newValue)
    {
        ExemptionJustification = _modificationTracker.Change(ExemptionJustification, newValue);
    }

    public void ChangeRentAsPercentageOfTheUnsoldShare()
    {
        var result = CalculateRentAsPercentageOfTheUnsoldShare(MarketValue, RentPerWeek, InitialSale);
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

    public decimal? CalculateRentAsPercentageOfMarketRent(MarketRentPerWeek? marketRentPerWeek, RentPerWeek? rentPerWeek)
    {
        if (marketRentPerWeek.IsNotProvided() || rentPerWeek.IsNotProvided())
        {
            return null;
        }

        if (marketRentPerWeek!.Value == 0)
        {
            return 0;
        }

        return (rentPerWeek!.Value / marketRentPerWeek.Value).RoundToTwoDecimalPlaces();
    }

    public decimal? CalculateExpectedFirstTranche(MarketValue? marketValue, InitialSale? initialSale)
    {
        if (marketValue.IsNotProvided() || initialSale.IsNotProvided())
        {
            return null;
        }

        return (marketValue!.Value * initialSale!.Value).RoundToTwoDecimalPlaces();
    }

    public decimal? CalculateRentAsPercentageOfTheUnsoldShare(MarketValue? marketValue, RentPerWeek? rentPerWeek, InitialSale? initialSale)
    {
        const int weeksAYear = 52;
        if (marketValue.IsNotProvided() || rentPerWeek.IsNotProvided() || initialSale.IsNotProvided())
        {
            return null;
        }

        if (marketValue!.Value == 0)
        {
            return 0;
        }

        var expectedFirstTranche = CalculateExpectedFirstTranche(marketValue, initialSale);
        return (rentPerWeek!.Value * weeksAYear / (marketValue.Value - expectedFirstTranche!.Value)).RoundToFourDecimalPlaces();
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new TenureDetailsSegmentEntity(
            MarketValue,
            MarketRentPerWeek,
            RentPerWeek,
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
        MarketRentPerWeek = null;
        RentPerWeek = null;
        InitialSale = null;
        ExpectedFirstTranche = null;
        ProspectiveRentAsPercentageOfMarketRent = null;
        RentAsPercentageOfTheUnsoldShare = null;
    }

    private void ClearExemptJustification(YesNoType exemptFromTheRightToSharedOwnership)
    {
        if (exemptFromTheRightToSharedOwnership != YesNoType.Yes)
        {
            ChangeExemptionJustification(null);
        }
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
               && MarketRentPerWeek.IsProvided()
               && RentPerWeek.IsProvided()
               && TargetRentExceedMarketRent?.Value != YesNoType.Undefined
               && ExemptFromTheRightToSharedOwnership != YesNoType.Undefined
               && !IsProspectiveRentIneligible
               && IsExemptJustificationProvided();
    }

    private bool IsSocialRentTenureCompleted()
    {
        return MarketValue.IsProvided()
            && RentPerWeek.IsProvided()
            && ExemptFromTheRightToSharedOwnership != YesNoType.Undefined
            && IsExemptJustificationProvided();
    }

    private bool IsSharedOwnershipTenureCompleted()
    {
        return MarketValue.IsProvided()
            && InitialSale.IsProvided()
            && ExpectedFirstTranche.IsProvided()
            && RentPerWeek.IsProvided()
            && !IsProspectiveRentIneligible;
    }

    private bool IsRentToBuyTenureCompleted()
    {
        return MarketValue.IsProvided()
               && MarketRentPerWeek.IsProvided()
               && RentPerWeek.IsProvided()
               && TargetRentExceedMarketRent?.Value != YesNoType.Undefined
               && !IsProspectiveRentIneligible;
    }

    private bool IsHomeOwnershipDisabilitiesTenureCompleted()
    {
        return MarketValue.IsProvided()
               && InitialSale.IsProvided()
               && ExpectedFirstTranche.IsProvided()
               && RentPerWeek.IsProvided()
               && !IsProspectiveRentIneligible;
    }

    private bool IsOlderPersonsSharedOwnershipTenureCompleted()
    {
        return MarketValue.IsProvided()
               && InitialSale.IsProvided()
               && ExpectedFirstTranche.IsProvided()
               && RentPerWeek.IsProvided()
               && !IsProspectiveRentIneligible;
    }
}
