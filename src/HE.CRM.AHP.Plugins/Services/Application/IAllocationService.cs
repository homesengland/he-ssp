using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public interface IAllocationService : ICrmService
    {
        AllocationClaimsDto GetAllocationWithClaims(string externalContactId, Guid accountId, Guid allocationId);

        void SetAllocationPhase(string externalContactId, Guid accountId, Guid allocationId, Guid deliveryPhaseId, string phaseClaimsDtoData);

        void CalculateGrantDetails(Guid allocationId);

        AllocationDto GetAllocation(string externalContactId, Guid accountId, Guid allocationId);

        Guid CreateAllocation(Guid schemeId, bool isVariation = false);
    }
}
