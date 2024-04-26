using System.ComponentModel;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public enum PublicGrantFields
{
    Undefined,

    [Description("county council public grant value")]
    CountyCouncilGrants,

    [Description("DHSC Extra Care funding grant value")]
    DhscExtraCareGrants,

    [Description("local authority funding grant value")]
    LocalAuthorityGrants,

    [Description("social services funding grant value")]
    SocialServicesGrants,

    [Description("health-related bodies grant value")]
    HealthRelatedGrants,

    [Description("lottery funding grant value")]
    LotteryGrants,

    [Description("other public bodies grant value")]
    OtherPublicBodiesGrants,
}
