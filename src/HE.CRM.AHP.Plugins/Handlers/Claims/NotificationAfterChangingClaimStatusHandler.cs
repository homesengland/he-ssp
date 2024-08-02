using DataverseModel;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;

namespace HE.CRM.AHP.Plugins.Handlers.Claims
{
    [CrmMessage(CrmMessage.Update)]
    [CrmProcessingStage(CrmProcessingStepStages.Postoperation)]
    public sealed class NotificationAfterChangingClaimStatusHandler : CrmEntityHandlerBase<invln_Claim, DataverseContext>
    {
        public override bool CanWork()
        {
            if (ExecutionData.PostImage.invln_Allocation == null)
            {
                return false;
            }

            var currentStatuCode = (invln_Claim_StatusCode)ExecutionData.Target.StatusCode.Value;
            return this.ValueChanged(invln_Claim.Fields.StatusCode) && currentStatuCode == invln_Claim_StatusCode.Submitted;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_AHP_EXTERNAL_CLAIM_SUBMITTED(CurrentState.invln_Allocation.Id);
        }
    }
}

