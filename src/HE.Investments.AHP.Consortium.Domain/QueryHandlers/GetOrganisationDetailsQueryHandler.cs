extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using MediatR;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetOrganisationDetailsQueryHandler : IRequestHandler<GetOrganisationDetailsQuery, OrganisationDetails>
{
    private readonly IOrganizationCrmSearchService _organisationSearchService;

    public GetOrganisationDetailsQueryHandler(IOrganizationCrmSearchService organisationSearchService)
    {
        _organisationSearchService = organisationSearchService;
    }

    public async Task<OrganisationDetails> Handle(GetOrganisationDetailsQuery request, CancellationToken cancellationToken)
    {
        var organisations = await _organisationSearchService.GetOrganizationFromCrmByOrganisationId([request.OrganisationId.ToGuidAsString()]);
        var organisationDetails = organisations.FirstOrDefault();
        if (organisationDetails.IsNotProvided())
        {
            throw new NotFoundException(nameof(OrganisationDetails), request.OrganisationId.ToGuidAsString());
        }

        return new OrganisationDetails(
            organisationDetails!.registeredCompanyName,
            organisationDetails.addressLine1,
            organisationDetails.city,
            organisationDetails.postalcode,
            organisationDetails.companyRegistrationNumber,
            request.OrganisationId.ToString());
    }
}
