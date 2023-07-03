using DataverseModel;
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

        private readonly IContactRepository contactRepository;
        private readonly IWebRoleRepository webRoleRepository;
        private readonly IPortalPermissionRepository portalPermissionRepository;
        private readonly IContactWebroleRepository contactWebroleRepository;

        #endregion

        #region Constructors

        public ContactService(CrmServiceArgs args) : base(args)
        {
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
                this.TracingService.Trace("Contact webroles: " + contactWebRole.Count);
                if (contactWebRole.Count == 0)
                {
                    //var defaultRole = webRoleRepository.GetDefaultPortalRole(portalType);
                    //if (defaultRole != null)
                    //{
                    //    AssignRoleToContact(contact, defaultRole); //trzeba przypisać default role dla konta
                    //    return defaultRole.invln_Name;
                    //}
                    //else
                    //{
                        return null;
                    //}
                }

                List<ContactRoleDto> roles = new List<ContactRoleDto>();
                var portalPermissionLevels = portalPermissionRepository.RetrieveAll();

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
                        accountId = contactRole.invln_Accountid.Id,
                        accountName = contactRole.invln_Accountid.Name,
                        permissionLevel = permissionLevel != null ? permissionLevel.invln_Permission.Value.ToString() : null,
                        webRoleName = contactRole.invln_Webroleid.Name,
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
