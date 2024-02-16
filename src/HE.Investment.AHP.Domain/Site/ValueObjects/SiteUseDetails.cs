using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteUseDetails : ValueObject, IQuestion
{
    public SiteUseDetails(
        bool? isPartOfStreetFrontInfill = null,
        bool? isForTravellerPitchSite = null,
        TravellerPitchSiteType travellerPitchSiteType = TravellerPitchSiteType.Undefined)
    {
        IsPartOfStreetFrontInfill = isPartOfStreetFrontInfill;
        IsForTravellerPitchSite = isForTravellerPitchSite;
        TravellerPitchSiteType = travellerPitchSiteType;

        if (IsForTravellerPitchSite == false && TravellerPitchSiteType != TravellerPitchSiteType.Undefined)
        {
            OperationResult.ThrowValidationError(
                nameof(IsForTravellerPitchSite),
                "Traveller pitch site type cannot be provided when is for traveller pitch site is No.");
        }
    }

    public bool? IsPartOfStreetFrontInfill { get; }

    public bool? IsForTravellerPitchSite { get; }

    public TravellerPitchSiteType TravellerPitchSiteType { get; }

    public SiteUseDetails WithSiteUse(bool? isPartOfStreetFrontInfill = null, bool? isForTravellerPitchSite = null)
    {
        return new SiteUseDetails(
            isPartOfStreetFrontInfill,
            isForTravellerPitchSite,
            isForTravellerPitchSite == true ? TravellerPitchSiteType : TravellerPitchSiteType.Undefined);
    }

    public SiteUseDetails WithTravellerPitchSite(TravellerPitchSiteType travellerPitchSiteType)
    {
        return new SiteUseDetails(
            IsPartOfStreetFrontInfill,
            IsForTravellerPitchSite,
            travellerPitchSiteType);
    }

    public bool IsAnswered()
    {
        return IsPartOfStreetFrontInfill.IsProvided()
               && IsForTravellerPitchSite.IsProvided()
               && (IsForTravellerPitchSite == false || (IsForTravellerPitchSite == true && TravellerPitchSiteType != TravellerPitchSiteType.Undefined));
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsPartOfStreetFrontInfill;
        yield return IsForTravellerPitchSite;
        yield return TravellerPitchSiteType;
    }
}
