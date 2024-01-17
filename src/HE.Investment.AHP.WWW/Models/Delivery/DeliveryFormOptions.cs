using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public static class DeliveryFormOptions
{
    public static IEnumerable<ExtendedSelectListItem> RemoveDeliveryPhase => SelectListHelper.FromEnumToExtendedList<RemoveDeliveryPhaseAnswer>();

    public static IEnumerable<SelectListItem> TypeOfHomes => SelectListHelper.FromEnum<TypeOfHomes>();
}
