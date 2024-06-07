using System;
using DataverseModel;

namespace HE.CRM.AHP.Plugins.Tests.Helpers
{
    public static class HelperDeliveryPhase
    {
        public static invln_DeliveryPhase CreateDeliveryPhase(invln_scheme application, DateTime? startOnSiteMilestoneClaimDate,  DateTime? completionMilestoneClaimDate)
        {
            return new invln_DeliveryPhase()
            {
                Id = Guid.NewGuid(),
                invln_Application = application?.ToEntityReference(),
                invln_startonsitemilestoneclaimdate = startOnSiteMilestoneClaimDate,
                invln_completionmilestoneclaimdate = completionMilestoneClaimDate
            };
        }
    }
}
