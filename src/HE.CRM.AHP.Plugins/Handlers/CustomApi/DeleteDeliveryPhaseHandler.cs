using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;
using HE.CRM.AHP.Plugins.Services.HomeType;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class DeleteDeliveryPhaseHandler : CrmActionHandlerBase<invln_deletedeliveryphaseRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_deletedeliveryphaseRequest.Fields.invln_applicationId);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_deletedeliveryphaseRequest.Fields.invln_organisationId);
        private string deliveryPhaseId => ExecutionData.GetInputParameter<string>(invln_deletedeliveryphaseRequest.Fields.invln_deliveryPhaseId);
        private string externalUserId => ExecutionData.GetInputParameter<string>(invln_deletedeliveryphaseRequest.Fields.invln_userId);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return deliveryPhaseId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            CrmServicesFactory.Get<IDeliveryPhaseService>().DeleteDeliveryPhase(applicationId, organisationId, deliveryPhaseId, externalUserId);
        }
    }

    #endregion
}
