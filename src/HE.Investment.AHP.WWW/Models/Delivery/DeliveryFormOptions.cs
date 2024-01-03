using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public static class DeliveryFormOptions
{
    public static IEnumerable<SelectListItem> RemoveDeliveryPhase => SelectListHelper.FromEnum<RemoveDeliveryPhaseAnswer>();
}
