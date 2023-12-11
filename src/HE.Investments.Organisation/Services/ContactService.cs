using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

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

    public Task<ContactDto?> RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId)
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
                return Task.FromResult<ContactDto?>(MapContactEntityToDto(retrievedContact));
            }
        }

        return Task.FromResult<ContactDto?>(null);
    }

    public async Task UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(contactExternalId))
        {
            var retrievedContact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
            if (retrievedContact != null)
            {
                var contactToUpdate = MapContactDtoToEntity(contactDto);
                contactToUpdate.Id = retrievedContact.Id;
                await service.UpdateAsync(contactToUpdate, cancellationToken);
            }
        }
    }

    public Task<ContactRolesDto?> GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string portalType, string contactExternalId)
    {
        var contact = _contactRepository.GetContactWithGivenEmailAndExternalId(service, contactEmail, contactExternalId);
        if (contact != null)
        {
            var contactWebRole = _webRoleRepository.GetContactWebrole(service, contact.Id, portalType);
            if (contactWebRole.Count == 0)
            {
                return Task.FromResult<ContactRolesDto?>(null);
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
                string webRoleName = webroleReference?.Name ?? (contactRole.Contains("ae.invln_name") ? ((dynamic)contactRole["ae.invln_name"]).Value : string.Empty);
                roles.Add(new ContactRoleDto()
                {
                    accountId = contactRole.Contains("invln_accountid") && contactRole["invln_accountid"] != null ? ((EntityReference)contactRole["invln_accountid"]).Id : Guid.Empty,
                    accountName = contactRole.Contains("invln_accountid") && contactRole["invln_accountid"] != null ? ((EntityReference)contactRole["invln_accountid"]).Name : null,
                    permissionLevel = permissionLevel != null && permissionLevel.Contains("invln_permission") && permissionLevel["invln_permission"] != null ? ((OptionSetValue)permissionLevel["invln_permission"]).Value.ToString(CultureInfo.InvariantCulture) : null,
                    webRoleName = webRoleName,
                });
            }

            var contactRolesDto = new ContactRolesDto()
            {
                email = contactEmail,
                externalId = contactExternalId,
                contactRoles = roles,
            };

            return Task.FromResult<ContactRolesDto?>(contactRolesDto);
        }

        return Task.FromResult<ContactRolesDto?>(null);
    }

    public async Task<Guid> LinkContactWithOrganization(IOrganizationServiceAsync2 service, string contactExternalId, Guid organisationGuid, int portalType)
    {
        var contact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
        var defaultRole = _webRoleRepository.GetDefaultPortalRoles(service, portalType);
        if (contact != null)
        {
            var contactWebroleExists = _webRoleRepository.GetContactWebroleForOrganisation(service, contact.Id, organisationGuid) != null;
            if (!contactWebroleExists)
            {
                var contactWebroleToCreate = new Entity("invln_contactwebrole")
                {
                    Attributes =
            {
                { "invln_accountid", new EntityReference("account", organisationGuid) },
                { "invln_contactid", contact?.ToEntityReference() },
                { "invln_webroleid", defaultRole.First().ToEntityReference() },
            },
                };
                return await service.CreateAsync(contactWebroleToCreate);
            }

            throw new InvalidPluginExecutionException("Webrole for given contact and organisation already exists");
        }

        throw new InvalidPluginExecutionException("Contact with given external id not found in CRM");
    }

    public async Task RemoveLinkBetweenContactAndOrganisation(IOrganizationServiceAsync2 service, Guid organisationGuid, string contactExternalId, int? portalType = null)
    {
        var contact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
        if (contact != null)
        {
            var portalTypeFilter = GeneratePortalTypeFilter(portalType);
            var contactWebrole = _webRoleRepository.GetContactWebroleForGivenOrganisationAndPortal(service, organisationGuid, contact.Id, portalTypeFilter);
            if (contactWebrole != null)
            {
                await service.DeleteAsync("invln_contactwebrole", contactWebrole.Id);
            }
        }
    }

    public Task<List<ContactDto>> GetAllOrganisationContactsForPortal(IOrganizationServiceAsync2 service, Guid organisationGuid, int? portalType = null)
    {
        var contactList = new List<ContactDto>();
        var portalTypeFilter = GeneratePortalTypeFilter(portalType);
        var contacts = _contactRepository.GetContactsForOrganisation(service, organisationGuid, portalTypeFilter);
        if (contacts.Any())
        {
            foreach (var contact in contacts)
            {
                contactList.Add(MapContactEntityToDto(contact));
            }
        }

        return Task.FromResult(contactList);
    }

    public async Task UpdateContactWebrole(IOrganizationServiceAsync2 service, string contactExternalId, Guid organisationGuid, int newWebRole)
    {
        var contact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
        if (contact != null)
        {
            var currentRoleName = _webRoleRepository.GetContactWebroleForOrganisation(service, contact.Id, organisationGuid);
            if (currentRoleName != null)
            {
                var webrole = _webRoleRepository.GetWebroleByPermissionOptionSetValue(service, newWebRole);
                if (webrole != null)
                {
                    var contactWebroleToUpdate = new Entity("invln_contactwebrole")
                    {
                        Id = currentRoleName.Id,
                        Attributes =
                            {
                                { "invln_webroleid", webrole.ToEntityReference() },
                            },
                    };

                    await service.UpdateAsync(contactWebroleToUpdate);
                }
            }
        }
    }

    public Task<List<ContactRolesDto>> GetContactRolesForOrganisationContacts(IOrganizationServiceAsync2 service, List<string> contactExternalId, Guid organisationGuid)
    {
        var contactRolesList = new List<ContactRolesDto>();
        if (contactExternalId.Any())
        {
            var contactExternalFilter = "<condition attribute=\"invln_externalid\" operator=\"in\">";
            foreach (var contactExternal in contactExternalId)
            {
                contactExternalFilter += $"<value>{contactExternal}</value>";
            }
            contactExternalFilter += "</condition>";

            var contactWebroles = _webRoleRepository.GetWebrolesForPassedContacts(service, contactExternalFilter, organisationGuid);
            if (contactWebroles.Any())
            {
                foreach (var contactWebrole in contactWebroles)
                {
                    contactRolesList.Add(new ContactRolesDto()
                    {
                        contactRoles = new List<ContactRoleDto>()
                        {
                            new ContactRoleDto()
                            {
                                accountId = contactWebrole.Contains("invln_accountid") && contactWebrole["invln_accountid"] != null ? ((EntityReference)contactWebrole["invln_accountid"]).Id : Guid.Empty,
                                accountName = contactWebrole.Contains("acc.name") && contactWebrole["acc.name"] != null ? contactWebrole.GetAttributeValue<AliasedValue>("acc.name").Value.ToString() : string.Empty,
                                permissionLevel = contactWebrole.Contains("ppl.invln_name") && contactWebrole["ppl.invln_name"] != null ? contactWebrole.GetAttributeValue<AliasedValue>("ppl.invln_name").Value.ToString() : null,
                                webRoleName = contactWebrole.Contains("wr.invln_name") ? contactWebrole.GetAttributeValue<AliasedValue>("wr.invln_name").Value.ToString() : string.Empty,
                            },
                        },
                        externalId = contactWebrole.Contains("cnt.invln_externalid") && contactWebrole["cnt.invln_externalid"] != null ? contactWebrole.GetAttributeValue<AliasedValue>("cnt.invln_externalid").Value.ToString() : string.Empty,
                    });
                }
            }
        }

        return Task.FromResult(contactRolesList);
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
            contactExternalId = contact.Contains("invln_externalid") ? contact["invln_externalid"].ToString() : string.Empty,
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

    private string GeneratePortalTypeFilter(int? portalType)
    {
        if (portalType != null)
        {
            return @"<filter>
                        <condition attribute=""invln_portal"" operator=""eq"" value=""" + portalType + @""" />
                   </filter>";
        }

        return string.Empty;
    }
}
