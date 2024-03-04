using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.WWW.Models;

public static class ProjectFormOptions
{
    private static readonly Dictionary<ActivityType, string> ActivityTypesWithDescription = new()
    {
        { ActivityType.DevelopingHomes, "This includes new build and regeneration." },
        { ActivityType.ProvidingInfrastructure, "Support would be provided directly for the provision of infrastructure only (such as remediation, access or utilities) and not for the direct development of housing." },
        { ActivityType.ManufacturingHomesWithinFactory, string.Empty },
        { ActivityType.AcquiringLand, string.Empty },
        { ActivityType.SellingLandToHomesEngland, string.Empty },
    };

    public static IEnumerable<ExtendedSelectListItem> ActivityTypes => ActivityTypesWithDescription.ToExtendedSelectList();

    public static ExtendedSelectListItem ActivityTypeOther => SelectListHelper.FromEnumToExtendedListItem(ActivityType.Other);
}
