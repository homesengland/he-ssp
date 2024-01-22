using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class GetMultipleDeliveryPhaseHandler : CrmActionHandlerBase<invln_getmultipledeliveryphaseRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_getmultipledeliveryphaseRequest.Fields.invln_applicationId);
        private string externaluserId => ExecutionData.GetInputParameter<string>(invln_getmultipledeliveryphaseRequest.Fields.invln_userId);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_getmultipledeliveryphaseRequest.Fields.invln_organisationId);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultipledeliveryphaseRequest.Fields.invln_fieldstoretrieve);
        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return organisationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            var applications = CrmServicesFactory.Get<IDeliveryPhaseService>().GetDeliveryPhases(applicationId, organisationId, externaluserId, fieldsToRetrieve);
            if (applications != null)
            {
                var serializedApplications = JsonSerializer.Serialize(applications);
                ExecutionData.SetOutputParameter(invln_getmultipledeliveryphaseResponse.Fields.invln_deliveryPhaseList, serializedApplications);
            }
        }

        #endregion
    }
}
