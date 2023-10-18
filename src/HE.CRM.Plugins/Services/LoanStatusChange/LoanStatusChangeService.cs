using System;
using System.Drawing.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using HE.CRM.Plugins.Services.GovNotifyEmail;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.LoanStatusChange
{
    public class LoanStatusChangeService : CrmService, ILoanStatusChangeService
    {
        #region Fields
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly IGovNotifyEmailService _govNotifyEmailService;

        private readonly ILoanApplicationRepository _loanApplicationRepositoryAdmin;
        private readonly IGovNotifyEmailRepository _govNotifyEmailRepositoryAdmin;
        private readonly IEnvironmentVariableRepository _environmentVariableRepositoryAdmin;
        private readonly INotificationSettingRepository _notificationSettingRepositoryAdmin;
        private readonly ISystemUserRepository _systemUserRepositoryAdmin;
        #endregion

        #region Constructors

        public LoanStatusChangeService(CrmServiceArgs args) : base(args)
        {
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();

            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
        }
        #endregion

        #region Public Methods

        public void SendNotificationOnLoanStatusChangeCreate(invln_Loanstatuschange target)
        {
            if (target.invln_changesource != null)
            {
                var loanApplication = _loanApplicationRepository.GetById(target.invln_Loanapplication.Id, new string[] { nameof(invln_Loanapplication.OwnerId).ToLower(), nameof(invln_Loanapplication.invln_Name).ToLower(), nameof(invln_Loanapplication.invln_Contact).ToLower() });
                SendNotification(target, loanApplication);
            }
        }
        #endregion

        #region Private Methods

        private void SendNotification(invln_Loanstatuschange loanStatusChange, invln_Loanapplication loanApplication)
        {
            var statusLabel = string.Empty;
            var pastFormStatus = string.Empty;
            switch (loanStatusChange.invln_changeto.Value)
            {
                case (int)invln_InternalStatus.Draft:
                    statusLabel = "Draft";
                    pastFormStatus = "is now being edited by the applicant";
                    if (loanStatusChange.invln_changefrom == null)
                    {
                        _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(loanStatusChange, loanApplication, "saved your development loan application as draft");
                    }
                    break;
                case (int)invln_InternalStatus.ApplicationSubmitted:
                    statusLabel = "Application submitted";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(loanStatusChange, loanApplication, "submitted your application");
                    break;
                case (int)invln_InternalStatus.Inactive:
                    statusLabel = "Inactive";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.ApplicationUnderReview:
                    statusLabel = "Application under review";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_INFORMATION(loanStatusChange, loanApplication);
                    break;
                case (int)invln_InternalStatus.HoldRequested:
                    statusLabel = "Hold requested";
                    pastFormStatus = "has been requested to be put on hold";
                    break;
                case (int)invln_InternalStatus.Withdrawn:
                    statusLabel = "Withdrawn";
                    pastFormStatus = "has been withdrawn";
                    if (loanStatusChange.invln_changesource?.Value == (int)invln_ChangesourceSet.External)
                    {
                        _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(loanStatusChange, loanApplication, "withdrawn your application");
                    }
                    else if(loanStatusChange.invln_changesource?.Value == (int)invln_ChangesourceSet.Internal)
                    {
                        _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_INFORMATION(loanStatusChange, loanApplication);
                    }
                    break;
                case (int)invln_InternalStatus.CashflowRequested:
                    statusLabel = "Cashflow requested";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.CashflowUnderReview:
                    statusLabel = "Cashflow under review";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.OnHold:
                    statusLabel = "On hold";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.ReferredBacktoApplicant:
                    statusLabel = "Reffered back to applicant";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.UnderReview:
                    statusLabel = "Under review";
                    pastFormStatus = "has been submitted by the borrower for your review";
                    _govNotifyEmailService.SendNotifications_EXTERNAL_APPLICATION_STATUS_INFORMATION(loanStatusChange, loanApplication);
                    break;
                case (int)invln_InternalStatus.SentforApproval:
                    statusLabel = "Sent for approval";
                    pastFormStatus = "has been sent for your approval";
                    break;
                case (int)invln_InternalStatus.NotApproved:
                    statusLabel = "Not Approved";
                    pastFormStatus = "has not been approved and has been returned";
                    break;
                case (int)invln_InternalStatus.ApprovedSubjecttoDueDiligence:
                    statusLabel = "Approved subject to due diligence";
                    pastFormStatus = "ISP has been approved";
                    break;
                case (int)invln_InternalStatus.ApplicationDeclined:
                    statusLabel = "Application declined";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.InDueDiligence:
                    statusLabel = "In Due Diligence";
                    pastFormStatus = "has been returned without approval on Pre-Complete";
                    break;
                case (int)invln_InternalStatus.SentforPreCompleteApproval:
                    statusLabel = "Sent for Pre-Complete Approval";
                    pastFormStatus = "has been sent for Pre-Complete approval";
                    break;
                case (int)invln_InternalStatus.ApprovedSubjectToContract:
                    statusLabel = "Approved Subject to Contract";
                    pastFormStatus = "has been approved subject to contract";
                    break;
                case (int)invln_InternalStatus.CPsSatisfied:
                    statusLabel = "CPs Satisfied";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.AwaitingCPSatisfaction:
                    statusLabel = "Awaiting CP satisfaction";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.LoanAvailable:
                    statusLabel = "Loan available";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                default:
                    break;
            }

            SendInternalCrmNotification(loanStatusChange, loanApplication, statusLabel);
            _govNotifyEmailService.SendNotifications_INTERNAL_LOAN_APP_STATUS_CHANGE(loanStatusChange, loanApplication, statusLabel, pastFormStatus);

        }

        public void SendInternalCrmNotification(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string statusLabel)
        {
            var internalNotification = new invln_sendinternalcrmnotificationRequest()
            {
                invln_notificationbody = $"[Application ref no {loanApplication.invln_Name} - Status change to '{statusLabel}](?pagetype=entityrecord&etn=invln_loanapplication&id={loanApplication.Id})'",
                invln_notificationowner = loanApplication.OwnerId.Id.ToString(),
                invln_notificationtitle = "Information",
            };
            _ = _loanApplicationRepositoryAdmin.ExecuteNotificatioRequest(internalNotification);
        }
        #endregion
    }
}
