using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Allocation
{
    public class SetAllocationPhaseHandler : CrmActionHandlerBase<invln_setallocationphaseRequest, DataverseContext>
    {
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_setallocationphaseRequest.Fields.invln_userid);
        private Guid accountId => ExecutionData.GetInputParameter<Guid>(invln_setallocationphaseRequest.Fields.invln_accountid);
        private Guid allocationId => ExecutionData.GetInputParameter<Guid>(invln_setallocationphaseRequest.Fields.invln_allocationid);
        private Guid deliveryPhaseId => ExecutionData.GetInputParameter<Guid>(invln_setallocationphaseRequest.Fields.invln_deliveryphaseid);
        private string phaseClaimsDto => ExecutionData.GetInputParameter<string>(invln_setallocationphaseRequest.Fields.invln_phaseclaimsdto);




        public override bool CanWork()
        {
            TracingService.Trace($"* invln_userid : {externalContactId}");
            TracingService.Trace($"* accountId : {accountId}");
            TracingService.Trace($"* allocationId : {allocationId}");
            TracingService.Trace($"* deliveryPhaseId : {deliveryPhaseId}");
            TracingService.Trace($"* phaseClaimsDto : {phaseClaimsDto}");
            return externalContactId != null && accountId != null && allocationId != null && deliveryPhaseId != null && phaseClaimsDto != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IAllocationService>().SetAllocationPhase(externalContactId, accountId, allocationId, deliveryPhaseId, phaseClaimsDto);

            this.TracingService.Trace("Send Response");
            ExecutionData.SetOutputParameter(invln_setallocationphaseResponse.Fields.id, deliveryPhaseId);
        }

    }
}




