using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
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
        private readonly IContactWebroleRepository contactWebroleRepository;

        #endregion

        #region Constructors

        public ContactService(CrmServiceArgs args) : base(args)
        {
            contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            webRoleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();
            contactWebroleRepository = CrmRepositoriesFactory.Get<IContactWebroleRepository>();
        }

        #endregion

        #region Public Methods

        public string GetContactRole(string email, string ssid, string portal)
        {
            if (Guid.TryParse(ssid, out Guid ssidAsGuid) && Guid.TryParse(portal, out Guid portalId))
            {
                var contact = contactRepository.GetContactWithGivenEmailAndSSID(email, ssidAsGuid);
                if(contact != null)
                {
                    var contactRole = webRoleRepository.GetContactRole(contact.Id, portalId);
                    if(contactRole != null)
                    {
                        return contactRole.invln_Name;
                    }
                    else
                    {
                        var defaultRole = webRoleRepository.GetRoleByName("Default role");
                        if(defaultRole != null)
                        {
                            AssignRoleToContact(contact, defaultRole); //trzeba przypisać default role dla konta
                            return defaultRole.invln_Name;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
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
