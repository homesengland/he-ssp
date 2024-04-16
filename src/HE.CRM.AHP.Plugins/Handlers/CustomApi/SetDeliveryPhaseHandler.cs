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
    public class SetDeliveryPhaseHandler : CrmActionHandlerBase<invln_setdeliveryphaseRequest, DataverseContext>
    {
        #region Fields

        private string deliveryPhaseData => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_deliveryPhase);
        private string userId => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_userId);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_organisationId);
        private string applicationId => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_applicationId);
        private string fieldsToSet => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_fieldstoset);

        #endregion Fields

        #region Base Methods Overrides

        public override bool CanWork()
        {
            return deliveryPhaseData != null && userId != null && organisationId != null && applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var createdRecordGuid = CrmServicesFactory.Get<IDeliveryPhaseService>().SetDeliveryPhase(deliveryPhaseData, userId, organisationId, applicationId, fieldsToSet);
            ExecutionData.SetOutputParameter(invln_setdeliveryphaseResponse.Fields.invln_deliveryphaseid, createdRecordGuid.ToString());
        }

        #endregion Base Methods Overrides
    }
}
