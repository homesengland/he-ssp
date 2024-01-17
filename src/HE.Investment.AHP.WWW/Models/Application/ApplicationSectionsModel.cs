using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Helpers;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Models.Application;

public class ApplicationSectionsModel
{
    public ApplicationSectionsModel(
        string applicationId,
        string siteName,
        string name,
        ApplicationStatus status,
        string? referenceNumber,
        ModificationDetails? lastModificationDetails,
        IList<ApplicationSection> sections)
    {
        ApplicationId = applicationId;
        SiteName = siteName;
        Name = name;
        Status = status;
        ReferenceNumber = referenceNumber;
        LastModificationDetails = lastModificationDetails;
        Sections = sections;
    }

    public string ApplicationId { get; }

    public string SiteName { get; }

    public string Name { get; }

    public ApplicationStatus Status { get; }

    public string? ReferenceNumber { get; }

    public ModificationDetails? LastModificationDetails { get; }

    public IList<ApplicationSection> Sections { get; }

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
