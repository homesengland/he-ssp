using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.GovNotifyEmail;
using Microsoft.Crm.Sdk.Messages;


namespace HE.CRM.Plugins.Handlers.ReviewsApprovals
{
    public class NotificationAfterChangingReviewsApprovalStatusHandler : CrmEntityHandlerBase<invln_reviewapproval, DataverseContext>
    {

        public readonly IIspRepository _ispRepository;
        public readonly ILoanApplicationRepository _loanApplicationRepository;

        public NotificationAfterChangingReviewsApprovalStatusHandler(IIspRepository ispRepository, ILoanApplicationRepository loanRepository)
        {
            _ispRepository = ispRepository;
            _loanApplicationRepository = loanRepository;
        }

        public override bool CanWork()
        {
            if (ExecutionData.PostImage.invln_status == null)
            {
                return false;
            }

            return ValueChanged(invln_reviewapproval.Fields.invln_status, (int)invln_StatusReviewApprovalSet.Reviewed) || ValueChanged(invln_reviewapproval.Fields.invln_status, (int)invln_StatusReviewApprovalSet.Approved);
        }

        public override void DoWork()
        {
            TracingService.Trace("NotificationAfterChangingReviewsApprovalStatusHandler");

            var ispCrmGuid = ExecutionData.PostImage.invln_ispid.Id;
            var ispCRM = _ispRepository.GetById(ispCrmGuid, invln_ISP.Fields.invln_Loanapplication, invln_ISP.Fields.invln_ISPId);
            var loanAppCRM = _loanApplicationRepository.GetById(ispCRM.invln_Loanapplication.Id, invln_Loanapplication.Fields.invln_LoanapplicationId, invln_Loanapplication.Fields.OwnerId, invln_Loanapplication.Fields.invln_Name, invln_Loanapplication.Fields.StatusCode);

            TracingService.Trace($"ispCrmGuid : {ispCrmGuid}");
            TracingService.Trace($"ispCRM : {ispCRM.invln_ISPId}");
            TracingService.Trace($"loanAppCRM : {loanAppCRM.invln_LoanapplicationId}");

            if (loanAppCRM.StatusCode.Value == (int)invln_Loanapplication_StatusCode.SentforApproval)
            {
                TracingService.Trace("SendNotifications_INTERNAL_SENT_FOR_APPROVAL_NOTIFICATION");
                CrmServicesFactory.Get<IGovNotifyEmailService>().SendNotifications_INTERNAL_SENT_FOR_APPROVAL_NOTIFICATION(loanAppCRM);
                TracingService.Trace("SendInternalCrmNotificationOnSentForApproval");
                SendInternalCrmNotificationOnSentForApproval(loanAppCRM);
            }
        }

        public void SendInternalCrmNotificationOnSentForApproval(invln_Loanapplication loanApplication)
        {
            var internalNotification = new invln_sendinternalcrmnotificationRequest()
            {
                invln_notificationbody = $" Application ref no {loanApplication.invln_Name} Reviewed",
                invln_notificationowner = loanApplication.OwnerId.Id.ToString(),
                invln_notificationtitle = "Information",
            };
            _ = _loanApplicationRepository.ExecuteNotificatioRequest(internalNotification);
        }
    }
}
