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
    public class SendNotificationUrbWantsAdditionalPaymentsForPhaseHandler : CrmEntityHandlerBase<invln_DeliveryPhase, DataverseContext>
    {
        #region Fields

        private invln_DeliveryPhase target => ExecutionData.Target;
        private invln_DeliveryPhase preImage => ExecutionData.PreImage;
        private invln_DeliveryPhase postImage => ExecutionData.PostImage;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return postImage.invln_urbrequestingearlymilestonepayments != preImage.invln_urbrequestingearlymilestonepayments && postImage.invln_urbrequestingearlymilestonepayments == true;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_AHP_INTERNAL_EXTERNAL_WANTS_ADDITIONAL_PAYMENTS_FOR_PHASE(target.ToEntityReference());
        }

        #endregion
    }
}
