using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.WWW.Models;

public static class ProjectFormOptions
{
    public static IEnumerable<ExtendedSelectListItem> ActivityTypes => SelectListHelper.FromEnumToExtendedList<SupportActivityType>()
        .Where(x => x.Value != SupportActivityType.Other.ToString());

    public static ExtendedSelectListItem ActivityTypeOther => SelectListHelper.FromEnumToExtendedListItem(SupportActivityType.Other);
}
