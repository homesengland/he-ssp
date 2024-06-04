using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Contract.Queries;
using HE.Investments.Organisation.Services;
using MediatR;

namespace HE.Investments.Organisation.QueryHandlers;

internal sealed class GetOrganisationDetailsQueryHandler : IRequestHandler<GetOrganisationDetailsQuery, OrganisationDetails>
{
    private readonly IOrganisationSearchService _organisationSearchService;

    public GetOrganisationDetailsQueryHandler(IOrganisationSearchService organisationSearchService)
    {
        _organisationSearchService = organisationSearchService;
    }

    public async Task<OrganisationDetails> Handle(GetOrganisationDetailsQuery request, CancellationToken cancellationToken)
    {
        var organisationDetails = await _organisationSearchService.GetByOrganisation(
            ShortGuid.TryToGuidAsString(request.OrganisationIdentifier.Value),
            cancellationToken);
        if (organisationDetails.Item.IsNotProvided())
        {
            throw new NotFoundException(nameof(OrganisationDetails), request.OrganisationIdentifier);
        }

        return new OrganisationDetails(
            organisationDetails.Item!.Name,
            organisationDetails.Item.Street,
            organisationDetails.Item.City,
            organisationDetails.Item.PostalCode,
            organisationDetails.Item.CompanyNumber,
            organisationDetails.Item.OrganisationId);
    }
}
