using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using HE.Investments.Organisation.CrmFields;
using HE.Investments.Organisation.CrmRepository;
using HE.Investments.Organisation.Extensions;
using Microsoft.FeatureManagement;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationServiceAsync2 _service;
    private readonly IOrganisationChangeRequestRepository _organisationChangeRequestRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IFeatureManager _featureManager;

    public OrganizationService(
        IOrganizationServiceAsync2 service,
        IOrganisationChangeRequestRepository organisationChangeRequestRepository,
        IContactRepository contactRepository,
        IFeatureManager featureManager)
    {
        _service = service;
        _organisationChangeRequestRepository = organisationChangeRequestRepository;
        _contactRepository = contactRepository;
        _featureManager = featureManager;
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

    public async Task<ContactDto?> GetOrganisationChangeDetailsRequestContact(string accountId)
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
            var isAhpEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.AhpProgram);
            var account = await _service.RetrieveAsync(AccountEntity.Name, organizationId, AccountEntity.AllColumns(isAhpEnabled));

            organizationDetailsDto.registeredCompanyName = account.GetStringAttribute(AccountEntity.Properties.CompanyName);
            organizationDetailsDto.companyRegistrationNumber = account.GetStringAttribute(AccountEntity.Properties.CompanyNumber);
            organizationDetailsDto.addressLine1 = account.GetStringAttribute(AccountEntity.Properties.AddressLine1);
            organizationDetailsDto.addressLine2 = account.GetStringAttribute(AccountEntity.Properties.AddressLine2);
            organizationDetailsDto.addressLine3 = account.GetStringAttribute(AccountEntity.Properties.AddressLine3);
            organizationDetailsDto.city = account.GetStringAttribute(AccountEntity.Properties.City);
            organizationDetailsDto.postalcode = account.GetStringAttribute(AccountEntity.Properties.PostalCode);
            organizationDetailsDto.country = account.GetStringAttribute(AccountEntity.Properties.Country);
            organizationDetailsDto.isUnregisteredBody = account.GetBooleanAttribute(AccountEntity.Properties.UnregisteredBody) ?? false;

            if (isAhpEnabled)
            {
                organizationDetailsDto.investmentPartnerStatus = account.GetOptionSetAttribute(AccountEntity.Properties.InvestmentPartnerStatus)?.Value;
            }

            var primaryContactReference = account.GetEntityReference(AccountEntity.Properties.PrimaryContactId);
            if (primaryContactReference != null)
            {
                var contact = await _service.RetrieveAsync(
                    ContactEntity.Name,
                    primaryContactReference.Id,
                    new ColumnSet(ContactEntity.Properties.FullName, ContactEntity.Properties.EmailAddress, ContactEntity.Properties.TelephoneNumber));

                organizationDetailsDto.compayAdminContactName = contact.GetStringAttribute(ContactEntity.Properties.FullName);
                organizationDetailsDto.compayAdminContactEmail = contact.GetStringAttribute(ContactEntity.Properties.EmailAddress);
                organizationDetailsDto.compayAdminContactTelephone = contact.GetStringAttribute(ContactEntity.Properties.TelephoneNumber);
            }
        }

        return organizationDetailsDto;
    }

    private static Entity MapOrganizationDtoToEntity(OrganizationDetailsDto organizationDetailsDto)
    {
        var organizationEntity = new Entity(AccountEntity.Name)
        {
            Attributes = new AttributeCollection
            {
                { AccountEntity.Properties.CompanyName, organizationDetailsDto.registeredCompanyName },
                { AccountEntity.Properties.CompanyNumber, organizationDetailsDto.companyRegistrationNumber },
                { AccountEntity.Properties.AddressLine1, organizationDetailsDto.addressLine1 },
                { AccountEntity.Properties.AddressLine2, organizationDetailsDto.addressLine2 },
                { AccountEntity.Properties.AddressLine3, organizationDetailsDto.addressLine3 },
                { AccountEntity.Properties.City, organizationDetailsDto.city },
                { AccountEntity.Properties.PostalCode, organizationDetailsDto.postalcode },
                { AccountEntity.Properties.Country, organizationDetailsDto.country },
                { AccountEntity.Properties.County, organizationDetailsDto.county },
            },
        };
        return organizationEntity;
    }

    private static Entity MapOrganizationDtoToOrganizationChangeRequestEntity(OrganizationDetailsDto organizationDetailsDto)
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
                { "invln_organisationid", new EntityReference(AccountEntity.Name, new Guid(organizationDetailsDto.organisationId)) },
            },
        };
        return organisationChangeRequestEntity;
    }
}
