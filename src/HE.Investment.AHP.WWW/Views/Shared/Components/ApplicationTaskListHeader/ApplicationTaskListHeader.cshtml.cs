using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ApplicationTaskListHeader;

public class ApplicationTaskListHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        int completeSectionsCount,
        int incompleteSectionsCount,
        int totalSectionsCount,
        ApplicationStatus applicationStatus,
        string? referenceNumber,
        ModificationDetails? lastModificationDetails,
        ModificationDetails? lastSubmissionDetails)
    {
        return View("ApplicationTaskListHeader", (completeSectionsCount, incompleteSectionsCount, totalSectionsCount, applicationStatus, referenceNumber, lastModificationDetails, lastSubmissionDetails));
    }
}
