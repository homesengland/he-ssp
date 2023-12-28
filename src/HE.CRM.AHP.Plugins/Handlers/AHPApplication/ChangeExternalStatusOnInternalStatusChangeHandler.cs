using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class ChangeExternalStatusOnInternalStatusChangeHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        #region Fields

        private invln_scheme target => ExecutionData.Target;
        private invln_scheme preImage => ExecutionData.PreImage;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null && preImage != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IApplicationService>().ChangeExternalStatus(target, preImage);
        }

        #endregion
    }
}
