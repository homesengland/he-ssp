using System.ComponentModel;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public enum PublicGrantFields
{
    Undefined,

    [Description("amount you received from the county council")]
    CountyCouncilGrants,

    [Description("amount you received from DHSC Extra Care funding")]
    DhscExtraCareGrants,

    [Description("amount you received from local authority funding")]
    LocalAuthorityGrants,

    [Description("amount you received from social services funding")]
    SocialServicesGrants,

    [Description("amount you received from health-related bodies")]
    HealthRelatedGrants,

    [Description("amount you received from lottery funding")]
    LotteryGrants,

    [Description("amount you received from other public bodies")]
    OtherPublicBodiesGrants,
}
