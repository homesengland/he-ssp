using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.AhpStatusChange;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.AhpStatusChange
{
    public class SendNotificationOnCreateHandler : CrmEntityHandlerBase<invln_AHPStatusChange, DataverseContext>
    {
        #region Fields

        private invln_AHPStatusChange target => ExecutionData.Target;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IAhpStatusChangeService>().SendNotificationOnAhpStatusChangeCreate(target);
        }

        #endregion
    }
}

