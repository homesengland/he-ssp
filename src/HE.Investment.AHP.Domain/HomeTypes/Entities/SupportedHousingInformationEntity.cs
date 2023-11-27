using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.SupportedHousingInformation)]
public class SupportedHousingInformationEntity : IHomeTypeSegmentEntity
{
    private readonly List<RevenueFundingSourceType> _revenueFundingSources;

    private readonly ModificationTracker _modificationTracker = new();

    public SupportedHousingInformationEntity(
        YesNoType localCommissioningBodiesConsulted = YesNoType.Undefined,
        YesNoType shortStayAccommodation = YesNoType.Undefined,
        RevenueFundingType revenueFundingType = RevenueFundingType.Undefined,
        IEnumerable<RevenueFundingSourceType>? revenueFundingSources = null,
        MoreInformation? moveOnArrangements = null,
        MoreInformation? typologyLocationAndDesign = null,
        MoreInformation? exitPlan = null)
    {
        LocalCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
        ShortStayAccommodation = shortStayAccommodation;
        RevenueFundingType = revenueFundingType;
        _revenueFundingSources = revenueFundingSources?.ToList() ?? new List<RevenueFundingSourceType>();
        MoveOnArrangements = moveOnArrangements;
        TypologyLocationAndDesign = typologyLocationAndDesign;
        ExitPlan = exitPlan;
    }

    public bool IsModified => _modificationTracker.IsModified;

    public YesNoType LocalCommissioningBodiesConsulted { get; private set; }

    public YesNoType ShortStayAccommodation { get; private set; }

    public RevenueFundingType RevenueFundingType { get; private set; }

    public IReadOnlyCollection<RevenueFundingSourceType> RevenueFundingSources => _revenueFundingSources;

    public MoreInformation? MoveOnArrangements { get; private set; }

    public MoreInformation? TypologyLocationAndDesign { get; private set; }

    public MoreInformation? ExitPlan { get; private set; }

    public void ChangeLocalCommissioningBodiesConsulted(YesNoType localCommissioningBodiesConsulted)
    {
        LocalCommissioningBodiesConsulted = _modificationTracker.Change(LocalCommissioningBodiesConsulted, localCommissioningBodiesConsulted);
    }

    public void ChangeShortStayAccommodation(YesNoType shortStayAccommodation)
    {
        ShortStayAccommodation = _modificationTracker.Change(ShortStayAccommodation, shortStayAccommodation);
    }

    public void ChangeRevenueFundingType(RevenueFundingType revenueFundingType)
    {
        RevenueFundingType = _modificationTracker.Change(RevenueFundingType, revenueFundingType);
    }

    public void ChangeSources(IEnumerable<RevenueFundingSourceType> revenueFundingSources)
    {
        var uniqueRevenueFundingSources = revenueFundingSources.Distinct().ToList();

        if (!_revenueFundingSources.SequenceEqual(uniqueRevenueFundingSources))
        {
            _revenueFundingSources.Clear();
            _revenueFundingSources.AddRange(uniqueRevenueFundingSources);
            _modificationTracker.MarkAsModified();
        }
    }

    public void ChangeMoveOnArrangements(MoreInformation? newValue)
    {
        MoveOnArrangements = _modificationTracker.Change(MoveOnArrangements, newValue);
    }

    public void ChangeTypologyLocationAndDesign(MoreInformation? newValue)
    {
        TypologyLocationAndDesign = _modificationTracker.Change(TypologyLocationAndDesign, newValue);
    }

    public void ChangeExitPlan(MoreInformation? newValue)
    {
        ExitPlan = _modificationTracker.Change(ExitPlan, newValue);
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new SupportedHousingInformationEntity(
            LocalCommissioningBodiesConsulted,
            ShortStayAccommodation,
            RevenueFundingType,
            RevenueFundingSources,
            MoveOnArrangements,
            TypologyLocationAndDesign,
            ExitPlan);
    }

    public bool IsRequired(HousingType housingType)
    {
        return housingType is HousingType.HomesForOlderPeople or HousingType.HomesForDisabledAndVulnerablePeople;
    }

    public bool IsCompleted()
    {
        return LocalCommissioningBodiesConsulted != YesNoType.Undefined
               && ShortStayAccommodation != YesNoType.Undefined
               && RevenueFundingType != RevenueFundingType.Undefined
               && ConditionalRoutesAreCompleted();
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
        if (targetHousingType is HousingType.Undefined or HousingType.General)
        {
            ChangeLocalCommissioningBodiesConsulted(YesNoType.Undefined);
            ChangeShortStayAccommodation(YesNoType.Undefined);
            ChangeRevenueFundingType(RevenueFundingType.Undefined);
            ChangeSources(Enumerable.Empty<RevenueFundingSourceType>());
            ChangeMoveOnArrangements(null);
            ChangeTypologyLocationAndDesign(null);
            ChangeExitPlan(null);
        }
    }

    private bool ConditionalRoutesAreCompleted()
    {
        if (RevenueFundingType == RevenueFundingType.RevenueFundingNeededAndIdentified && ShortStayAccommodation == YesNoType.Yes)
        {
            return RevenueFundingSources.Any() && MoveOnArrangements.IsProvided() && ExitPlan.IsProvided() && TypologyLocationAndDesign.IsProvided();
        }
        else if (RevenueFundingType == RevenueFundingType.RevenueFundingNeededAndIdentified && ShortStayAccommodation == YesNoType.No)
        {
            return RevenueFundingSources.Any() && TypologyLocationAndDesign.IsProvided();
        }
        else if (RevenueFundingType != RevenueFundingType.RevenueFundingNeededAndIdentified && ShortStayAccommodation == YesNoType.No)
        {
            return ExitPlan.IsProvided() && TypologyLocationAndDesign.IsProvided();
        }
        else if (RevenueFundingType != RevenueFundingType.RevenueFundingNeededAndIdentified && ShortStayAccommodation == YesNoType.Yes)
        {
            return MoveOnArrangements.IsProvided() && ExitPlan.IsProvided() && TypologyLocationAndDesign.IsProvided();
        }
        else
        {
            return false;
        }
    }
}
