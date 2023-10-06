using System.Text.Encodings.Web;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.GovNotifyEmail
{
    public class GovNotifyEmailService : CrmService, IGovNotifyEmailService
    {

        private readonly IGovNotifyEmailRepository _govNotifyEmailRepositoryAdmin;
        private readonly IEnvironmentVariableRepository _environmentVariableRepositoryAdmin;
        private readonly INotificationSettingRepository _notificationSettingRepositoryAdmin;
        private readonly ISystemUserRepository _systemUserRepositoryAdmin;
        private readonly ILoanApplicationRepository _loanApplicationRepositoryAdmin;

        public GovNotifyEmailService(CrmServiceArgs args) : base(args)
        {
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
        }

        public void SendGovNotifyEmail(EntityReference ownerId, EntityReference regardingObjectId, string subject, string applicationId, string statusAtBody, string entityLogicalName, string recordId)
        {
            var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("INTERNAL_LOAN_APP_STATUS_CHANGE");
            var emailToCreate = new invln_govnotifyemail()
            {
                OwnerId = ownerId,
                RegardingObjectId = regardingObjectId,
                StatusCode = new OptionSetValue((int)invln_govnotifyemail_StatusCode.Draft),
                invln_notificationsettingid = emailTemplate?.ToEntityReference(),
            };
            var emailId = _govNotifyEmailRepositoryAdmin.Create(emailToCreate);

            if (emailTemplate != null)
            {
                var orgUrl = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_environmenturl") ?? "";
                var loanAppId = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_loanappid") ?? "";
                var ownerData = _systemUserRepositoryAdmin.GetById(emailToCreate.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                var govNotParams = new INTERNAL_LOAN_APP_STATUS_CHANGE()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters()
                    {
                        recipientEmail = ownerData.InternalEMailAddress,
                        username = ownerData.FullName,
                        applicationId = applicationId,
                        applicationUrl = orgUrl + "/main.aspx?appid=" + loanAppId + "&pagetype=entityrecord&etn=" + entityLogicalName + "&id=" + recordId,
                        subject = subject,
                        statusAtBody = statusAtBody
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                // TODO: delete after MVP when update possible from gov notify returned data
                _govNotifyEmailRepositoryAdmin.Update(new invln_govnotifyemail()
                {
                    Id = emailId,
                    Subject = subject,
                    invln_body = JsonSerializer.Serialize(govNotParams, options)
                });
                //
                var govNotReq = new invln_sendgovnotifyemailRequest()
                {
                    invln_emailid = emailId.ToString(),
                    invln_govnotifyparameters = JsonSerializer.Serialize(govNotParams, options),
                };
                _ = _loanApplicationRepositoryAdmin.ExecuteGovNotifyNotificationRequest(govNotReq);
            }
        }
    }
}
