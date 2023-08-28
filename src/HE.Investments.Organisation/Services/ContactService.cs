using HE.Investments.Organisation.CrmRepository;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.Services;
public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public Entity RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId)
    {
        if (!string.IsNullOrEmpty(contactExternalId))
        {
            string[] fields = { nameof(Contact.FirstName).ToLower(), nameof(Contact.LastName).ToLower(), nameof(Contact.EMailAddress1).ToLower(),
                    nameof(Contact.Address1_Telephone1).ToLower(), nameof(Contact.invln_externalid).ToLower(), nameof(Contact.JobTitle).ToLower(), nameof(Contact.Address1_City).ToLower(),
                    nameof(Contact.Address1_County).ToLower(), nameof(Contact.Address1_PostalCode).ToLower(), nameof(Contact.Address1_Country).ToLower(), nameof(Contact.Address1_Telephone2).ToLower(),
                    nameof(Contact.invln_termsandconditionsaccepted).ToLower(), };
            var retrievedContact = _contactRepository.GetContactViaExternalId(service, contactExternalId, fields);
        }
    }

    public void UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, string serializedContact)
    {
        if (!string.IsNullOrEmpty(serializedContact) && !string.IsNullOrEmpty(contactExternalId))
        {
            var retrievedContact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
            if (retrievedContact != null)
            {
                var deserializedContact = JsonSerializer.Deserialize<ContactDto>(serializedContact);
                var contactToUpdate = ContactDtoMapper.MapContactDtoToRegularEntitry(deserializedContact);
                contactToUpdate.Id = retrievedContact.Id;
                contactRepository.Update(contactToUpdate);
            }
        }
    }
}
