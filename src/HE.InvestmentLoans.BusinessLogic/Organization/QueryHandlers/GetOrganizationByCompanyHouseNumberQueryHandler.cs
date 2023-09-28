extern alias Org;

using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;

public class GetOrganizationByCompanyHouseNumberQueryHandler : IRequestHandler<GetOrganizationByCompanyHouseNumberQuery, OrganizationBasicDetails>
{
    private readonly IOrganisationSearchService _searchService;

    public GetOrganizationByCompanyHouseNumberQueryHandler(IOrganisationSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<OrganizationBasicDetails> Handle(GetOrganizationByCompanyHouseNumberQuery request, CancellationToken cancellationToken)
    {
        var response = await _searchService.GetByCompaniesHouseNumber(request.CompanyHouseNumber, cancellationToken);
        if (response.Item.IsNotProvided())
        {
            throw new NotFoundException($"Organization with company house number {request.CompanyHouseNumber} not found");
        }

        var organization = response.Item;

        return new OrganizationBasicDetails(organization.Name, organization.Street, organization.City, organization.PostalCode, organization.CompanyNumber);
    }
}
