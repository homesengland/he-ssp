using System.Globalization;
using System.Text;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.FeatureManagement;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.Services;
public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IWebRoleRepository _webRoleRepository;
    private readonly IPortalPermissionRepository _permissionRepository;
    private readonly IFeatureManager _featureManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly int _commonPortalTypeOption = 858110002;

    public ContactService(
        IContactRepository contactRepository,
        IWebRoleRepository webRoleRepository,
        IPortalPermissionRepository permissionRepository,
        IFeatureManager featureManager,
        IDateTimeProvider dateTimeProvider)
    {
        _contactRepository = contactRepository;
        _webRoleRepository = webRoleRepository;
        _permissionRepository = permissionRepository;
        _featureManager = featureManager;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<ContactDto?> RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId)
    {
        if (!string.IsNullOrEmpty(contactExternalId))
        {
            string[] fields =
            [
                "firstname", "lastname", "emailaddress1", "telephone1", "invln_externalid", "jobtitle", "address1_city",
                "address1_county", "address1_postalcode", "address1_country", "mobilephone", "invln_termsandconditionsaccepted"
            ];
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

    public async Task<ContactRolesDto?> GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string contactExternalId, int? portalType = null)
    {
        var contact = _contactRepository.GetContactWithGivenEmailOrExternalId(service, contactEmail, contactExternalId);
        if (contact != null)
        {
            await ConnectingNotConnectedContactWithExternalId(service, contact, contactExternalId);
            var portalTypeFilter = GeneratePortalTypeFilter(portalType);
            var contactWebRole = _webRoleRepository.GetContactWebRole(service, contact.Id.ToString(), portalTypeFilter);
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
                    permissionLevel = portalPermissionLevels.Where(x => (dynamic)x["invln_portalpermissionlevelid"] == ((dynamic)contactRole["ae.invln_portalpermissionlevelid"]).Value.Id).AsEnumerable().FirstOrDefault();
                }

                var webRoleReference = (EntityReference)contactRole["invln_webroleid"];
                string webRoleName = webRoleReference?.Name ?? (contactRole.Contains("ae.invln_name") ? ((dynamic)contactRole["ae.invln_name"]).Value : string.Empty);
                var permission = permissionLevel != null && permissionLevel.Contains("invln_permission") && permissionLevel["invln_permission"] != null ? ((OptionSetValue)permissionLevel["invln_permission"])?.Value : null;
                roles.Add(new ContactRoleDto()
                {
                    accountId = contactRole.Contains("invln_accountid") && contactRole["invln_accountid"] != null ? ((EntityReference)contactRole["invln_accountid"]).Id : Guid.Empty,
                    accountName = contactRole.Contains("invln_accountid") && contactRole["invln_accountid"] != null ? ((EntityReference)contactRole["invln_accountid"]).Name : null,
                    permissionLevel = permission?.ToString(CultureInfo.InvariantCulture),
                    webRoleName = webRoleName,
                    permission = permission,
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

    public async Task<string> LinkContactWithOrganization(IOrganizationServiceAsync2 service, string contactExternalId, string organisationId, int portalType)
    {
        var contact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
        var defaultRole = _webRoleRepository.GetDefaultPortalRoles(service, portalType);
        if (contact != null)
        {
            var contactWebRoleExists = _webRoleRepository.GetContactWebRoleForOrganisation(service, contact.Id.ToString(), organisationId.TryToGuidAsString()) != null;
            if (!contactWebRoleExists)
            {
                var contactWebRoleToCreate = new Entity("invln_contactwebrole")
                {
                    Attributes =
            {
                { "invln_accountid", new EntityReference("account", new Guid(organisationId.TryToGuidAsString())) },
                { "invln_contactid", contact.ToEntityReference() },
                { "invln_webroleid", defaultRole[0].ToEntityReference() },
            },
                };
                contactWebRoleToCreate.Id = await service.CreateAsync(contactWebRoleToCreate);
                var req = new OrganizationRequest("invln_sendrequesttoassigncontacttoexistingorganisation")
                {
                    ["invln_organisationid"] = organisationId.TryToGuidAsString(),
                    ["invln_contactid"] = contact.Id.ToString(),
                };
                service.Execute(req);
                return contactWebRoleToCreate.Id.ToString();
            }

            throw new InvalidPluginExecutionException("Webrole for given contact and organisation already exists");
        }

        throw new InvalidPluginExecutionException("Contact with given external id not found in CRM");
    }

    public async Task RemoveLinkBetweenContactAndOrganisation(IOrganizationServiceAsync2 service, string organisationId, string contactExternalId, int? portalType = null)
    {
        var contact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
        if (contact != null)
        {
            var portalTypeFilter = GeneratePortalTypeFilter(portalType);
            var contactWebRole = _webRoleRepository.GetContactWebRoleForGivenOrganisationAndPortal(service, organisationId.TryToGuidAsString(), contact.Id.ToString(), portalTypeFilter);
            if (contactWebRole != null)
            {
                await service.DeleteAsync("invln_contactwebrole", contactWebRole.Id);
            }
        }
    }

    public Task<List<ContactDto>> GetAllOrganisationContactsForPortal(IOrganizationServiceAsync2 service, string organisationId, int? portalType = null)
    {
        var contactList = new List<ContactDto>();
        var portalTypeFilter = GeneratePortalTypeFilter(portalType);
        var contacts = _contactRepository.GetContactsForOrganisation(service, organisationId.TryToGuidAsString(), portalTypeFilter);
        foreach (var contact in contacts)
        {
            contactList.Add(MapContactEntityToDto(contact));
        }

        return Task.FromResult(contactList);
    }

    public async Task UpdateContactWebRole(IOrganizationServiceAsync2 service, string contactExternalId, string contactAssigningExternalId, string organisationId, int newWebRole, int? portalType = null)
    {
        var contact = _contactRepository.GetContactViaExternalId(service, contactExternalId);
        if (contact != null)
        {
            var currentRoleName = _webRoleRepository.GetContactWebRoleForOrganisation(service, contact.Id.ToString(), organisationId.TryToGuidAsString());
            if (currentRoleName != null)
            {
                var portalTypeFilter = GeneratePortalTypeFilter(portalType);
                var webRole = _webRoleRepository.GetWebRoleByPermissionOptionSetValue(service, newWebRole, portalTypeFilter);
                if (webRole != null)
                {
                    var attributes = new AttributeCollection
                    {
                         { "invln_webroleid", webRole.ToEntityReference() },
                    };

                    if (await _featureManager.IsEnabledAsync(FeatureFlags.WebRoleAuditFieldsImplemented))
                    {
                        var requesterContact = _contactRepository.GetContactViaExternalId(service, contactAssigningExternalId);
                        if (requesterContact != null)
                        {
                            attributes.AddRange(
                                new KeyValuePair<string, object>("invln_contactassigningwebrole", requesterContact.ToEntityReference()),
                                new KeyValuePair<string, object>("invln_datetimeofassigningwebrole", _dateTimeProvider.UtcNow));
                        }
                    }

                    var contactWebRoleToUpdate = new Entity("invln_contactwebrole")
                    {
                        Id = currentRoleName.Id,
                        Attributes = attributes,
                    };

                    await service.UpdateAsync(contactWebRoleToUpdate);
                }
            }
        }
    }

    public Task<List<ContactRolesDto>> GetContactRolesForOrganisationContacts(IOrganizationServiceAsync2 service, List<string> contactExternalId, string organisationId)
    {
        var contactExternalFilter = new StringBuilder("<condition attribute=\"invln_externalid\" operator=\"in\">");
        foreach (var contactExternal in contactExternalId)
        {
            contactExternalFilter.Append(CultureInfo.InvariantCulture, $"<value>{contactExternal}</value>");
        }

        _ = contactExternalFilter.Append("</condition>");
        var contactWebRoles = _webRoleRepository.GetWebRolesForPassedContacts(service, contactExternalFilter.ToString(), organisationId.TryToGuidAsString());
        return Task.FromResult(GenerateContactRolesList(contactWebRoles));
    }

    public async Task<string> CreateNotConnectedContact(IOrganizationServiceAsync2 service, ContactDto contact, string organisationId, int role, string inviterExternalId, int? portalType = null)
    {
        var inviter = _contactRepository.GetContactViaExternalId(service, inviterExternalId) ?? throw new InvalidPluginExecutionException("Inviter with given external ID does not exists");
        var invitedContact = _contactRepository.GetContactWithGivenEmail(service, contact.email);
        if (invitedContact == null)
        {
            var contactToCreate = MapContactDtoToEntity(contact);
            var contactGuid = Guid.NewGuid();
            contactToCreate.Id = contactGuid;
            contactToCreate["invln_externalid"] = $"_{contactGuid}";
            _ = await service.CreateAsync(contactToCreate);
            invitedContact = contactToCreate;
        }

        var portalTypeFilter = GeneratePortalTypeFilter(portalType);
        var webrole = _webRoleRepository.GetWebRoleByPermissionOptionSetValue(service, role, portalTypeFilter) ?? throw new InvalidPluginExecutionException("Given webrole does not exists");
        var organisationEntityReference = new EntityReference("account", new Guid(organisationId.TryToGuidAsString()));

        var contactWebroleToCreate = new Entity("invln_contactwebrole")
        {
            Attributes =
            {
                ["invln_accountid"] = organisationEntityReference,
                ["invln_contactid"] = invitedContact.ToEntityReference(),
                ["invln_webroleid"] = webrole.ToEntityReference(),
            },
        };
        _ = await service.CreateAsync(contactWebroleToCreate);

        var req = new OrganizationRequest("invln_invitecontacttojoinexistingorganisation")
        {
            ["invln_invitedcontactid"] = invitedContact.Id.ToString(),
            ["invln_organisationid"] = organisationId.TryToGuidAsString(),
            ["invln_invitercontactid"] = inviter.Id.ToString(),
        };
        service.Execute(req);
        return invitedContact.Id.ToString();
    }

    private async Task ConnectingNotConnectedContactWithExternalId(IOrganizationServiceAsync2 service, Entity contact, string contactExternalId)
    {
        if (contact.Contains("invln_externalid") && contact["invln_externalid"] != null
            && ((string)contact["invln_externalid"]).StartsWith('_'))
        {
            var contactToUpdate = new Entity("contact")
            {
                Id = contact.Id,
                Attributes =
                {
                    ["invln_externalid"] = contactExternalId,
                },
            };
            await service.UpdateAsync(contactToUpdate);
        }
    }

    private List<ContactRolesDto> GenerateContactRolesList(List<Entity> contactWebroles)
    {
        var contactRolesList = new List<ContactRolesDto>();
        foreach (var contactWebrole in contactWebroles)
        {
            contactRolesList.Add(new ContactRolesDto()
            {
                contactRoles =
                [
                    new()
                    {
                        accountId =
                            contactWebrole.Contains("invln_accountid") && contactWebrole["invln_accountid"] != null
                                ? ((EntityReference)contactWebrole["invln_accountid"]).Id
                                : Guid.Empty,
                        accountName = GetAliasedValueAsStringOrDefault(contactWebrole, "acc.name"),
                        permissionLevel = GetAliasedValueAsStringOrDefault(contactWebrole, "ppl.invln_name"),
                        webRoleName = GetAliasedValueAsStringOrDefault(contactWebrole, "wr.invln_name"),
                        permission = (GetAliasedValueAsObjectOrDefault(contactWebrole, "ppl.invln_permission") as OptionSetValue)?.Value,
                    }

                ],
                externalId = GetAliasedValueAsStringOrDefault(contactWebrole, "cnt.invln_externalid"),
            });
        }

        return contactRolesList;
    }

    private string? GetAliasedValueAsStringOrDefault(Entity entity, string attributeName)
    {
        return entity.Contains(attributeName) && entity[attributeName] != null ? entity.GetAttributeValue<AliasedValue>(attributeName).Value.ToString() : string.Empty;
    }

    private object? GetAliasedValueAsObjectOrDefault(Entity entity, string attributeName)
    {
        return entity.Contains(attributeName) && entity[attributeName] != null ? entity.GetAttributeValue<AliasedValue>(attributeName).Value : null;
    }

    private ContactDto MapContactEntityToDto(Entity contact)
    {
        var contactDto = new ContactDto()
        {
            firstName = contact.Contains("firstname") ? contact["firstname"].ToString() : string.Empty,
            lastName = contact.Contains("lastname") ? contact["lastname"].ToString() : string.Empty,
            email = contact.Contains("emailaddress1") ? contact["emailaddress1"].ToString() : string.Empty,
            phoneNumber = contact.Contains("telephone1") ? contact["telephone1"].ToString() : string.Empty,
            secondaryPhoneNumber = contact.Contains("mobilephone") ? contact["mobilephone"].ToString() : string.Empty,
            jobTitle = contact.Contains("jobtitle") ? contact["jobtitle"].ToString() : string.Empty,
            city = contact.Contains("address1_city") ? contact["address1_city"].ToString() : string.Empty,
            county = contact.Contains("address1_county") ? contact["address1_county"].ToString() : string.Empty,
            postcode = contact.Contains("address1_postalcode") ? contact["address1_postalcode"].ToString() : string.Empty,
            country = contact.Contains("address1_country") ? contact["address1_country"].ToString() : string.Empty,
            contactId = contact.Id.ToString(),
            contactExternalId = contact.Contains("invln_externalid") ? contact["invln_externalid"].ToString() : string.Empty,
            webrole = (GetAliasedValueAsObjectOrDefault(contact, "ppl.invln_permission") as OptionSetValue)?.Value,
        };

        if (contact.Contains("invln_termsandconditionsaccepted") && bool.TryParse(contact["invln_termsandconditionsaccepted"].ToString(), out var termsAccepted))
        {
            contactDto.isTermsAndConditionsAccepted = termsAccepted;
        }

        return contactDto;
    }

    private Entity MapContactDtoToEntity(ContactDto contactDto)
    {
        var entity = new Entity("contact")
        {
            ["firstname"] = contactDto.firstName,
            ["lastname"] = contactDto.lastName,
            ["emailaddress1"] = contactDto.email,
            ["telephone1"] = contactDto.phoneNumber,
            ["mobilephone"] = contactDto.secondaryPhoneNumber,
            ["jobtitle"] = contactDto.jobTitle,
            ["address1_city"] = contactDto.city,
            ["address1_county"] = contactDto.county,
            ["address1_postalcode"] = contactDto.postcode,
            ["address1_country"] = contactDto.country,
            ["invln_termsandconditionsaccepted"] = contactDto.isTermsAndConditionsAccepted,
        };

        if (Guid.TryParse(contactDto.contactId, out var recordId))
        {
            entity.Id = recordId;
        }

        return entity;
    }

    private string GeneratePortalTypeFilter(int? portalType)
    {
        portalType ??= _commonPortalTypeOption;
        return @"<filter>
                        <condition attribute=""invln_portal"" operator=""eq"" value=""" + portalType + @""" />
                   </filter>";
    }
}
