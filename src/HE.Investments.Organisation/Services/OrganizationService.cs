using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.Services;
public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationServiceAsync2 _service;
    private readonly IOrganisationChangeRequestRepository _organisationChangeRequestRepository;
    private readonly IContactRepository _contactRepository;

    public OrganizationService(
        IOrganizationServiceAsync2 service,
        IOrganisationChangeRequestRepository organisationChangeRequestRepository,
        IContactRepository contactRepository)
    {
        _service = service;
        _organisationChangeRequestRepository = organisationChangeRequestRepository;
        _contactRepository = contactRepository;
    }

    public async Task<Guid> CreateOrganisationChangeRequest(OrganizationDetailsDto organizationDetails, string contactExternalId)
    {
        var organisationChangeRequestToCreate = MapOrganizationDtoToOrganizationChangeRequestEntity(organizationDetails);
        var contact = _contactRepository.GetContactViaExternalId(_service, contactExternalId);
        organisationChangeRequestToCreate["invln_contactid"] = contact?.ToEntityReference();
        return await _service.CreateAsync(organisationChangeRequestToCreate);
    }

    public Guid CreateOrganization(OrganizationDetailsDto organizationDetails)
    {
        var organizationToCreate = MapOrganizationDtoToEntity(organizationDetails);
        return _service.Create(organizationToCreate);
    }

    public async Task<ContactDto?> GetOrganisationChangeDetailsRequestContact(Guid accountId)
    {
        var organisationChangeDetailsRequest = await _organisationChangeRequestRepository.GetChangeRequestForOrganisation(_service, accountId);
        if (organisationChangeDetailsRequest != null)
        {
            var contactReference = (EntityReference)organisationChangeDetailsRequest["invln_contactid"];
            var retrievedContact = _service.Retrieve("contact", contactReference.Id, new ColumnSet("invln_externalid"));
            return new ContactDto()
            {
                contactId = contactReference.Id.ToString(),
                firstName = contactReference.Name,
                contactExternalId = retrievedContact["invln_externalid"].ToString(),
            };
        }

        return null;
    }

    public async Task<OrganizationDetailsDto> GetOrganizationDetails(string accountId, string contactExternalId)
    {
        var organizationDetailsDto = new OrganizationDetailsDto();
        if (Guid.TryParse(accountId, out var organizationId))
        {
            var account = await _service.RetrieveAsync("account", organizationId, new ColumnSet(
                    "name",
                    "he_companieshousenumber",
                    "address1_line1",
                    "address1_line2",
                    "address1_line3",
                    "address1_city",
                    "address1_postalcode",
                    "address1_country",
                    "primarycontactid"));

            organizationDetailsDto.registeredCompanyName = account.Contains("name") ? account["name"].ToString() : null;
            organizationDetailsDto.companyRegistrationNumber = account.Contains("he_companieshousenumber") ? account["he_companieshousenumber"].ToString() : null;
            organizationDetailsDto.addressLine1 = account.Contains("address1_line1") ? account["address1_line1"].ToString() : null;
            organizationDetailsDto.addressLine2 = account.Contains("address1_line2") ? account["address1_line2"].ToString() : null;
            organizationDetailsDto.addressLine3 = account.Contains("address1_line3") ? account["address1_line3"].ToString() : null;
            organizationDetailsDto.city = account.Contains("address1_city") ? account["address1_city"].ToString() : null;
            organizationDetailsDto.postalcode = account.Contains("address1_postalcode") ? account["address1_postalcode"].ToString() : null;
            organizationDetailsDto.country = account.Contains("address1_country") ? account["address1_country"].ToString() : null;

            if (account.Contains("primarycontactid") && account["primarycontactid"] != null)
            {
                var primaryContactReference = (EntityReference)account.Attributes["primarycontactid"];
                var contact = await _service.RetrieveAsync("contact", primaryContactReference.Id, new ColumnSet(
                        "fullname",
                        "emailaddress1",
                        "telephone1"));

                organizationDetailsDto.compayAdminContactName = contact.Contains("fullname") ? contact.Attributes["fullname"].ToString() : null;
                organizationDetailsDto.compayAdminContactEmail = contact.Contains("emailaddress1") ? contact.Attributes["emailaddress1"].ToString() : null;
                organizationDetailsDto.compayAdminContactTelephone = contact.Contains("telephone1") ? contact.Attributes["telephone1"].ToString() : null;
            }
        }

        return organizationDetailsDto;
    }

    private Entity MapOrganizationDtoToEntity(OrganizationDetailsDto organizationDetailsDto)
    {
        var organizationEntity = new Entity("account")
        {
            Attributes = new AttributeCollection()
            {
                { "name", organizationDetailsDto.registeredCompanyName },
                { "he_companieshousenumber", organizationDetailsDto.companyRegistrationNumber },
                { "address1_line1", organizationDetailsDto.addressLine1 },
                { "address1_line2", organizationDetailsDto.addressLine2 },
                { "address1_line3", organizationDetailsDto.addressLine3 },
                { "address1_city", organizationDetailsDto.city },
                { "address1_postalcode", organizationDetailsDto.postalcode },
                { "address1_country", organizationDetailsDto.country },
                { "address1_county", organizationDetailsDto.county },
            },
        };
        return organizationEntity;
    }

    private Entity MapOrganizationDtoToOrganizationChangeRequestEntity(OrganizationDetailsDto organizationDetailsDto)
    {
        var organisationChangeRequestEntity = new Entity("invln_organisationchangerequest")
        {
            Attributes = new AttributeCollection()
            {
                { "invln_registeredcompanyname", organizationDetailsDto.registeredCompanyName },
                { "invln_organisationphonenumber", organizationDetailsDto.organisationPhoneNumber },
                { "invln_addressline1", organizationDetailsDto.addressLine1 },
                { "invln_addressline2", organizationDetailsDto.addressLine2 },
                { "invln_townorcity", organizationDetailsDto.city },
                { "invln_county", organizationDetailsDto.county },
                { "invln_postcode", organizationDetailsDto.postalcode },
                { "invln_organisationid", new EntityReference("account", new Guid(organizationDetailsDto.organisationId)) },
            },
        };
        return organisationChangeRequestEntity;
    }
}
