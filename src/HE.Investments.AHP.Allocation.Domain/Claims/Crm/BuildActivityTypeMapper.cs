using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Crm;

public static class BuildActivityTypeMapper
{
    public static BuildActivityType ToDomain(int? newBuildActivityType, int? rehabBuildActivityType)
    {
        if (newBuildActivityType.HasValue)
        {
            return MapNewBuildActivityType(newBuildActivityType.Value);
        }

        if (rehabBuildActivityType.HasValue)
        {
            return MapRehabBuildActivityType(rehabBuildActivityType.Value);
        }

        throw new ArgumentException($"One of {newBuildActivityType} or {rehabBuildActivityType} must be provided.", nameof(newBuildActivityType));
    }

    private static BuildActivityType MapNewBuildActivityType(int newBuildActivityType)
    {
        return newBuildActivityType switch
        {
            (int)invln_NewBuildActivityType.AcquisitionandWorks => BuildActivityType.AcquisitionAndWorks,
            (int)invln_NewBuildActivityType.LandInclusivePackagepackagedeal => BuildActivityType.LandInclusivePackage,
            (int)invln_NewBuildActivityType.OffTheShelf => BuildActivityType.OffTheShelf,
            (int)invln_NewBuildActivityType.WorksOnly => BuildActivityType.WorksOnly,
            (int)invln_NewBuildActivityType.Regeneration => BuildActivityType.Regeneration,
            _ => throw new ArgumentOutOfRangeException(nameof(newBuildActivityType), newBuildActivityType, null),
        };
    }

    private static BuildActivityType MapRehabBuildActivityType(int rehabBuildActivityType)
    {
        return rehabBuildActivityType switch
        {
            (int)invln_RehabActivityType.AcquisitionandWorksrehab => BuildActivityType.AcquisitionAndWorksRehab,
            (int)invln_RehabActivityType.Conversion => BuildActivityType.Conversion,
            (int)invln_RehabActivityType.ExistingSatisfactory => BuildActivityType.ExistingSatisfactory,
            (int)invln_RehabActivityType.LeaseandRepair => BuildActivityType.LeaseAndRepair,
            (int)invln_RehabActivityType.PurchaseandRepair => BuildActivityType.PurchaseAndRepair,
            (int)invln_RehabActivityType.Reimprovement => BuildActivityType.Reimprovement,
            (int)invln_RehabActivityType.WorksOnly => BuildActivityType.WorksOnlyRehab,
            _ => throw new ArgumentOutOfRangeException(nameof(rehabBuildActivityType), rehabBuildActivityType, null),
        };
    }
}
