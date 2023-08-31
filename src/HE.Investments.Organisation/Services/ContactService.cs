using System.Linq;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.Services;
public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IWebRoleRepository _webRoleRepository;
    private readonly IPortalPermissionRepository _permissionRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public ContactService(IContactRepository contactRepository, IWebRoleRepository webRoleRepository, IPortalPermissionRepository permissionRepository, IOrganizationRepository organizationRepository)
    {
        _contactRepository = contactRepository;
        _webRoleRepository = webRoleRepository;
        _permissionRepository = permissionRepository;
        _organizationRepository = organizationRepository;
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

    public ContactRolesDto? GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string portalType, string contactExternalId)
    {
        var contact = _contactRepository.GetContactWithGivenEmailAndExternalId(service, contactEmail, contactExternalId);
        if (contact != null)
        {
            var contactWebRole = _webRoleRepository.GetContactWebrole(service, contact.Id, portalType);
            if (contactWebRole.Count == 0)
            {
                return null;
            }

            var roles = new List<ContactRoleDto>();
            var portalPermissionLevels = _permissionRepository.RetrieveAll(service);
            foreach (var contactRole in contactWebRole)
            {
                Entity? permissionLevel = null;
                if (contactRole.Contains("ae.invln_portalpermissionlevelid") && contactRole["ae.invln_portalpermissionlevelid"] != null && ((dynamic)contactRole["ae.invln_portalpermissionlevelid"]).Value != null)
                {
                    permissionLevel = portalPermissionLevels.Where(x => (dynamic)x["invln_portalpermissionlevelid"] == ((dynamic)contactRole["ae.invln_portalpermissionlevelid"]).Value.Id).ToList().FirstOrDefault();
                }

                var webroleReference = (EntityReference)contactRole["invln_webroleid"];
                string webRoleName = webroleReference?.Name ?? (contactRole.Contains("ae.invln_name") ? ((dynamic)contactRole["ae.invln_name"]).Value : null);
                roles.Add(new ContactRoleDto()
                {
                    accountId = contactRole.Contains("invln_accountid") && contactRole["invln_accountid"] != null ? ((EntityReference)contactRole["invln_accountid"]).Id : Guid.Empty,
                    accountName = contactRole.Contains("invln_accountid") && contactRole["invln_accountid"] != null ? ((EntityReference)contactRole["invln_accountid"]).Name : null,
                    permissionLevel = permissionLevel != null && permissionLevel.Contains("invln_permission") && permissionLevel["invln_permission"] != null ? ((OptionSetValue)permissionLevel["invln_permission"]).Value.ToString() : null,
                    webRoleName = webRoleName,
                });
            }

            var contactRolesDto = new ContactRolesDto()
            {
                email = contactEmail,
                externalId = contactExternalId,
                contactRoles = roles,
            };

            return contactRolesDto;
        }

        return null;
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
