using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Common.Mappers;

public static class AhpApplicationStatusMapper
{

    public static int MapToCrmStatus(ApplicationStatus status)
    {
        return status switch
        {
            ApplicationStatus.New => (int)invln_externalstatusahp.New,
            ApplicationStatus.Draft => (int)invln_externalstatusahp.Draft,
            ApplicationStatus.ApplicationSubmitted => (int)invln_externalstatusahp.ApplicationSubmitted,
            ApplicationStatus.Deleted => (int)invln_externalstatusahp.Deleted,
            ApplicationStatus.OnHold => (int)invln_externalstatusahp.OnHold,
            ApplicationStatus.Withdrawn => (int)invln_externalstatusahp.Withdrawn,
            ApplicationStatus.UnderReview => (int)invln_externalstatusahp.UnderReview,
            ApplicationStatus.Rejected => (int)invln_externalstatusahp.Rejected,
            ApplicationStatus.RequestedEditing => (int)invln_externalstatusahp.RequestedEditing,
            ApplicationStatus.ReferredBackToApplicant => (int)invln_externalstatusahp.ReferredBackToApplicant,
            ApplicationStatus.ApprovedSubjectToContract => (int)invln_externalstatusahp.ApprovedSubjectToContract,
            ApplicationStatus.Approved => (int)invln_externalstatusahp.Approved,
            _ => throw new NotImplementedException(),
        };
    }

    public static ApplicationStatus MapToPortalStatus(int? crmStatus)
    {
        return crmStatus switch
        {
            (int)invln_externalstatusahp.New => ApplicationStatus.New,
            (int)invln_externalstatusahp.Draft => ApplicationStatus.Draft,
            (int)invln_externalstatusahp.ApplicationSubmitted => ApplicationStatus.ApplicationSubmitted,
            (int)invln_externalstatusahp.Deleted => ApplicationStatus.Deleted,
            (int)invln_externalstatusahp.OnHold => ApplicationStatus.OnHold,
            (int)invln_externalstatusahp.Withdrawn => ApplicationStatus.Withdrawn,
            (int)invln_externalstatusahp.UnderReview => ApplicationStatus.UnderReview,
            (int)invln_externalstatusahp.Rejected => ApplicationStatus.Rejected,
            (int)invln_externalstatusahp.RequestedEditing => ApplicationStatus.RequestedEditing,
            (int)invln_externalstatusahp.ReferredBackToApplicant => ApplicationStatus.ReferredBackToApplicant,
            (int)invln_externalstatusahp.ApprovedSubjectToContract => ApplicationStatus.ApprovedSubjectToContract,
            (int)invln_externalstatusahp.Approved => ApplicationStatus.Approved,
            null => ApplicationStatus.Draft,
            _ => throw new ArgumentOutOfRangeException(nameof(crmStatus), crmStatus, null),
        };
    }
}

