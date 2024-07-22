using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IClaimRepository : ICrmEntityRepository<invln_Claim, DataverseContext>
    {
        invln_Claim GetClaimForAllocationDeliveryPhase(Guid deliveryPhaseId, int milestone);

        IEnumerable<invln_Claim> GetClaimsForAllocation(Guid allocationId, bool onlyApproved, params string[] claimColumns);
    }
}


