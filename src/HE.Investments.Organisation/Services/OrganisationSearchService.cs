using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;

public class OrganisationSearchService : IOrganisationSearchService
{
    private readonly ICompaniesHouseApi _companiesHouseApi;
    private readonly IOrganizationRepository _organizationRepository;

    public OrganisationSearchService(ICompaniesHouseApi companiesHouseApi, IOrganizationRepository organizationRepository)
    {
        _companiesHouseApi = companiesHouseApi;
        _organizationRepository = organizationRepository;
    }

    public async Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, string? companyNumber, CancellationToken cancellationToken)
    {
        var companyHouseApiResult = await _companiesHouseApi.Search(organisationName, pagingParams, cancellationToken);

        return new OrganisationSearchResult(companyHouseApiResult.Items.Select(x => new OrganisationSearchItem(x.CompanyNumber, x.CompanyName, x.OfficeAddress.Locality!, x.OfficeAddress.AddressLine1!, x.OfficeAddress.PostalCode!)).ToList(), companyHouseApiResult.Hits, null!);
    }

    public List<OrganizationDetailsDto> SearchOrganizationInCrm(List<string> organisationNumbers, IOrganizationServiceAsync2 service)
    {
        var retrievedEntities = _organizationRepository.SearchForOrganizations(service, organisationNumbers);
        if (retrievedEntities != null && retrievedEntities.Entities.Count > 0)
        {
            var organizationDtoList = new List<OrganizationDetailsDto>();
            foreach (var account in retrievedEntities.Entities)
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
                };
                organizationDtoList.Add(organization);
            }

            return organizationDtoList;
        }

        return new List<OrganizationDetailsDto>();
    }
}
