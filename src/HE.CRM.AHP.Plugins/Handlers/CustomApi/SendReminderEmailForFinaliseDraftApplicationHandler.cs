using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SendReminderEmailForFinaliseDraftApplicationHandler : CrmActionHandlerBase<invln_sendremindertofinalisadraftapplicationRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_sendremindertofinalisadraftapplicationRequest.Fields.invln_applicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("SendReminderEmailForFinaliseDraftApplication");
            if (Guid.TryParse(applicationId, out var applicationGuid))
            {
                CrmServicesFactory.Get<IApplicationService>().SendReminderEmailForFinaliseDraftApplication(applicationGuid);
            }
        }

        #endregion
    }
}
