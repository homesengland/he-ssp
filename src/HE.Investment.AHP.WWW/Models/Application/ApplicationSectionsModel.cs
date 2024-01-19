using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Helpers;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationSectionsModel(
    string ApplicationId,
    string SiteName,
    string Name,
    ApplicationStatus Status,
    string? ReferenceNumber,
    ModificationDetails? LastModificationDetails,
    IList<ApplicationSection> Sections)
{
    public bool CanBePutOnHold()
    {
        var statusesAllowedForPutOnHold = ApplicationStatusDivision.GetAllStatusesAllowedForPutOnHold();
        return statusesAllowedForPutOnHold.Contains(Status);
    }

    public bool CanBeWithdrawn()
    {
        var statusesAllowedForWithdraw = ApplicationStatusDivision.GetAllStatusesAllowedForWithdraw();
        return statusesAllowedForWithdraw.Contains(Status);
    }
}
