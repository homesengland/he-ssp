using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.AhpStatusChange;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.ContactWebrole
{
    public class SendNotificationOnRoleModificationHandler : CrmEntityHandlerBase<invln_contactwebrole, DataverseContext>
    {
        #region Fields



        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            if (ExecutionData.Context.MessageName.ToLower() == "delete")
            {
                this.TracingService.Trace("Delete");
                CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS((EntityReference)ExecutionData.Context.InputParameters["Target"]);
            }
            else
            {
                this.TracingService.Trace("Update or Create");
                var target = ExecutionData.Target;
                CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS(target.ToEntityReference());
            }
        }

        #endregion
    }
}
