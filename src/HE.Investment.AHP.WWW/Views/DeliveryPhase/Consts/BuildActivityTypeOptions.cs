using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Views.DeliveryPhase.Consts;

public static class BuildActivityTypeOptions
{
    public static IList<ExtendedSelectListItem> BuildActivityTypeForNewBuildOptions(BuildActivityTypeForNewBuild? selectedOption)
    {
        return new[]
        {
                BuildActivityTypeForNewBuild.AcquisitionAndWorks.ToSelectListItem(
                    selectedOption,
                    "Construction of new affordable homes on land purchased without the benefit of any public subsidy."),
                BuildActivityTypeForNewBuild.OffTheShelf.ToSelectListItem(
                    selectedOption,
                    "Brand new completed homes, suitable for affordable housing, purchased from a housing developer, following an inspection by a qualified person and ready for immediate occupation"),
                BuildActivityTypeForNewBuild.WorksOnly.ToSelectListItem(
                    selectedOption,
                    "Construction of new affordable homes on land already owned by the provider and for which public subsidy has been received to acquire it."),
                BuildActivityTypeForNewBuild.LandInclusivePackage.ToSelectListItem(
                    selectedOption,
                    "Where land has been acquired from a developer who will also construct the new affordable homes on the land."),
                BuildActivityTypeForNewBuild.Regeneration.ToSelectListItem(
                    selectedOption,
                    "Construction of additional new build homes as part of an estate regeneration project."),
        };
    }

    public static IList<ExtendedSelectListItem> BuildActivityTypeForRehabOptions(BuildActivityTypeForRehab? selectedOption)
    {
        return new[]
        {
                BuildActivityTypeForRehab.AcquisitionAndWorksRehab.ToSelectListItem(
                    selectedOption,
                    "Where a provider acquires a second-hand property on the open market for refurbishment or conversion."),
                BuildActivityTypeForRehab.ExistingSatisfactory.ToSelectListItem(
                    selectedOption,
                    "The acquisition of property on the open market that is already suitable as affordable housing and requires no or limited additional works."),
                BuildActivityTypeForRehab.PurchaseAndRepair.ToSelectListItem(
                    selectedOption,
                    "Acquisition outright of a second-hand property on the open market that requires some degree of works in order to bring it up to a lettable standard as affordable housing."),
                BuildActivityTypeForRehab.LeaseAndRepair.ToSelectListItem(
                    selectedOption,
                    "The acquisition on a short-term leased basis from a landlord on the open market that requires some degree of work in order to bring it up to a lettable standard as affordable housing.  Lease must be for a minimum of 5 years."),
                BuildActivityTypeForRehab.Reimprovement.ToSelectListItem(
                    selectedOption,
                    "The property must already be in the ownership of the provider who must have purchased it with the help of public subsidy and has already had some form of paid for improvement or conversion in the past.  The work may be improvement or conversion but not repairs."),
                BuildActivityTypeForRehab.Conversion.ToSelectListItem(
                    selectedOption,
                    "Refers to property conversion of a large family home into smaller flats.  Property must already be owned by the provider and priority."),
                BuildActivityTypeForRehab.WorksOnly.ToSelectListItem(
                    selectedOption,
                    "The property must already be in the ownership of the provider and is in need of rehabilitation, improvement or conversion."),
                BuildActivityTypeForRehab.Regeneration.ToSelectListItem(
                    selectedOption,
                    "Rehabilitation of existing properties as part of an estate regeneration project."),
        };
    }

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
