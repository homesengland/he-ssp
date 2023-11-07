extern alias Org;

using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Organisation.Services;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;

public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, OrganizationBasicDetails>
{
    private readonly IOrganisationSearchService _searchService;

    public GetOrganizationQueryHandler(IOrganisationSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<OrganizationBasicDetails> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
    {
        var response = await _searchService.GetByOrganisation(request.CompanyHouseNumberOrOrganisationId, cancellationToken);
        if (response.Item.IsNotProvided())
        {
            throw new NotFoundException($"Organization with id {request.CompanyHouseNumberOrOrganisationId} not found");
        }

        var organization = response.Item!;

        return new OrganizationBasicDetails(
            organization.Name,
            organization.Street,
            organization.City,
            organization.PostalCode,
            organization.CompanyNumber,
            organization.OrganisationId);
    }
}
