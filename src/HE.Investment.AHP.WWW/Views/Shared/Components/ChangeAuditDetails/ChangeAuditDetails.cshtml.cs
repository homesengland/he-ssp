using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ChangeAuditDetails;

public class ChangeAuditDetails : ViewComponent
{
    public IViewComponentResult Invoke(
        ChangeAuditType changeAuditType,
        HE.Investments.Common.Contract.ModificationDetails? modificationDetails)
    {
        return View("ChangeAuditDetails", (changeAuditType, modificationDetails));
    }
}
