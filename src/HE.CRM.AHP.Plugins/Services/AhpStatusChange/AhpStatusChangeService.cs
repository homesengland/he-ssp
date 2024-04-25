using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.AHP.Plugins.Services.GovNotifyEmail;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Services.AhpStatusChange
{
    public class AhpStatusChangeService : CrmService, IAhpStatusChangeService
    {
        #region Fields
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IGovNotifyEmailService _govNotifyEmailService;

        private readonly ILoanApplicationRepository _loanApplicationRepositoryAdmin;
        private readonly IGovNotifyEmailRepository _govNotifyEmailRepositoryAdmin;
        private readonly IEnvironmentVariableRepository _environmentVariableRepositoryAdmin;
        private readonly INotificationSettingRepository _notificationSettingRepositoryAdmin;
        private readonly ISystemUserRepository _systemUserRepositoryAdmin;
        #endregion

        #region Constructors

        public AhpStatusChangeService(CrmServiceArgs args) : base(args)
        {
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();

            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
        }
        #endregion

        #region Public Methods

        public void SendNotificationOnAhpStatusChangeCreate(invln_AHPStatusChange target)
        {
            if (target.invln_ChangeSource != null)
            {
                this.TracingService.Trace("Change source: " +  target.invln_ChangeSource.Value);
                var ahpApplication = _ahpApplicationRepository.GetById(target.invln_AHPApplication.Id, new string[] { nameof(invln_scheme.OwnerId).ToLower(), nameof(invln_scheme.invln_schemename).ToLower(), nameof(invln_scheme.invln_contactid).ToLower(), nameof(invln_scheme.invln_organisationid).ToLower() });
                SendNotification(target, ahpApplication);
            }
        }
        #endregion

        #region Private Methods

        private void SendNotification(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            var statusLabel = string.Empty;
            switch (ahpStatusChange.invln_Changeto.Value)
            {
                case (int)invln_AHPInternalStatus.Withdrawn:
                    statusLabel = "Withdrawn";
                    _govNotifyEmailService.SendNotifications_AHP_INTERNAL_REQUEST_TO_WITHDRAW(ahpStatusChange, ahpApplication);
                    break;

                case (int)invln_AHPInternalStatus.ApplicationSubmitted:
                    statusLabel = "ApplicationSubmitted";
                    if (ahpStatusChange.invln_Changefrom.Value == (int)invln_AHPInternalStatus.Draft)
                    {
                        _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_APPLICATION_SUBMITTED(ahpStatusChange, ahpApplication);
                    }
                    else
                    {
                        TracingService.Trace("Changefrom is not Draft");
                    }
                    break;

                case (int)invln_AHPInternalStatus.ReferredBackToApplicant:
                    statusLabel = "ReferredBackToApplicant";
                    _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT(ahpStatusChange, ahpApplication);
                    break;

                case (int)invln_AHPInternalStatus.ApprovedSubjecttoContract:
                    statusLabel = "ApprovedSubjecttoContract";
                    _govNotifyEmailService.SendNotifications_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT(ahpStatusChange, ahpApplication);
                    break;

                default:
                    break;
            }

            var pastFormStatus = string.Empty;
            switch (ahpStatusChange.invln_Changeto.Value)
            {
                case (int)invln_AHPInternalStatus.Draft:
                    statusLabel = "Draft";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ApplicationSubmitted:
                    statusLabel = "ApplicationSubmitted";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.Approved:
                    statusLabel = "Approved";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ApprovedContractExecuted:
                    statusLabel = "ApprovedContractExecuted";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ApprovedContractPassedComplianceChecks:
                    statusLabel = "ApprovedContractPassedComplianceChecks";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ApprovedContractReceivedBackToHE:
                    statusLabel = "ApprovedContractReceivedBackToHE";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ApprovedEngressmentIssued:
                    statusLabel = "ApprovedEngressmentIssued";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ApprovedSubjecttoContract:
                    statusLabel = "ApprovedSubjecttoContract";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.Deleted:
                    statusLabel = "Deleted";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.InternallyApprovedSubjectToIPQ:
                    statusLabel = "InternallyApprovedSubjectToIPQ";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.InternallyApprovedSubjectToIPQAndRegulatorySignOff:
                    statusLabel = "InternallyApprovedSubjectToIPQAndRegulatorySignOff";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.InternallyApprovedSubjectToRegulatorSignOff:
                    statusLabel = "InternallyApprovedSubjectToRegulatorSignOff";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.InternallyRejected:
                    statusLabel = "InternallyRejected";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.OnHold:
                    statusLabel = "OnHold";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.ReferredBackToApplicant:
                    statusLabel = "ReferredBackToApplicant";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.Rejected:
                    statusLabel = "Rejected";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.RequestedEditing:
                    statusLabel = "RequestedEditing";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewGoingToBidClinic:
                    statusLabel = "UnderReviewGoingToBidClinic";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewGoingToCMEModeration:
                    statusLabel = "UnderReviewGoingToCMEModeration";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewGoingToSLT:
                    statusLabel = "UnderReviewGoingToSLT";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewInAssessment:
                    statusLabel = "UnderReviewInAssessment";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewInternallyApproved:
                    statusLabel = "UnderReviewInternallyApproved";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewPendingAssessment:
                    statusLabel = "UnderReviewPendingAssessment";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_AHPInternalStatus.Withdrawn:
                    statusLabel = "Withdrawn";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
               
                //    statusLabel = "Draft";
                //    pastFormStatus = "is now being edited by the applicant";
                //    if (loanStatusChange.invln_changefrom == null)
                //    {
                //        _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(loanStatusChange, loanApplication, "saved your development loan application as draft");
                //    }
                //    break;
                //case (int)invln_InternalStatus.ApplicationSubmitted:
                //    statusLabel = "Application submitted";
                //    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                //    _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(loanStatusChange, loanApplication, "submitted your application");
                //    break;

                default:
                    break;
            }

            this.TracingService.Trace("Status Label: " + statusLabel);
            SendInternalCrmNotification(ahpStatusChange, ahpApplication, statusLabel);
            //_govNotifyEmailService.SendNotifications_INTERNAL_LOAN_APP_STATUS_CHANGE(ahpStatusChange, ahpApplication, statusLabel, pastFormStatus);

        }

        public void SendInternalCrmNotification(invln_AHPStatusChange statusChange, invln_scheme ahpApplication, string statusLabel)
        {
            this.TracingService.Trace("Sending Internal Crm Notification");
            var internalNotification = new invln_sendinternalcrmnotificationRequest()
            {
                invln_notificationbody = $"[Application ref no {ahpApplication.invln_schemename} - Status change to '{statusLabel}](?pagetype=entityrecord&etn=.invln_scheme&id={ahpApplication.Id})'",
                invln_notificationowner = ahpApplication.OwnerId.Id.ToString(),
                invln_notificationtitle = "Information",
            };

            this.TracingService.Trace("Executing Notification Request");
            _loanApplicationRepositoryAdmin.ExecuteNotificatioRequest(internalNotification);
        }

        #endregion
    }
}
