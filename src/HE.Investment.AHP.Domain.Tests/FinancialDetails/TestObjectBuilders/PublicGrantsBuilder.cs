using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.TestsUtils;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

public class PublicGrantsBuilder
{
    private readonly PublicGrants _item = new();

    private PublicGrantsBuilder()
    {
    }

    private PublicGrantsBuilder(PublicGrants item)
    {
        _item = item;
    }

    public static PublicGrantsBuilder New() => new();

    public static PublicGrantsBuilder NewWithAllValuesAs50()
    {
        var item = new PublicGrants(
            new PublicGrantValue(PublicGrantFields.CountyCouncilGrants, 50),
            new PublicGrantValue(PublicGrantFields.DhscExtraCareGrants, 50),
            new PublicGrantValue(PublicGrantFields.LocalAuthorityGrants, 50),
            new PublicGrantValue(PublicGrantFields.SocialServicesGrants, 50),
            new PublicGrantValue(PublicGrantFields.HealthRelatedGrants, 50),
            new PublicGrantValue(PublicGrantFields.LotteryGrants, 50),
            new PublicGrantValue(PublicGrantFields.OtherPublicBodiesGrants, 50));

        return new PublicGrantsBuilder(item);
    }

    public PublicGrantsBuilder WithHealthRelatedGrants(decimal ownResources)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.HealthRelated),
            new PublicGrantValue(PublicGrantFields.HealthRelatedGrants, ownResources));

        return this;
    }

    public PublicGrants Build() => _item;
}
