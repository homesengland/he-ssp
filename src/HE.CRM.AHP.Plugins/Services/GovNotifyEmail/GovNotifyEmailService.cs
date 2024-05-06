using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepositoryAdmin;
        private readonly IProgrammeRepository _programmeRepositoryAdmin;

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
            _deliveryPhaseRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IDeliveryPhaseRepository>();
            _programmeRepositoryAdmin = CrmRepositoriesFactory.GetSystem<IProgrammeRepository>();
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

        public void SendNotifications_AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION(EntityReference ahpApplicationId, EntityReference contactId)
        {
            if (ahpApplicationId != null && contactId != null)
            {
                var contact = _contactRepositoryAdmin.GetById(contactId.Id, nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());
                var ahpApplication = _ahpApplicationRepositoryAdmin.GetById(ahpApplicationId.Id, nameof(Contact.OwnerId).ToLower());

                this.TracingService.Trace("AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION");
                var govNotParams = new AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_AHP_EXTERNAL_REMINDER_TO_FINALIZE_DRAFT_APPLICATION()
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
                this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplicationId, emailTemplate.invln_subject, parameters, emailTemplate);
            }
        }

        public void SendNotifications_AHP_INTERNAL_EXTERNAL_WANTS_ADDITIONAL_PAYMENTS_FOR_PHASE(EntityReference deliveryPhaseId)
        {
            var deliveryPhase = _deliveryPhaseRepositoryAdmin.GetById(deliveryPhaseId.Id, nameof(invln_DeliveryPhase.invln_Application).ToLower());
            var ahpApplicationId = deliveryPhase.invln_Application;
            var ahpApplication = _ahpApplicationRepositoryAdmin.GetById(ahpApplicationId.Id, nameof(Contact.OwnerId).ToLower(), nameof(invln_scheme.invln_organisationid).ToLower());

            if (ahpApplication.OwnerId.LogicalName == SystemUser.EntityLogicalName)
            {
                this.TracingService.Trace("AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE");
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE");
                var ownerData = _systemUserRepositoryAdmin.GetById(ahpApplication.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                var account = _accountRepositoryAdmin.GetById(ahpApplication.invln_organisationid.Id, nameof(Account.Name).ToLower());
                var subject = (account.Name ?? "NO NAME") + " " + emailTemplate.invln_subject;
                var govNotParams = new AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_AHP_DELIVERY_PHASE_NOTIFICATION_OF_ADDITIONAL_PAYMENTS_FOR_PHASE()
                    {
                        recipientEmail = ownerData.InternalEMailAddress,
                        subject = subject,
                        username = ownerData.FullName ?? "NO NAME",
                        organisationname = account.Name ?? "NO NAME",
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
            }
        }

        public void SendNotifications_AHP_INTERNAL_REQUEST_TO_WITHDRAW(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            if (ahpStatusChange.invln_ChangeSource.Value == (int)invln_ChangesourceSet.External)
            {
                TracingService.Trace("AHP_INTERNAL_REQUEST_TO_WITHDRAW");
                var account = _accountRepositoryAdmin.GetById(ahpApplication.invln_organisationid.Id, nameof(Account.Name).ToLower());
                var ownerData = _systemUserRepositoryAdmin.GetById(ahpApplication.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_INTERNAL_REQUEST_TO_WITHDRAW");

                if (account != null && ownerData != null && emailTemplate != null)
                {
                    var subject = emailTemplate.invln_subject;
                    var govNotParams = new AHP_INTERNAL_REQUEST_TO_WITHDRAW()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters_AHP_INTERNAL_REQUEST_TO_WITHDRAW()
                        {
                            recipientEmail = ownerData.InternalEMailAddress,
                            subject = subject,
                            username = ownerData.FullName ?? "NO NAME",
                            organisationname = account.Name ?? "NO NAME",
                            reason = ahpStatusChange.invln_Comment,
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    var parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
                }
                else
                {
                    TracingService.Trace("Probably there is no email template. Mail not sent.");
                }
            }
        }

        public void SendNotifications_AHP_EXTERNAL_APPLICATION_SUBMITTED(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            if (ahpStatusChange.invln_ChangeSource.Value == (int)invln_ChangesourceSet.External)
            {
                TracingService.Trace("AHP_EXTERNAL_APPLICATION_SUBMITTED");
                var contact = _contactRepositoryAdmin.GetById(ahpApplication.invln_contactid.Id, nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_EXTERNAL_APPLICATION_SUBMITTED");

                if (contact != null && emailTemplate != null)
                {
                    var subject = emailTemplate.invln_subject;
                    var govNotParams = new AHP_EXTERNAL_APPLICATION_SUBMITTED()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters_AHP_EXTERNAL_APPLICATION_SUBMITTED()
                        {
                            recipientEmail = contact.EMailAddress1,
                            subject = subject,
                            name = contact.FullName ?? "NO NAME",
                            referencenumber = ahpApplication.invln_applicationid ?? "NO NAME",
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    var parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
                }
                else
                {
                    TracingService.Trace("Probably there is no email template. Mail not sent.");
                }
            }
        }

        public void SendNotifications_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            if (ahpStatusChange.invln_ChangeSource.Value == (int)invln_ChangesourceSet.External)
            {
                TracingService.Trace("AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT");
                var contact = _contactRepositoryAdmin.GetById(ahpApplication.invln_contactid.Id, Contact.Fields.FullName, Contact.Fields.EMailAddress1);
                if (ahpApplication.invln_programmelookup == null)
                {
                    TracingService.Trace("There is no programme on ahpApplication. Mail not sent.");
                    return;
                }
                var programme = _programmeRepositoryAdmin.GetById(ahpApplication.invln_programmelookup.Id, invln_programme.Fields.invln_programmename);
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT");

                if (contact != null && programme != null && emailTemplate != null)
                {
                    var subject = emailTemplate.invln_subject;
                    var govNotParams = new AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters_AHP_EXTERNAL_APPLICATION_REFERRED_BACK_TO_APPLICANT()
                        {
                            recipientEmail = contact.EMailAddress1,
                            subject = subject,
                            name = contact.FullName ?? "NO NAME",
                            schemename = ahpApplication.invln_schemename ?? "NO NAME",
                            programmename = programme.invln_programmename ?? "NO NAME",
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    var parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
                }
                else
                {
                    TracingService.Trace("Probably there is no email template. Mail not sent.");
                }
            }
        }

        public void SendNotifications_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            if (ahpStatusChange.invln_ChangeSource.Value == (int)invln_ChangesourceSet.Internal)
            {
                TracingService.Trace("AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT");
                var account = _accountRepositoryAdmin.GetById(ahpApplication.invln_organisationid.Id, Account.Fields.invln_ProviderManagementLead, Account.Fields.Name);
                var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT");
                SystemUser user = null;
                if (account.invln_ProviderManagementLead != null)
                {
                    user = _systemUserRepositoryAdmin.GetById(account.invln_ProviderManagementLead.Id, SystemUser.Fields.FullName, SystemUser.Fields.DomainName);
                }

                if (account != null && user != null && emailTemplate != null)
                {
                    var subject = emailTemplate.invln_subject.Replace("[SchemeName]", ahpApplication.invln_schemename);

                    var govNotParams = new AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT()
                    {
                        templateId = emailTemplate?.invln_templateid,
                        personalisation = new parameters_AHP_INTERNAL_APPLICATION_APPROVED_SUBJECT_TO_CONTRACT()
                        {
                            recipientEmail = "",
                            subject = subject,
                            name = user.FullName ?? "NO NAME",
                            organisationname = account.Name ?? "NO NAME",
                            applicationname = ahpApplication.invln_schemename ?? "NO NAME",
                            applicationurl = GetAhpApplicationUrl(ahpApplication.ToEntityReference()),
                        }
                    };

                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };


                    govNotParams.personalisation.recipientEmail = user.DomainName;
                    var parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);


                    govNotParams.personalisation.recipientEmail = "Housing.Contracts@homesengland.gov.uk";
                    parameters = JsonSerializer.Serialize(govNotParams, options);
                    this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
                }
                else
                {
                    TracingService.Trace("Probably there is no email template or invln_ProviderManagementLead on Account. Mail not sent.");
                }
            }
        }

        public void SendNotifications_AHP_INTERNAL_APPLICATION_ON_HOLD(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            TracingService.Trace("AHP_INTERNAL_APPLICATION_ON_HOLD");
            var account = _accountRepositoryAdmin.GetById(ahpApplication.invln_organisationid.Id, Account.Fields.Name);
            var ownerData = _systemUserRepositoryAdmin.GetById(ahpApplication.OwnerId.Id, nameof(SystemUser.InternalEMailAddress).ToLower(), nameof(SystemUser.FullName).ToLower());

            if (ahpApplication.invln_programmelookup == null)
            {
                TracingService.Trace("There is no programme on ahpApplication. Mail not sent.");
                return;
            }
            var programme = _programmeRepositoryAdmin.GetById(ahpApplication.invln_programmelookup.Id, invln_programme.Fields.invln_programmename);

            if (ahpStatusChange.invln_changedby == null)
            {
                TracingService.Trace("There is no changedby on ahpStatusChange. Mail not sent.");
                return;
            }
            var contact = _contactRepositoryAdmin.GetById(ahpStatusChange.invln_changedby.Id, nameof(Contact.FullName).ToLower());

            var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_INTERNAL_APPLICATION_ON_HOLD");

            if (account != null && ownerData != null && programme != null && contact != null && emailTemplate != null)
            {
                var subject = emailTemplate.invln_subject.Replace("[Organisation_name]", account.Name);
                subject = subject.Replace("[Programme_name]", programme.invln_programmename);

                var govNotParams = new AHP_INTERNAL_APPLICATION_ON_HOLD()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_AHP_INTERNAL_APPLICATION_ON_HOLD()
                    {
                        recipientEmail = ownerData.InternalEMailAddress,
                        subject = subject,
                        name = ownerData.FullName ?? "NO NAME",
                        organisationname = account.Name ?? "NO NAME",
                        nameofuserwhoputapplicationonhold = contact.FullName,
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
            }
            else
            {
                TracingService.Trace("Probably there is no email template. Mail not sent.");
            }
        }

        public void SendNotifications_AHP_EXTERNAL_APPLICATION_ON_HOLD(invln_AHPStatusChange ahpStatusChange, invln_scheme ahpApplication)
        {
            TracingService.Trace("AHP_EXTERNAL_APPLICATION_ON_HOLD");
            var contact = _contactRepositoryAdmin.GetById(ahpApplication.invln_contactid.Id, nameof(Contact.FullName).ToLower(), nameof(Contact.EMailAddress1).ToLower());
            if (ahpApplication.invln_programmelookup == null)
            {
                TracingService.Trace("There is no programme on ahpApplication. Mail not sent.");
                return;
            }
            var programme = _programmeRepositoryAdmin.GetById(ahpApplication.invln_programmelookup.Id, invln_programme.Fields.invln_programmename);
            var emailTemplate = _notificationSettingRepositoryAdmin.GetTemplateViaTypeName("AHP_INTERNAL_APPLICATION_ON_HOLD");


            if (contact != null && programme != null && emailTemplate != null)
            {
                var subject = emailTemplate.invln_subject;

                var govNotParams = new AHP_EXTERNAL_APPLICATION_ON_HOLD()
                {
                    templateId = emailTemplate?.invln_templateid,
                    personalisation = new parameters_AHP_EXTERNAL_APPLICATION_ON_HOLD()
                    {
                        recipientEmail = contact.EMailAddress1,
                        subject = subject,
                        name = contact.FullName ?? "NO NAME",
                        programmename = programme.invln_programmename ?? "NO NAME",
                        applicationname = ahpApplication.invln_schemename,
                    }
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parameters = JsonSerializer.Serialize(govNotParams, options);
                this.SendGovNotifyEmail(ahpApplication.OwnerId, ahpApplication.ToEntityReference(), subject, parameters, emailTemplate);
            }
            else
            {
                TracingService.Trace("Probably there is no email template. Mail not sent.");
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
