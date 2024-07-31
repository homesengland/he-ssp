using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.GovNotifyEmail;
using HE.CRM.Plugins.Services.ISPs;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Handlers.ISPs
{
    public class SendEmailEndAddNotificationOnOwnerChangeHandler : CrmEntityHandlerBase<invln_ISP, DataverseContext>
    {
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly IGovNotifyEmailService _govNotifyEmailService;

        public SendEmailEndAddNotificationOnOwnerChangeHandler(ILoanApplicationRepository loanApplicationRepository, IGovNotifyEmailService govNotifyEmailService)
        {
            _loanApplicationRepository = loanApplicationRepository;
            _govNotifyEmailService = govNotifyEmailService;
        }

        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            var application = _loanApplicationRepository.GetById(ExecutionData.PostImage.invln_Loanapplication.Id);
            SendInternalCrmNotification(application, ExecutionData.PostImage.OwnerId);

            _govNotifyEmailService.SendNotifications_AHP_INTERNAL_REQUEST_TO_WITHDRAW(ahpStatusChange, ahpApplication);
        }

        public void SendInternalCrmNotification(invln_Loanapplication application, EntityReference owner)
        {
            this.TracingService.Trace("Sending Internal Crm Notification");

            var internalNotification = new invln_sendinternalcrmnotificationRequest()
            {
                invln_notificationbody = $"Application ref no {application.invln_Name} has been assigned to you for review/approval",
                invln_notificationowner = owner.Id.ToString(),
                invln_notificationtitle = "Information",
            };
            var _ispRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IIspRepository>();
            this.TracingService.Trace("Executing Notification Request");
            _ispRepositoryAdmin.ExecuteNotificatioRequest(internalNotification);
        }
    }
}
