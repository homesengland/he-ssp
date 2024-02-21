using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Mappers;

public static class PublicGrantsMappers
{
    public static PublicGrants MapToPublicGrants(AhpApplicationDto application)
    {
        static PublicGrantValue? MapProvidedValues(decimal? value, PublicGrantFields field) => value.IsProvided()
            ? new PublicGrantValue(field, value!.Value)
            : null;

        return new PublicGrants(
            MapProvidedValues(application.howMuchReceivedFromCountyCouncil, PublicGrantFields.CountyCouncilGrants),
            MapProvidedValues(application.howMuchReceivedFromDhscExtraCareFunding, PublicGrantFields.DhscExtraCareGrants),
            MapProvidedValues(application.howMuchReceivedFromLocalAuthority1, PublicGrantFields.LocalAuthorityGrants),
            MapProvidedValues(application.howMuchReceivedFromSocialServices, PublicGrantFields.SocialServicesGrants),
            MapProvidedValues(application.howMuchReceivedFromDepartmentOfHealth, PublicGrantFields.HealthRelatedGrants),
            MapProvidedValues(application.howMuchReceivedFromLotteryFunding, PublicGrantFields.LotteryGrants),
            MapProvidedValues(application.howMuchReceivedFromOtherPublicBodies, PublicGrantFields.OtherPublicBodiesGrants));
    }

    public static void MapFromPublicGrants(PublicGrants publicGrants, AhpApplicationDto dto)
    {
        dto.howMuchReceivedFromCountyCouncil = publicGrants.CountyCouncil?.Value;
        dto.howMuchReceivedFromDhscExtraCareFunding = publicGrants.DhscExtraCare?.Value;
        dto.howMuchReceivedFromLocalAuthority1 = publicGrants.LocalAuthority?.Value;
        dto.howMuchReceivedFromSocialServices = publicGrants.SocialServices?.Value;
        dto.howMuchReceivedFromDepartmentOfHealth = publicGrants.HealthRelated?.Value;
        dto.howMuchReceivedFromLotteryFunding = publicGrants.Lottery?.Value;
        dto.howMuchReceivedFromOtherPublicBodies = publicGrants.OtherPublicBodies?.Value;
    }
}
