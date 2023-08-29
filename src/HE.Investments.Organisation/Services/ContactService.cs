using HE.Common.IntegrationModel.PortalIntegrationModel;
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

    public ContactDto? RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId)
    {
        if (!string.IsNullOrEmpty(contactExternalId))
        {
            string[] fields =
            {
                "firstname", "lastname", "emailaddress1", "address1_telephone1", "invln_externalid", "jobtitle", "address1_city",
                "address1_county", "address1_postalcode", "address1_country", "address1_telephone2", "invln_termsandconditionsaccepted",
            };
            var retrievedContact = _contactRepository.GetContactViaExternalId(service, contactExternalId, fields);
            if (retrievedContact != null)
            {
                return MapContactEntityToDto(retrievedContact);
            }
        }

        return null;
    }

    public void UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto)
    {
        if (!string.IsNullOrEmpty(contactExternalId))
        {
            var retrievedContact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
            if (retrievedContact != null)
            {
                var contactToUpdate = MapContactDtoToEntity(contactDto);
                contactToUpdate.Id = retrievedContact.Id;
                service.Update(contactToUpdate);
            }
        }
    }

    private ContactDto MapContactEntityToDto(Entity contact)
    {
        var contactDto = new ContactDto()
        {
            firstName = contact.Contains("firstname") ? contact["firstname"].ToString() : string.Empty,
            lastName = contact.Contains("lastname") ? contact["lastname"].ToString() : string.Empty,
            email = contact.Contains("emailaddress1") ? contact["emailaddress1"].ToString() : string.Empty,
            phoneNumber = contact.Contains("address1_telephone1") ? contact["address1_telephone1"].ToString() : string.Empty,
            secondaryPhoneNumber = contact.Contains("address1_telephone2") ? contact["address1_telephone2"].ToString() : string.Empty,
            jobTitle = contact.Contains("jobtitle") ? contact["jobtitle"].ToString() : string.Empty,
            city = contact.Contains("address1_city") ? contact["address1_city"].ToString() : string.Empty,
            county = contact.Contains("address1_county") ? contact["address1_county"].ToString() : string.Empty,
            postcode = contact.Contains("address1_postalcode") ? contact["address1_postalcode"].ToString() : string.Empty,
            country = contact.Contains("address1_country") ? contact["address1_country"].ToString() : string.Empty,
            contactId = contact.Id.ToString(),
        };

        if (contact.Contains("invln_termsandconditionsaccepted") && bool.TryParse(contact["invln_termsandconditionsaccepted"].ToString(), out var termsAccepted))
        {
            contactDto.isTermsAndConditionsAccepted = termsAccepted;
        }

        return contactDto;
    }

    private Entity MapContactDtoToEntity(ContactDto contactDto)
    {
        var entity = new Entity("contact");
        entity["firstname"] = contactDto.firstName;
        entity["lastname"] = contactDto.lastName;
        entity["emailaddress1"] = contactDto.email;
        entity["address1_telephone1"] = contactDto.phoneNumber;
        entity["address1_telephone2"] = contactDto.secondaryPhoneNumber;
        entity["jobtitle"] = contactDto.jobTitle;
        entity["address1_city"] = contactDto.city;
        entity["address1_county"] = contactDto.county;
        entity["address1_postalcode"] = contactDto.postcode;
        entity["address1_country"] = contactDto.country;
        entity["invln_termsandconditionsaccepted"] = contactDto.isTermsAndConditionsAccepted;

        if (Guid.TryParse(contactDto.contactId, out var recordId))
        {
            entity.Id = recordId;
        }

        return entity;
    }
}
