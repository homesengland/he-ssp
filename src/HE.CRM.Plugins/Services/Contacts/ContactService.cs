﻿using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HE.CRM.Plugins.Services.Contacts
{
    public class ContactService : CrmService, IContactService
    {
        #region Fields
        private readonly IAccountRepository accountRepository;
        private readonly IContactRepository contactRepository;
        private readonly IWebRoleRepository webRoleRepository;
        private readonly IPortalPermissionRepository portalPermissionRepository;
        private readonly IContactWebroleRepository contactWebroleRepository;

        #endregion

        #region Constructors

        public ContactService(CrmServiceArgs args) : base(args)
        {
            accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            webRoleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();
            contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
            portalPermissionRepository = CrmRepositoriesFactory.Get<IPortalPermissionRepository>();
        }

        #endregion

        #region Public Methods

        public ContactRolesDto GetContactRoles(string contactEmail, string contactExternalId, string portalType)
        {
            this.TracingService.Trace("Get contact for externalid and email");
            var contact = contactRepository.GetContactWithGivenEmailAndExternalId(contactEmail, contactExternalId);
            if (contact != null)
            {
                var contactWebRole = webRoleRepository.GetContactWebRole(contact.Id, portalType);
                List<ContactRoleDto> roles = new List<ContactRoleDto>();
                var portalPermissionLevels = portalPermissionRepository.RetrieveAll();
                this.TracingService.Trace("Contact webroles: " + contactWebRole.Count);
                if (contactWebRole.Count == 0)
                {
                    // TODO: Should return null when no role
                    var defaultRoles = webRoleRepository.GetDefaultPortalRoles(portalType);
                    var defaultRole = defaultRoles.Count > 0 ? defaultRoles[0] : null;
                    var defaultAccount = accountRepository.GetDefaultAccount();

                    this.TracingService.Trace("Create new contactwebrole");
                    contactWebroleRepository.Create(new invln_contactwebrole()
                    {
                        invln_Accountid = defaultAccount != null ? defaultAccount.ToEntityReference() : null,
                        invln_Webroleid = defaultRole != null ? defaultRole.ToEntityReference() : null,
                        invln_Contactid = contact.ToEntityReference()
                    });

                    string permissionLevelValue = null;
                    if(defaultRole != null && defaultRole.invln_Portalpermissionlevelid != null)
                    {
                        this.TracingService.Trace("Default role permissions: " + defaultRole.invln_Portalpermissionlevelid.Id);
                        var ppLevel = portalPermissionLevels.Where(x => x.invln_portalpermissionlevelId == defaultRole.invln_Portalpermissionlevelid.Id).FirstOrDefault();
                        permissionLevelValue = ppLevel.invln_Permission != null ? ppLevel.invln_Permission.Value.ToString() : null;
                    }

                    this.TracingService.Trace("Add ContactRoleDto");
                    roles.Add(new ContactRoleDto()
                    {
                        accountId = defaultAccount != null ? defaultAccount.Id : Guid.Empty,
                        accountName = defaultAccount != null ? defaultAccount.Name : "account_not_set_CRM",
                        permissionLevel = permissionLevelValue != null ? permissionLevelValue : "permission_level_not_set_CRM",
                        webRoleName = defaultRole != null && defaultRole.invln_Name != null ? defaultRole.invln_Name : "default_role_not_set_CRM",
                    });
                }

                this.TracingService.Trace("Going through roles...");
                foreach (var contactRole in contactWebRole)
                {
                    invln_portalpermissionlevel permissionLevel = null;
                    if (contactRole["ae.invln_portalpermissionlevelid"] != null && ((dynamic)contactRole["ae.invln_portalpermissionlevelid"]).Value != null)
                    {
                        this.TracingService.Trace("PermissionLevel lookup exists");
                        permissionLevel = portalPermissionLevels.Where(x => x.invln_portalpermissionlevelId == ((dynamic)contactRole["ae.invln_portalpermissionlevelid"]).Value.Id).ToList().FirstOrDefault();
                    }

                    this.TracingService.Trace("Add role");
                    roles.Add(new ContactRoleDto()
                    {
                        accountId = contactRole.invln_Accountid != null ? contactRole.invln_Accountid.Id : Guid.Empty,
                        accountName = contactRole.invln_Accountid != null ? contactRole.invln_Accountid.Name : null,
                        permissionLevel = permissionLevel != null && permissionLevel.invln_Permission != null ? permissionLevel.invln_Permission.Value.ToString() : null,
                        webRoleName = contactRole.invln_Webroleid != null ? contactRole.invln_Webroleid.Name : null,
                    });
                }

                this.TracingService.Trace("Creating ContactRolesDto");
                ContactRolesDto contactRolesDto = new ContactRolesDto()
                {
                    email = contactEmail,
                    externalId = contactExternalId,
                    contactRoles = roles,
                };

                return contactRolesDto;
            }

            return null;
        }

        #endregion

        private void AssignRoleToContact(Contact contact, invln_Webrole role)
        {
            var contactRoleToCreate = new invln_contactwebrole()
            {
                invln_Contactid = contact.ToEntityReference(),
                invln_Webroleid = role.ToEntityReference(),
            };
            contactWebroleRepository.Create(contactRoleToCreate);
        }
    }
}
