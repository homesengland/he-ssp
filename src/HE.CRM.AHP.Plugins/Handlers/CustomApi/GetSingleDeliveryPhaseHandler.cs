using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetSingleDeliveryPhaseHandler : CrmActionHandlerBase<invln_getsingledeliveryphaseRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_getsingledeliveryphaseRequest.Fields.invln_applicationId);
        private string organizationId => ExecutionData.GetInputParameter<string>(invln_getsingledeliveryphaseRequest.Fields.invln_organisationId);
        private string externalUserId => ExecutionData.GetInputParameter<string>(invln_getsingledeliveryphaseRequest.Fields.invln_userid);
        private string deliveryPhaseId => ExecutionData.GetInputParameter<string>(invln_getsingledeliveryphaseRequest.Fields.invln_deliveryPhaseId);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsingledeliveryphaseRequest.Fields.invln_fieldstoretrieve);

        #endregion Fields

        #region Base Methods Overrides

        public override bool CanWork()
        {
            TracingService.Trace(deliveryPhaseId.ToString());
            return deliveryPhaseId != null && deliveryPhaseId != Guid.Empty.ToString();
        }

        public override void DoWork()
        {
            TracingService.Trace("Start Do Work");
            var deliveryPhase = CrmServicesFactory.Get<IDeliveryPhaseService>().GetDeliveryPhase(applicationId, organizationId, externalUserId, deliveryPhaseId, fieldsToRetrieve);
            if (deliveryPhase != null)
            {
                var deliveryPhaseSerialized = JsonSerializer.Serialize(deliveryPhase);
                ExecutionData.SetOutputParameter(invln_getsingledeliveryphaseResponse.Fields.invln_deliveryPhase, deliveryPhaseSerialized);
            }
        }

        #endregion Base Methods Overrides
    }
}
