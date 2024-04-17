using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class ChangeAhpApplicationHandler : CrmActionHandlerBase<invln_changeahpapplicationstatusRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_changeahpapplicationstatusRequest.Fields.invln_applicationid);
        private string contactId => ExecutionData.GetInputParameter<string>(invln_changeahpapplicationstatusRequest.Fields.invln_userid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_changeahpapplicationstatusRequest.Fields.invln_organisationid);
        private int newStatus => ExecutionData.GetInputParameter<int>(invln_changeahpapplicationstatusRequest.Fields.invln_newapplicationstatus);
        private string changeReason => ExecutionData.GetInputParameter<string>(invln_changeahpapplicationstatusRequest.Fields.invln_changereason);
        private bool representationsandwarranties => ExecutionData.GetInputParameter<bool>(invln_changeahpapplicationstatusRequest.Fields.invln_representationsandwarranties);

        #endregion Fields

        #region Base Methods Overrides

        public override bool CanWork()
        {
            return applicationId != null && contactId != null && organisationId != null && newStatus != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("method");
            CrmServicesFactory.Get<IApplicationService>().ChangeApplicationStatus(organisationId, contactId, applicationId, newStatus, changeReason, representationsandwarranties);
        }

        #endregion Base Methods Overrides
    }
}
