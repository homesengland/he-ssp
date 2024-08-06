using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.AhpProject;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Allocation
{
    public class GetAllocationHandler : CrmActionHandlerBase<invln_getallocationRequest, DataverseContext>
    {
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getallocationRequest.Fields.invln_userid);
        private Guid accountId => ExecutionData.GetInputParameter<Guid>(invln_getallocationRequest.Fields.invln_accountid);
        private Guid allocationId => ExecutionData.GetInputParameter<Guid>(invln_getallocationRequest.Fields.invln_allocationid);

        public override bool CanWork()
        {
            TracingService.Trace($"* invln_userid : {externalContactId}");
            TracingService.Trace($"* accountId : {accountId}");
            TracingService.Trace($"* allocationId : {allocationId}");
            return externalContactId != null && accountId != null && allocationId != null;
        }

        public override void DoWork()
        {
            AllocationDto allocationClaims = CrmServicesFactory.Get<IAllocationService>().GetAllocation(externalContactId, accountId, allocationId);

            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_getallocationResponse.Fields.invln_ahpallocation, JsonSerializer.Serialize(allocationClaims));
        }
    }
}

