using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Services;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;

namespace HE.CRM.AHP.Plugins.Handlers.DeliveryPhase
{
    public class PostUpdateDeliveryPhaseHandler : CrmEntityHandlerBase<invln_DeliveryPhase, DataverseContext>
    {
        private invln_DeliveryPhase target => ExecutionData.Target;
        private invln_DeliveryPhase preImage => ExecutionData.PreImage;
        private invln_DeliveryPhase postImage => ExecutionData.PostImage;


        public override bool CanWork()
        {
            return postImage.StatusCode != preImage.StatusCode && postImage.StatusCode.Value == (int)invln_DeliveryPhase_StatusCode.RejectedAdjustment;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_AHP_EXTERNAL_REJECT_ON_DELIVERY_PHASE(target.ToEntityReference());
        }
    }
}
