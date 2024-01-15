using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Views.DeliveryPhase.Consts;

public static class BuildActivityTypeOptions
{
    public static IList<ExtendedSelectListItem> GetBuildActivityTypesOptions(
        IList<BuildActivityType> buildActivityTypes,
        BuildActivityType? selectedOption)
    {
        return buildActivityTypes.Select(x => x.ToSelectListItem(selectedOption, BuildActivityTypeDescriptions[x])).ToList();
    }

    public static IDictionary<BuildActivityType, string> BuildActivityTypeDescriptions = new Dictionary<BuildActivityType, string>()
    {
        { BuildActivityType.AcquisitionAndWorksRehab, "Construction of new homes on land purchased by the provider without any public subsidy." },
        { BuildActivityType.AcquisitionAndWorks, "Construction of new homes on land purchased by the provider without any public subsidy." },
        {
            BuildActivityType.ExistingSatisfactory,
            "The acquisition of property on the open market that is already suitable as affordable housing and requires no or limited additional works."
        },
        {
            BuildActivityType.PurchaseAndRepair,
            "Second hand homes which require some repair to bring them to a standard suitable for affordable housing letting."
        },
        {
            BuildActivityType.LeaseAndRepair,
            "Acquisition on a short-term lease from a landlord on the open market that requires some degree of work in order to bring it up to standard. Lease must be for a minimum of 5 years."
        },
        {
            BuildActivityType.Reimprovement,
            "Property already in ownership of the provider who has purchased it with the help of public subsidy and already had some form of paid for improvement or conversion in the past."
        },
        {
            BuildActivityType.Conversion,
            "Refers to property conversion of a large family home into smaller flats. Property must already be owned by the provider."
        },
        {
            BuildActivityType.WorksOnlyRehab,
            "Construction of new homes on land already owned by the provider that was purchased with the help of public subsidy."
        },
        {
            BuildActivityType.WorksOnly,
            "Construction of new homes on land already owned by the provider and for which public subsidy has been received to acquire it."
        },
        {
            BuildActivityType.OffTheShelf,
            "Brand new completed homes, suitable for affordable housing, purchased from a housing developer, following an inspection and ready for immediate occupation."
        },
        { BuildActivityType.LandInclusivePackage, "Where land has been acquired from a developer who will also construct the new homes on the land." },
        { BuildActivityType.RegenerationRehab, "Rehabilitation of existing properties as part of an estate regeneration project." },
        { BuildActivityType.Regeneration, "Construction of additional new build homes as part of an estate regeneration project." },
    };

    private static ExtendedSelectListItem ToSelectListItem(
        this Enum buildActivityTypeForNewBuild,
        Enum? selectedOption,
        string hint)
    {
        return new(
            buildActivityTypeForNewBuild.GetDescription(),
            buildActivityTypeForNewBuild.ToString(),
            buildActivityTypeForNewBuild.Equals(selectedOption),
            hint);
    }
}
