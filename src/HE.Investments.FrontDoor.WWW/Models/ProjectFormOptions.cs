using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.WWW.Models;

public static class ProjectFormOptions
{
    public static IEnumerable<ExtendedSelectListItem> ActivityTypes => SelectListHelper.FromEnumToExtendedList<ActivityType>()
        .Where(x => x.Value != ActivityType.Other.ToString());

    public static ExtendedSelectListItem ActivityTypeOther => SelectListHelper.FromEnumToExtendedListItem(ActivityType.Other);
}
