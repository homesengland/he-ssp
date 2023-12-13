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
        private readonly IWebRoleRepository _webRoleRepository;
        private readonly IPortalPermissionRepository _portalPermissionRepository;

        public GovNotifyEmailService(CrmServiceArgs args) : base(args)
        {
            _govNotifyEmailRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IGovNotifyEmailRepository>();
            _environmentVariableRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IEnvironmentVariableRepository>();
            _notificationSettingRepositoryAdmin = CrmRepositoriesFactory.GetSystem<INotificationSettingRepository>();
            _systemUserRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ISystemUserRepository>();
            _loanApplicationRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ILoanApplicationRepository>();
            _contactRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IContactRepository>();
            _contactWebroleRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IContactWebroleRepository>();
            _webRoleRepository = CrmRepositoriesFactory.GetSystem<IWebRoleRepository>();
            _portalPermissionRepository = CrmRepositoriesFactory.GetSystem<IPortalPermissionRepository>();
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
                var webrole = _webRoleRepository.GetById(contactWebrole.invln_Webroleid.Id, nameof(invln_Webrole.invln_Portalpermissionlevelid).ToLower());
                if (webrole != null && webrole.invln_Portalpermissionlevelid != null)
                {
                    var portalPermission = _portalPermissionRepository.GetById(webrole.invln_Portalpermissionlevelid.Id, nameof(invln_portalpermissionlevel.invln_Permission).ToLower());
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
