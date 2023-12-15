using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Messages;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IContactRepository : ICrmEntityRepository<Contact, DataverseContext>
    {
        Contact GetFirstContactWithGivenEmail(string email);
        List<Contact> GetAllContactsWithGivenEmail(string email);
        Contact GetContactWithGivenEmailAndExternalId(string contactEmail, string contactExternalId);
        Contact GetContactViaExternalId(string contactExternalId, string[] columnSet = null);
        AssociateResponse ExecuteAssociateRequest(AssociateRequest request);
        List<Contact> GetContactsForOrganisation(Guid organisationId);
        List<Contact> GetOrganisationAdministrators(Guid organisationId);
    }
}
