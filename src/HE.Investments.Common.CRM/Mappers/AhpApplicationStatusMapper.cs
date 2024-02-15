using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.Common.CRM.Mappers;

public static class AhpApplicationStatusMapper
{
    public static int MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.New => (int)invln_ExternalStatusAHP.New,
            ApplicationStatus.Draft => (int)invln_ExternalStatusAHP.Draft,
            ApplicationStatus.ApplicationSubmitted => (int)invln_ExternalStatusAHP.ApplicationSubmitted,
            ApplicationStatus.Deleted => (int)invln_ExternalStatusAHP.Deleted,
            ApplicationStatus.OnHold => (int)invln_ExternalStatusAHP.OnHold,
            ApplicationStatus.Withdrawn => (int)invln_ExternalStatusAHP.Withdrawn,
            ApplicationStatus.UnderReview => (int)invln_ExternalStatusAHP.UnderReview,
            ApplicationStatus.Rejected => (int)invln_ExternalStatusAHP.Rejected,
            ApplicationStatus.RequestedEditing => (int)invln_ExternalStatusAHP.RequestedEditing,
            ApplicationStatus.ReferredBackToApplicant => (int)invln_ExternalStatusAHP.ReferredBackToApplicant,
            ApplicationStatus.ApprovedSubjectToContract => (int)invln_ExternalStatusAHP.ApprovedSubjectToContract,
            ApplicationStatus.Approved => (int)invln_ExternalStatusAHP.Approved,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };
    }

    public static ApplicationStatus MapToPortalStatus(int? crmStatus)
    {
        return crmStatus switch
        {
            (int)invln_ExternalStatusAHP.New => ApplicationStatus.New,
            (int)invln_ExternalStatusAHP.Draft => ApplicationStatus.Draft,
            (int)invln_ExternalStatusAHP.ApplicationSubmitted => ApplicationStatus.ApplicationSubmitted,
            (int)invln_ExternalStatusAHP.Deleted => ApplicationStatus.Deleted,
            (int)invln_ExternalStatusAHP.OnHold => ApplicationStatus.OnHold,
            (int)invln_ExternalStatusAHP.Withdrawn => ApplicationStatus.Withdrawn,
            (int)invln_ExternalStatusAHP.UnderReview => ApplicationStatus.UnderReview,
            (int)invln_ExternalStatusAHP.Rejected => ApplicationStatus.Rejected,
            (int)invln_ExternalStatusAHP.RequestedEditing => ApplicationStatus.RequestedEditing,
            (int)invln_ExternalStatusAHP.ReferredBackToApplicant => ApplicationStatus.ReferredBackToApplicant,
            (int)invln_ExternalStatusAHP.ApprovedSubjectToContract => ApplicationStatus.ApprovedSubjectToContract,
            (int)invln_ExternalStatusAHP.Approved => ApplicationStatus.Approved,
            null => ApplicationStatus.Draft,
            _ => throw new ArgumentOutOfRangeException(nameof(crmStatus), crmStatus, null),
        };
    }
}
