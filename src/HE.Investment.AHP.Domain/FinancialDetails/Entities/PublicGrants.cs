using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class PublicGrants : ValueObject, IQuestion
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

    public PublicGrantValue? CountyCouncil { get; }

    public PublicGrantValue? DhscExtraCare { get; }

    public PublicGrantValue? LocalAuthority { get; }

    public PublicGrantValue? SocialServices { get; }

    public PublicGrantValue? HealthRelated { get; }

    public PublicGrantValue? Lottery { get; }

    public PublicGrantValue? OtherPublicBodies { get; }

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

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CountyCouncil;
        yield return DhscExtraCare;
        yield return LocalAuthority;
        yield return SocialServices;
        yield return HealthRelated;
        yield return Lottery;
        yield return OtherPublicBodies;
    }
}
