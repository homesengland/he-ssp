using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Application;

public static class Tenures
{
    private static readonly Dictionary<Tenure, string> TenuresList = new()
    {
        {
            Tenure.AffordableRent, "Homes let to working households at a lower cost to give them the opportunity to save for a deposit to but their first home."
        },
        {
            Tenure.SocialRent,
            "Social rent is low-cost rental social housing that is let at below market rent by a registered provider based on a formula set by the government."
        },
        { Tenure.SharedOwnership, "Shared Ownership allows a customer to purchase a share of a home and pay rent on the remaining share." },
        {
            Tenure.RentToBuy,
            "Homes let at below market rent by a registered provider. The rent, including service charge, is set at up to 80% of the local market rent for an equivalent home."
        },
        {
            Tenure.HomeOwnershipLongTermDisabilities,
            "HOLD is a form of Shared Ownership that enable providers to purchase homes on the open market that meet the specific requirements of the homeowner."
        },
        { Tenure.OlderPersonsSharedOwnership, "OPSO is a form of Shared Ownership for people aged 55 and over." },
    };

    public static IList<ExtendedSelectListItem> AvailableTenures => TenuresList.ToExtendedSelectList();
}
