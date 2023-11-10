using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Application;

public enum Tenure
{
    Undefined = 0,

    [Description("Affordable Rent")]
    AffordableRent,

    [Description("Social Rent")]
    SocialRent,

    [Description("Shared Ownership")]
    SharedOwnership,

    [Description("Rent to Buy")]
    RentToBuy,

    [Description("Home Ownership for People with Long-term Disabilities (HOLD)")]
    HomeOwnershipLongTermDisabilities,

    [Description("Older Persons Shared Ownership (OPSO)")]
    OlderPersonsSharedOwnership,
}
