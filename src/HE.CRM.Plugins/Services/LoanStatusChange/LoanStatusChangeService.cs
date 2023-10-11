using System.Drawing.Text;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
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
        #endregion

        #region Constructors

        public LoanStatusChangeService(CrmServiceArgs args) : base(args)
        {
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();

            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();

            _govNotifyEmailService = CrmServicesFactory.Get<IGovNotifyEmailService>();
        }
        #endregion

        #region Public Methods

        public void SendNotificationOnCreate(invln_Loanstatuschange target)
        {
            if (target.invln_changesource != null)
            {
                var loanApplication = _loanApplicationRepository.GetById(target.invln_Loanapplication.Id, new string[] { nameof(invln_Loanapplication.OwnerId).ToLower(), nameof(invln_Loanapplication.invln_Name).ToLower() });
                var loanApplicationToUpdate = new invln_Loanapplication()
                {
                    Id = loanApplication.Id,
                    OwnerId = loanApplication.OwnerId,
                };

                if (target.invln_changesource.Value == (int)invln_ChangesourceSet.Internal)
                {
                    InternalStatusPart(target, loanApplicationToUpdate);
                }
                //_loanApplicationRepository.Update(loanApplicationToUpdate);
            }
        }
        #endregion

        #region Private Methods

        private void InternalStatusPart(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication)
        {
            var statusLabel = string.Empty;
            var pastFormStatus = string.Empty;
            SetLabelAndPastFormStatus(statusChange, ref statusLabel, ref pastFormStatus);
            var owner = loanApplication.OwnerId;
            if (owner.LogicalName != Team.EntityLogicalName)
            {
                SendNotifications(statusChange, loanApplication, statusLabel, pastFormStatus);
            }
        }

        private void SetLabelAndPastFormStatus(invln_Loanstatuschange statusChange, ref string statusLabel, ref string pastFormStatus)
        {
            switch (statusChange.invln_changeto.Value)
            {
                case (int)invln_InternalStatus.Draft:
                    statusLabel = "Draft";
                    pastFormStatus = "is now being edited by the applicant";
                    break;
                case (int)invln_InternalStatus.ApplicationSubmitted:
                    statusLabel = "Application submitted";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.Inactive:
                    statusLabel = "Inactive";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.ApplicationUnderReview:
                    statusLabel = "Application under review";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.HoldRequested:
                    statusLabel = "Hold requested";
                    pastFormStatus = "has been requested to be put on hold";
                    break;
                case (int)invln_InternalStatus.Withdrawn:
                    statusLabel = "Withdrawn";
                    pastFormStatus = "has been withdrawn";
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
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.SentforApproval:
                    statusLabel = "Sent for approval";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
                    break;
                case (int)invln_InternalStatus.NotApproved:
                    statusLabel = "Not Approved";
                    pastFormStatus = "has not been approved and has been returned";
                    break;
                case (int)invln_InternalStatus.ApprovedSubjecttoDueDiligence:
                    statusLabel = "Approved subject to due diligence";
                    pastFormStatus = "has been changed to " + statusLabel; //TODO: to update
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
        }

        private void SendNotifications(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string statusLabel, string pastFormStatus)
        {
            var req1 = new invln_sendinternalcrmnotificationRequest()
            {
                invln_notificationbody = $"[Application ref no {loanApplication.invln_Name} - Status change to '{statusLabel}](?pagetype=entityrecord&etn=invln_loanapplication&id={loanApplication.Id})'",
                invln_notificationowner = loanApplication.OwnerId.Id.ToString(),
                invln_notificationtitle = "Information",
            };
            _ = _loanApplicationRepositoryAdmin.ExecuteNotificatioRequest(req1);

            var subject = $"Application ref no {loanApplication.invln_Name} - Status change to '{statusLabel}'";
            _govNotifyEmailService.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), subject, loanApplication.invln_Name, pastFormStatus, invln_Loanapplication.EntityLogicalName.ToLower(), loanApplication.Id.ToString());

        }
        #endregion
    }
}
