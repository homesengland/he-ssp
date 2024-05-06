extern alias Org;

using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using MediatR;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetConsortiumDetailsQueryHandler : IRequestHandler<GetConsortiumDetailsQuery, ConsortiumDetails>
{
    private readonly IConsortiumRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IOrganizationCrmSearchService _organisationSearchService;

    public GetConsortiumDetailsQueryHandler(
        IConsortiumRepository repository,
        IAccountUserContext accountUserContext,
        IOrganizationCrmSearchService organisationSearchService)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _organisationSearchService = organisationSearchService;
    }

    public async Task<ConsortiumDetails> Handle(GetConsortiumDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var consortium = await _repository.GetConsortium(request.ConsortiumId, account, cancellationToken);
        var organisations = await FetchOrganisationAddress(consortium, request.FetchAddress);

        return new ConsortiumDetails(
            consortium.Id,
            consortium.Programme,
            CreateMemberDetails(consortium.LeadPartner, GetDetails(organisations, consortium.LeadPartner.Id)),
            consortium.Members.Select(x => CreateMemberDetails(x, GetDetails(organisations, x.Id))).ToList());
    }

    private static ConsortiumMemberDetails CreateMemberDetails(ConsortiumMember consortiumMember, OrganizationDetailsDto organisationDetails)
    {
        return new ConsortiumMemberDetails(
            consortiumMember.Id,
            consortiumMember.Status,
            new OrganisationDetails(
                organisationDetails.registeredCompanyName,
                organisationDetails.addressLine1,
                organisationDetails.city,
                organisationDetails.postalcode,
                organisationDetails.companyRegistrationNumber,
                consortiumMember.Id.ToString()));
    }

    private static OrganizationDetailsDto GetDetails(IList<OrganizationDetailsDto> organisations, OrganisationId id)
    {
        return organisations.Single(x => x.organisationId == id.ToString());
    }

    private async Task<IList<OrganizationDetailsDto>> FetchOrganisationAddress(ConsortiumEntity consortium, bool fetchAddress)
    {
        var organisations = consortium.Members.Concat(new[] { consortium.LeadPartner });
        if (fetchAddress)
        {
            return await _organisationSearchService.GetOrganizationFromCrmByOrganisationId(organisations.Select(x => x.Id.ToString()));
        }

        return organisations.Select(x =>
                new OrganizationDetailsDto
                {
                    organisationId = x.Id.ToString(),
                    registeredCompanyName = x.OrganisationName,
                    addressLine1 = string.Empty,
                    city = string.Empty,
                    postalcode = string.Empty,
                    companyRegistrationNumber = string.Empty,
                })
            .ToList();
    }
}
