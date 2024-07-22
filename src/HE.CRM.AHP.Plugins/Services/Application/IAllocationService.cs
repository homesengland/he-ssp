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

        void CalculateGrantDetails(Guid allocationId, Guid organisationId);
    }
}
