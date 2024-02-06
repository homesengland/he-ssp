using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IHomesInDeliveryPhaseRepository : ICrmEntityRepository<invln_homesindeliveryphase, DataverseContext>
    {
        List<invln_homesindeliveryphase> GetHomesInDeliveryPhase(Guid deliveryPhaseId);
    }
}
