using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerialiedParameters;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.GovNotifyEmail
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

        //public void SendNotifications_EXTERNAL_APPLICATION_ACTION_REQUIRED(invln_AHPStatusChange statusChange, invln_scheme ahpApplication, string actionRequired)
        //{
        //    if (ahpApplication.invln_contactid != null)
        //    {
        //        this.TracingService.Trace("EXTERNAL_APPLICATION_ACTION_REQUIRED");
        //        var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("EXTERNAL_APPLICATION_ACTION_REQUIRED");
        //        var loanContactData = _contactRepositoryAdmin.GetById(ahpApplication.invln_contactid.Id, nameof(Contact.EMailAddress1).ToLower(), nameof(SystemUser.FullName).ToLower());
        //        var govNotParams = new EXTERNAL_APPLICATION_ACTION_REQUIRED()
        //        {
        //            templateId = emailTemplate?.invln_templateid,
        //            personalisation = new parameters_EXTERNAL_APPLICATION_ACTION_REQUIRED()
        //            {
        //                recipientEmail = loanContactData.EMailAddress1,
        //                username = loanContactData.FullName ?? "NO NAME",
        //                subject = emailTemplate.invln_subject,
        //                actionRequired = actionRequired,
        //                applicationId = ahpApplication.invln_schemename
        //            }
        //        };

        //        var options = new JsonSerializerOptions
        //        {
        //            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //            WriteIndented = true
        //        };

        //        var parameters = JsonSerializer.Serialize(govNotParams, options);
        //        this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), emailTemplate.invln_subject, parameters, emailTemplate);
        //    }
        //}

        private string GetAhpApplicationUrl(EntityReference ahpApplicationId)
        {
            if (ahpApplicationId != null)
            {
                var orgUrl = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_environmenturl") ?? "";
                var ahpAppId = _environmentVariableRepositoryAdmin.GetEnvironmentVariableValue("invln_ahpappid") ?? "";
                return orgUrl + "/main.aspx?appid=" + ahpAppId + "&pagetype=entityrecord&etn=" + invln_scheme.EntityLogicalName.ToLower() + "&id=" + ahpApplicationId.Id.ToString();
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
