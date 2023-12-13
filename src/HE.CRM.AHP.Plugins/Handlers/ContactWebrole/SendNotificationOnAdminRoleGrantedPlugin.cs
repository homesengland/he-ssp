using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Services;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;

namespace HE.CRM.AHP.Plugins.Handlers.ContactWebrole
{
    public class SendNotificationOnAdminRoleGrantedHandler : CrmEntityHandlerBase<invln_contactwebrole, DataverseContext>
    {
        #region Fields

        private invln_contactwebrole target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS(target.ToEntityReference());
        }

        #endregion
    }
}
