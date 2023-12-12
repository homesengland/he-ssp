using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class PublicGrants
{
    public PublicGrants(
        PublicGrantValue? countyCouncil,
        PublicGrantValue? dhscExtraCare,
        PublicGrantValue? localAuthority,
        PublicGrantValue? socialServices,
        PublicGrantValue? healthRelated,
        PublicGrantValue? lottery,
        PublicGrantValue? otherPublicBodies)
    {
        CountyCouncil = countyCouncil;
        DhscExtraCare = dhscExtraCare;
        LocalAuthority = localAuthority;
        SocialServices = socialServices;
        HealthRelated = healthRelated;
        Lottery = lottery;
        OtherPublicBodies = otherPublicBodies;
    }

    public PublicGrantValue? CountyCouncil { get; private set; }

    public PublicGrantValue? DhscExtraCare { get; private set; }

    public PublicGrantValue? LocalAuthority { get; private set; }

    public PublicGrantValue? SocialServices { get; private set; }

    public PublicGrantValue? HealthRelated { get; private set; }

    public PublicGrantValue? Lottery { get; private set; }

    public PublicGrantValue? OtherPublicBodies { get; private set; }

    public bool IsAnswered()
    {
        return CountyCouncil.IsProvided() && DhscExtraCare.IsProvided() && LocalAuthority.IsProvided() &&
               SocialServices.IsProvided() && HealthRelated.IsProvided() && Lottery.IsProvided() &&
               OtherPublicBodies.IsProvided();
    }

    public decimal CalculateTotal()
    {
        var totalGrants = CountyCouncil.GetValueOrZero() + DhscExtraCare.GetValueOrZero() + LocalAuthority.GetValueOrZero() + SocialServices.GetValueOrZero() +
                          HealthRelated.GetValueOrZero() + Lottery.GetValueOrZero() + OtherPublicBodies.GetValueOrZero();

        return totalGrants;
    }
}
