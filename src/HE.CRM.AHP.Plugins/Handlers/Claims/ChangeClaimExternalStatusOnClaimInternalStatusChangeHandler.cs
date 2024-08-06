using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Claim;

namespace HE.CRM.AHP.Plugins.Handlers.Claims
{
    public class ChangeClaimExternalStatusOnClaimInternalStatusChangeHandler : CrmEntityHandlerBase<invln_Claim, DataverseContext>
    {
        private invln_Claim target => ExecutionData.Target;

        public override bool CanWork()
        {
            return ValueChanged(invln_Claim.Fields.StatusCode);
        }

        public override void DoWork()
        {
            TracingService.Trace("ChangeClaimExternalStatusOnClaimInternalStatusChangeHandler");
            CrmServicesFactory.Get<IClaimService>().ChangeExternalStatus(target);
        }


    }
}
