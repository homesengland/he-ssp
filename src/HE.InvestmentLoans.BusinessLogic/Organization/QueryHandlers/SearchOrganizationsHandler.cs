extern alias Org;

using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.CompaniesHouse.Contract;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
internal class SearchOrganizationsHandler : IRequestHandler<SearchOrganizationsQuery, SearchOrganisationsQueryResponse>
{
    private readonly IOrganisationSearchService _searchService;

    public SearchOrganizationsHandler(IOrganisationSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<SearchOrganisationsQueryResponse> Handle(SearchOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var companyHousesResult = await _searchService.Search(request.SearchPhrase, new PagingQueryParams(request.PageSize, request.Page - 1), request.SearchPhrase, cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            throw new ExternalServiceException();
        }

        var viewmodel = new OrganizationViewModel
        {
            Organizations = companyHousesResult.Items
            .Select(c => new OrganizationBasicDetails(c.Name, c.Street, c.City, c.PostalCode, c.CompanyNumber)),
            Name = request.SearchPhrase,
            TotalOrganizations = companyHousesResult.TotalItems,
            Page = request.Page,
        };

        return new SearchOrganisationsQueryResponse(viewmodel);
    }

}
