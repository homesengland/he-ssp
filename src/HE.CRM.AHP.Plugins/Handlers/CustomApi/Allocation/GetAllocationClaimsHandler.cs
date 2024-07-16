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
    public class GetAllocationClaimsHandler : CrmActionHandlerBase<invln_getallocationclaimsRequest, DataverseContext>
    {
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getallocationclaimsRequest.Fields.invln_userid);
        private Guid accountId => ExecutionData.GetInputParameter<Guid>(invln_getallocationclaimsRequest.Fields.invln_accountid);
        private Guid allocationId => ExecutionData.GetInputParameter<Guid>(invln_getallocationclaimsRequest.Fields.invln_allocationid);

        public override bool CanWork()
        {
            TracingService.Trace($"* invln_userid : {externalContactId}");
            TracingService.Trace($"* accountId : {accountId}");
            TracingService.Trace($"* allocationId : {allocationId}");
            return externalContactId != null && accountId != null && allocationId != null;
        }

        public override void DoWork()
        {
            AllocationClaimsDto allocationClaims = CrmServicesFactory.Get<IAllocationService>().GetAllocationWithClaims(externalContactId, accountId, allocationId);

            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_getallocationclaimsResponse.Fields.invln_ahpallocationclaims, JsonSerializer.Serialize(allocationClaims));
        }

    }
}
