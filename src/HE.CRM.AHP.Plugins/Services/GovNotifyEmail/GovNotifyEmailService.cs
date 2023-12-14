using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.interfaces;
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
        private readonly IContactWebroleRepository _contactWebroleRepositoryAdmin;
        private readonly IWebRoleRepository _webRoleRepositoryAdmin;
        private readonly IPortalPermissionRepository _portalPermissionRepositoryAdmin;
        private readonly IAhpApplicationRepository _ahpApplicationRepositoryAdmin;
        private readonly IAccountRepository _accountRepositoryAdmin;

        public GovNotifyEmailService(CrmServiceArgs args) : base(args)
        {
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _contactRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IContactRepository>();
            _contactWebroleRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IContactWebroleRepository>();
            _webRoleRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IWebRoleRepository>();
            _portalPermissionRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IPortalPermissionRepository>();
            _ahpApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IAhpApplicationRepository>();
            _accountRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IAccountRepository>();
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

        public void SendNotifications_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS(EntityReference contactWebroleId)
        {
            var contactWebrole = _contactWebroleRepositoryAdmin.GetById(contactWebroleId.Id, nameof(invln_contactwebrole.invln_Contactid).ToLower());
            if (contactWebrole != null && contactWebrole.invln_Contactid != null)
            {
                var contact = _contactRepositoryAdmin.GetById(contactWebrole.invln_Contactid.Id, nameof(Contact.OwnerId).ToLower(), nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());

                if (contact != null && contact.OwnerId != null)
                {
                    this.TracingService.Trace("COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS");
                    var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS");
                    var govNotParams = new COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters_COMMON_CHANGE_EXTERNAL_USER_PERMISSIONS()
                        {
                            recipientEmail = contact.EMailAddress1,
                            username = contact.FullName ?? "NO NAME",
                            subject = emailTemplate.invln_subject,
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    var parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(contact.OwnerId, contact.ToEntityReference(), emailTemplate.invln_subject, parameters, emailTemplate);
                }
            }
        }

        public void SendNotifications_COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS(EntityReference contactWebroleId)
        {
            var contactWebrole = _contactWebroleRepositoryAdmin.GetById(contactWebroleId.Id, nameof(invln_contactwebrole.invln_Contactid).ToLower(), nameof(invln_contactwebrole.invln_Webroleid).ToLower());
            if (contactWebrole != null && contactWebrole.invln_Contactid != null && contactWebrole.invln_Webroleid != null)
            {
                var webrole = _webRoleRepositoryAdmin.GetById(contactWebrole.invln_Webroleid.Id, nameof(invln_Webrole.invln_Portalpermissionlevelid).ToLower());
                if (webrole != null && webrole.invln_Portalpermissionlevelid != null)
                {
                    var portalPermission = _portalPermissionRepositoryAdmin.GetById(webrole.invln_Portalpermissionlevelid.Id, nameof(invln_portalpermissionlevel.invln_Permission).ToLower());
                    if (portalPermission.invln_Permission != null && portalPermission.invln_Permission.Value == (int)invln_Permission.Admin)
                    {
                        this.Logger.Info("Granted Admin Permissions!");
                        var contact = _contactRepositoryAdmin.GetById(contactWebrole.invln_Contactid.Id, nameof(Contact.OwnerId).ToLower(), nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());
                        if (contact != null && contact.OwnerId != null)
                        {
                            this.TracingService.Trace("COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS");
                            var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS");
                            var govNotParams = new COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS()
                            {
                                templateId = emailTemplate?.invln_templateid,
                                personalisation = new parameters_COMMON_GRANT_ORGANISATION_ADMIN_PERMISSIONS()
                                {
                                    recipientEmail = contact.EMailAddress1,
                                    username = contact.FullName ?? "NO NAME",
                                    subject = emailTemplate.invln_subject,
                                }
                            };

                            var options = new JsonSerializerOptions
                            {
                                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                                WriteIndented = true
                            };

                            var parameters = JsonSerializer.Serialize(govNotParams, options);
                            this.SendGovNotifyEmail(contact.OwnerId, contact.ToEntityReference(), emailTemplate.invln_subject, parameters, emailTemplate);
                        }
                    }
                }
            }
        }

        public void SendNotifications_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION(EntityReference invitedContactId, EntityReference _inviterContactId, EntityReference organisationId)
        {
            if (invitedContactId != null && _inviterContactId != null && organisationId != null)
            {
                var invitedContact = _contactRepositoryAdmin.GetById(invitedContactId.Id, nameof(Contact.OwnerId).ToLower(), nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());
                var _inviterContact = _contactRepositoryAdmin.GetById(_inviterContactId.Id, nameof(Contact.FullName).ToLower());
                var account = _accountRepositoryAdmin.GetById(organisationId.Id, nameof(Account.Name).ToLower());

                if (invitedContact != null && invitedContact.OwnerId != null && _inviterContact != null && organisationId != null)
                {
                    this.TracingService.Trace("COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION");
                    var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION");
                    var govNotParams = new COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters_COMMON_INVITE_CONTACT_TO_JOIN_ORGANIZATION()
                        {
                            recipientEmail = invitedContact.EMailAddress1,
                            username = invitedContact.FullName ?? "NO NAME",
                            subject = emailTemplate.invln_subject,
                            invitername = _inviterContact.FullName ?? "NO NAME",
                            organisationname = account.Name
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    var parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(invitedContact.OwnerId, invitedContact.ToEntityReference(), emailTemplate.invln_subject, parameters, emailTemplate);
                }
            }
        }

        public void SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(EntityReference ahpApplicationId, EntityReference contactId)
        {
            if (ahpApplicationId != null && contactId != null)
            {
                var contact = _contactRepositoryAdmin.GetById(contactId.Id, nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());
                var ahpApplication = _ahpApplicationRepositoryAdmin.GetById(ahpApplicationId.Id, nameof(Contact.OwnerId).ToLower());

                this.TracingService.Trace("AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK");
                var govNotParams = new AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK()
                    {
                        recipientEmail = contact.EMailAddress1,
                        username = contact.FullName ?? "NO NAME",
                        subject = emailTemplate.invln_subject,
                        programmename = "CME"
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplicationId, emailTemplate.invln_subject, parameters, emailTemplate);
            }
        }

        public void SendReminderEmailForRefferedBackToApplicant(Guid applicationId)
        {
            var application = _ahpApplicationRepositoryAdmin.GetById(applicationId, new string[] { nameof(invln_scheme.invln_contactid).ToLower(), nameof(invln_scheme.invln_lastexternalmodificationby).ToLower() });
            if (application != null)
            {
                if (application.invln_contactid != null)
                {
                    SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(application.ToEntityReference(), application.invln_contactid);
                }
                if (application.invln_lastexternalmodificationby != null &&
                    (application.invln_contactid == null || (application.invln_contactid != null && application.invln_lastexternalmodificationby.Id != application.invln_contactid.Id)))
                {
                    SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_APPLICATION_REFERRED_BACK(application.ToEntityReference(), application.invln_lastexternalmodificationby);
                }
            }
        }

        public void SendReminderEmailForFinaliseDraftApplication(Guid applicationId)
        {
            var application = _ahpApplicationRepositoryAdmin.GetById(applicationId, new string[] { nameof(invln_scheme.invln_contactid).ToLower()});
            if (application != null && application.invln_contactid != null)
            {
                // send email
            }
        }


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
            this.TracingService.Trace("Create invln_govnotifyemail");
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
