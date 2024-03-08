using System.Text.Encodings.Web;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerialiedParameters;
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
        private readonly IContactRepository _contactRepositoryAdmin;

        public GovNotifyEmailService(CrmServiceArgs args) : base(args)
        {
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _contactRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IContactRepository>();
        }

        public void SendNotifications_EXTERNAL_APPLICATION_ACTION_REQUIRED(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string actionRequired)
        {
            if (loanApplication.invln_Contact != null)
            {
                this.TracingService.Trace("EXTERNAL_APPLICATION_ACTION_REQUIRED");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("EXTERNAL_APPLICATION_ACTION_REQUIRED");
                var loanContactData = _contactRepositoryAdmin.GetById(loanApplication.invln_Contact.Id, nameof(Contact.EMailAddress1).ToLower(), nameof(SystemUser.FullName).ToLower());
                var govNotParams = new EXTERNAL_APPLICATION_ACTION_REQUIRED()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_EXTERNAL_APPLICATION_ACTION_REQUIRED()
                    {
                        recipientEmail = loanContactData.EMailAddress1,
                        username = loanContactData.FullName ?? "NO NAME",
                        subject = emailTemplate.invln_subject,
                        actionRequired = actionRequired,
                        applicationId = loanApplication.invln_Name
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), emailTemplate.invln_subject, parameters, emailTemplate);
            }
        }

        public void SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string actionCompleted)
        {
            if (loanApplication.invln_Contact != null)
            {
                this.TracingService.Trace("EXTERNAL_APPLICATION_STATUS_CONFIRMATION");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("EXTERNAL_APPLICATION_STATUS_CONFIRMATION");
                var loanContactData = _contactRepositoryAdmin.GetById(loanApplication.invln_Contact.Id, nameof(Contact.EMailAddress1).ToLower(), nameof(SystemUser.FullName).ToLower());
                var subject = "You have " + actionCompleted;
                var govNotParams = new EXTERNAL_APPLICATION_STATUS_CONFIRMATION()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_EXTERNAL_APPLICATION_STATUS_CONFIRMATION()
                    {
                        recipientEmail = loanContactData.EMailAddress1,
                        username = loanContactData.FullName ?? "NO NAME",
                        subject = subject,
                        actionCompleted = actionCompleted
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), subject, parameters, emailTemplate);
            }
        }

        public void SendNotifications_EXTERNAL_APPLICATION_STATUS_INFORMATION(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication)
        {
            if (loanApplication.invln_Contact != null)
            {
                this.TracingService.Trace("EXTERNAL_APPLICATION_STATUS_INFORMATION");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("EXTERNAL_APPLICATION_STATUS_INFORMATION");
                var loanContactData = _contactRepositoryAdmin.GetById(loanApplication.invln_Contact.Id, nameof(Contact.EMailAddress1).ToLower(), nameof(SystemUser.FullName).ToLower());
                var subject = emailTemplate?.invln_subject;
                var govNotParams = new EXTERNAL_APPLICATION_STATUS_INFORMATION()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_EXTERNAL_APPLICATION_STATUS_INFORMATION()
                    {
                        recipientEmail = loanContactData.EMailAddress1,
                        username = loanContactData.FullName ?? "NO NAME",
                        subject = subject,
                        applicationId = loanApplication.invln_Name,
                        previousStatus = statusChange.FormattedValues[nameof(invln_Loanstatuschange.invln_changefrom).ToLower()],
                        newStatus = statusChange.FormattedValues[nameof(invln_Loanstatuschange.invln_changeto).ToLower()],
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), subject, parameters, emailTemplate);

            }
        }

        public void SendNotifications_INTERNAL_LOAN_APP_STATUS_CHANGE(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string statusLabel, string pastFormStatus)
        {
            if (loanApplication.OwnerId.LogicalName == SystemUser.EntityLogicalName)
            {
                this.TracingService.Trace("INTERNAL_LOAN_APP_STATUS_CHANGE");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("INTERNAL_LOAN_APP_STATUS_CHANGE");
                var subject = $"Application ref no {loanApplication.invln_Name} - Status change to '{statusLabel}'";
                var ownerData = _systemUserRepositoryAdmin.GetById(loanApplication.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                var govNotParams = new INTERNAL_LOAN_APP_STATUS_CHANGE()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_INTERNAL_LOAN_APP_STATUS_CHANGE()
                    {
                        recipientEmail = ownerData.InternalEMailAddress,
                        username = ownerData.FullName ?? "NO NAME",
                        applicationId = loanApplication.invln_Name,
                        applicationUrl = GetLoanApplicationUrl(loanApplication.ToEntityReference()),
                        subject = subject,
                        statusAtBody = pastFormStatus
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), subject, parameters, emailTemplate);

            }
        }

        public void SendNotifications_INTERNAL_LOAN_APP_OWNER_CHANGE(invln_Loanapplication loanApplication, string subject, string appId)
        {
            if (loanApplication.OwnerId.LogicalName == SystemUser.EntityLogicalName)
            {
                this.TracingService.Trace("INTERNAL_LOAN_APP_OWNER_CHANGE");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("INTERNAL_LOAN_APP_OWNER_CHANGE");
                var ownerData = _systemUserRepositoryAdmin.GetById(loanApplication.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                var govNotParams = new INTERNAL_LOAN_APP_OWNER_CHANGE()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_INTERNAL_LOAN_APP_OWNER_CHANGE()
                    {
                        recipientEmail = ownerData.InternalEMailAddress,
                        username = ownerData.FullName ?? "NO NAME",
                        applicationId = appId,
                        applicationUrl = GetLoanApplicationUrl(loanApplication.ToEntityReference()),
                        subject = subject,
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), subject, parameters, emailTemplate);

            }
        }

        public void SendNotifications_EXTERNAL_KYC_STATUS_CHANGE(Contact contact, string subject, Account organisation)
        {
            var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("EXTERNAL_KYC_STATUS_CHANGE");
            var govNotParams = new EXTERNAL_KYC_STATUS_CHANGE()
            {
                templateId = emailTemplate?.invln_templateid,
                personalisation = new parameters_EXTERNAL_KYC_STATUS_CHANGE()
                {
                    recipientEmail = contact.EMailAddress1,
                    subject = subject,
                    username = contact.FullName,
                }
            };
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            var parameters = JsonSerializer.Serialize(govNotParams, options);
            this.SendGovNotifyEmail(organisation.OwnerId, organisation.ToEntityReference(), subject, parameters, emailTemplate);
        }

        public void SendNotifications_EXTERNAL_APPLICATION_CASHFLOW_REQUESTED(invln_Loanapplication loanApplication)
        {
            if (loanApplication.invln_Contact != null)
            {
                this.TracingService.Trace("EXTERNAL_APPLICATION_CASHFLOW_REQUESTED");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("EXTERNAL_APPLICATION_CASHFLOW_REQUESTED");
                var loanContactData = _contactRepositoryAdmin.GetById(loanApplication.invln_Contact.Id, nameof(Contact.EMailAddress1).ToLower(), nameof(SystemUser.FullName).ToLower());

                var govNotParams = new EXTERNAL_APPLICATION_CASHFLOW_REQUESTED()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_EXTERNAL_APPLICATION_CASHFLOW_REQUESTED()
                    {
                        recipientEmail = loanContactData.EMailAddress1,
                        username = loanContactData.FullName ?? "NO NAME",
                        subject = emailTemplate?.invln_subject,
                        applicationId = loanApplication.invln_Name
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(loanApplication.OwnerId, loanApplication.ToEntityReference(), emailTemplate.invln_subject, parameters, emailTemplate);
            }
        }

        private string GetLoanApplicationUrl(EntityReference loanApplicationId)
        {
            if (loanApplicationId != null)
            {
                var orgUrl = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_environmenturl") ?? "";
                var loanAppId = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_loanappid") ?? "";
                return orgUrl + "/main.aspx?appid=" + loanAppId + "&pagetype=entityrecord&etn=" + invln_Loanapplication.EntityLogicalName.ToLower() + "&id=" + loanApplicationId.Id.ToString();
            }

            return string.Empty;
        }

        private void SendGovNotifyEmail(EntityReference ownerId, EntityReference regardingObjectId, string subject, string serializedParameters, invln_notificationsetting emailTemplate)
        {
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
                // TODO: delete after MVP when update possible from gov notify returned data
                _govNotifyEmailRepositoryAdmin.Update(new invln_govnotifyemail()
                {
                    Id = emailId,
                    Subject = subject,
                    invln_body = serializedParameters
                });
                //

                var govNotReq = new invln_sendgovnotifyemailRequest()
                {
                    invln_emailid = emailId.ToString(),
                    invln_govnotifyparameters = serializedParameters
                };
                _ = _loanApplicationRepositoryAdmin.ExecuteGovNotifyNotificationRequest(govNotReq);
            }
        }
    }
}
