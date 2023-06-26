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

        #endregion

        #region Constructors

        public ContactService(CrmServiceArgs args) : base(args)
        {
            contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            webRoleRepository = CrmRepositoriesFactory.Get<IWebRoleRepository>();
        }

        #endregion

        #region Public Methods

        public string GetContactRole(string email, string ssid, string portalId)
        {
            if (Guid.TryParse(ssid, out Guid ssidAsGuid))
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
            AssociateRequest associateRequest = new AssociateRequest();
            associateRequest.RelatedEntities = new EntityReferenceCollection();
            associateRequest.RelatedEntities.Add(contact.ToEntityReference());
            associateRequest.Relationship = new Relationship("invln_Contact_Webrole");
            associateRequest.Target = role.ToEntityReference();
            contactRepository.ExecuteAssociateRequest(associateRequest);
        }
    }
}
