using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.Services;

internal class OrganizationCrmSearchService : IOrganizationCrmSearchService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IOrganizationServiceAsync2 _organizationService;

    public OrganizationCrmSearchService(IOrganizationRepository organizationRepository, IOrganizationServiceAsync2 organizationService)
    {
        _organizationRepository = organizationRepository;
        _organizationService = organizationService;
    }

    public async Task<IList<OrganizationDetailsDto>> SearchOrganizationInCrmByName(string organisationNames, bool recordsWithCompanyHouseNumberIncluded)
    {
        IEnumerable<string> result = organisationNames.Split(' ').ToList();
        var retrievedEntities = await _organizationRepository.SearchForOrganizationsByName(_organizationService, result, recordsWithCompanyHouseNumberIncluded);

        return MapRetrievedEntites(retrievedEntities);
    }

    public async Task<IList<OrganizationDetailsDto>> SearchOrganizationInCrmByCompanyHouseNumber(IEnumerable<string> organisationNumbers)
    {
        var retrievedEntities = await _organizationRepository.SearchForOrganizationsByCompanyHouseNumber(_organizationService, organisationNumbers);
        return MapRetrievedEntites(retrievedEntities);
    }

    public async Task<IList<OrganizationDetailsDto>> GetOrganizationFromCrmByOrganisationId(IEnumerable<string> organisationIds)
    {
        organisationIds = organisationIds.Select(x => x.TryToGuidAsString());
        var retrievedEntities = await _organizationRepository.GetOrganisationsById(_organizationService, organisationIds);
        return MapRetrievedEntites(retrievedEntities);
    }

    public async Task<OrganizationDetailsDto?> SearchOrganizationInCrmByOrganizationId(string organizationId)
    {
        var retrievedEntities = await _organizationRepository.SearchForOrganizationsByOrganizationId(_organizationService, organizationId.TryToGuidAsString());
        if (retrievedEntities != null)
        {
            return MapEntityToDto(retrievedEntities);
        }

        return null;
    }

    private static OrganizationDetailsDto MapEntityToDto(Entity account)
    {
        var organization = new OrganizationDetailsDto()
        {
            registeredCompanyName = account.Contains("name") ? account["name"].ToString() : null,
            companyRegistrationNumber = account.Contains("he_companieshousenumber") ? account["he_companieshousenumber"].ToString() : null,
            addressLine1 = account.Contains("address1_line1") ? account["address1_line1"].ToString() : null,
            addressLine2 = account.Contains("address1_line2") ? account["address1_line2"].ToString() : null,
            addressLine3 = account.Contains("address1_line3") ? account["address1_line3"].ToString() : null,
            city = account.Contains("address1_city") ? account["address1_city"].ToString() : null,
            postalcode = account.Contains("address1_postalcode") ? account["address1_postalcode"].ToString() : null,
            country = account.Contains("address1_country") ? account["address1_country"].ToString() : null,
            organisationId = account.Contains("accountid") ? account["accountid"].ToString()!.TryToGuidAsString() : null,
        };
        return organization;
    }

    private static List<OrganizationDetailsDto> MapRetrievedEntites(EntityCollection? retrievedEntities)
    {
        if (retrievedEntities != null && retrievedEntities.Entities.Count > 0)
        {
            var organizationDtoList = new List<OrganizationDetailsDto>();
            foreach (var account in retrievedEntities.Entities)
            {
                organizationDtoList.Add(MapEntityToDto(account));
            }

            return organizationDtoList;
        }

        return [];
    }
}
