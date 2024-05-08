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
                this.TracingService.Trace("Change source: " + target.invln_ChangeSource.Value);
                var ahpApplication = _ahpApplicationRepository.GetById(
                    target.invln_AHPApplication.Id,
                    new string[] {
                        invln_scheme.Fields.invln_applicationid,
                        invln_scheme.Fields.OwnerId,
                        invln_scheme.Fields.invln_schemename,
                        invln_scheme.Fields.invln_contactid,
                        invln_scheme.Fields.invln_organisationid,
                        invln_scheme.Fields.invln_programmelookup
                    });
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
                case (int)invln_AHPInternalStatus.Draft:
                    statusLabel = "Draft";
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
                case (int)invln_AHPInternalStatus.Approved:
                    statusLabel = "Approved";
                    break;
                case (int)invln_AHPInternalStatus.ApprovedContractExecuted:
                    statusLabel = "ApprovedContractExecuted";
                    break;
                case (int)invln_AHPInternalStatus.ApprovedContractPassedComplianceChecks:
                    statusLabel = "ApprovedContractPassedComplianceChecks";
                    break;
                case (int)invln_AHPInternalStatus.ApprovedContractReceivedBackToHE:
                    statusLabel = "ApprovedContractReceivedBackToHE";
                    break;
                case (int)invln_AHPInternalStatus.ApprovedEngressmentIssued:
                    statusLabel = "ApprovedEngressmentIssued";
                    break;
                case (int)invln_AHPInternalStatus.ApprovedSubjecttoContract:
                    statusLabel = "ApprovedSubjecttoContract";
                    _govNotifyEmailService.SendNotifications_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT(ahpStatusChange, ahpApplication);
                    break;
                case (int)invln_AHPInternalStatus.Deleted:
                    statusLabel = "Deleted";
                    break;
                case (int)invln_AHPInternalStatus.InternallyApprovedSubjectToIPQ:
                    statusLabel = "InternallyApprovedSubjectToIPQ";
                    break;
                case (int)invln_AHPInternalStatus.InternallyApprovedSubjectToIPQAndRegulatorySignOff:
                    statusLabel = "InternallyApprovedSubjectToIPQAndRegulatorySignOff";
                    break;
                case (int)invln_AHPInternalStatus.InternallyApprovedSubjectToRegulatorSignOff:
                    statusLabel = "InternallyApprovedSubjectToRegulatorSignOff";
                    break;
                case (int)invln_AHPInternalStatus.InternallyRejected:
                    statusLabel = "InternallyRejected";
                    break;
                case (int)invln_AHPInternalStatus.OnHold:
                    statusLabel = "OnHold";
                    if (ahpStatusChange.invln_ChangeSource.Value == (int)invln_ChangesourceSet.External)
                    {
                        _govNotifyEmailService.SendNotifications_AHP_INTERNAL_APPLICATION_ON_HOLD(ahpStatusChange, ahpApplication);
                    }

                    if (ahpStatusChange.invln_ChangeSource.Value == (int)invln_ChangesourceSet.Internal)
                    {
                        _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_APPLICATION_ON_HOLD(ahpStatusChange, ahpApplication);
                    }
                    break;
                case (int)invln_AHPInternalStatus.ReferredBackToApplicant:
                    statusLabel = "ReferredBackToApplicant";
                    _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT(ahpStatusChange, ahpApplication);
                    break;
                case (int)invln_AHPInternalStatus.Rejected:
                    statusLabel = "Rejected";
                    _govNotifyEmailService.SendNotifications_AHP_EXTERNAL_APPLICATION_REJECTED(ahpStatusChange, ahpApplication);
                    break;
                case (int)invln_AHPInternalStatus.RequestedEditing:
                    statusLabel = "RequestedEditing";
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewGoingToBidClinic:
                    statusLabel = "UnderReviewGoingToBidClinic";
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewGoingToCMEModeration:
                    statusLabel = "UnderReviewGoingToCMEModeration";
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewGoingToSLT:
                    statusLabel = "UnderReviewGoingToSLT";
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewInAssessment:
                    statusLabel = "UnderReviewInAssessment";
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewInternallyApproved:
                    statusLabel = "UnderReviewInternallyApproved";
                    break;
                case (int)invln_AHPInternalStatus.UnderReviewPendingAssessment:
                    statusLabel = "UnderReviewPendingAssessment";
                    break;
                case (int)invln_AHPInternalStatus.Withdrawn:
                    statusLabel = "Withdrawn";
                    _govNotifyEmailService.SendNotifications_AHP_INTERNAL_REQUEST_TO_WITHDRAW(ahpStatusChange, ahpApplication);
                    break;

                default:
                    break;
            }

            this.TracingService.Trace("Status Label: " + statusLabel);
            SendInternalCrmNotification(ahpStatusChange, ahpApplication, statusLabel);
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
