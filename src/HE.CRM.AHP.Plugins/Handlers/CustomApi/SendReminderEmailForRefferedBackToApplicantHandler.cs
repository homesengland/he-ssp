using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SendReminderEmailForRefferedBackToApplicantHandler : CrmActionHandlerBase<invln_sendreminderemailforrefferedbacktoapplicantRequest, DataverseContext>
    {
        #region Fields

        private string applicationId => ExecutionData.GetInputParameter<string>(invln_sendreminderemailforrefferedbacktoapplicantRequest.Fields.invln_applicationid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return applicationId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("SendReminderEmailForRefferedBackToApplicantHandler");
            if (Guid.TryParse(applicationId, out var applicationGuid))
            {
                CrmServicesFactory.Get<IGovNotifyEmailService>().SendReminderEmailForRefferedBackToApplicant(applicationGuid);
            }
        }

        #endregion
    }
}
