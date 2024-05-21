extern alias Org;

using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using MediatR;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetConsortiumDetailsQueryHandler : IRequestHandler<GetConsortiumDetailsQuery, ConsortiumDetails>
{
    private readonly IConsortiumRepository _repository;

    private readonly IDraftConsortiumRepository _draftConsortiumRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IOrganizationCrmSearchService _organisationSearchService;

    public GetConsortiumDetailsQueryHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IAccountUserContext accountUserContext,
        IOrganizationCrmSearchService organisationSearchService)
    {
        _repository = repository;
        _draftConsortiumRepository = draftConsortiumRepository;
        _accountUserContext = accountUserContext;
        _organisationSearchService = organisationSearchService;
    }

    public async Task<ConsortiumDetails> Handle(GetConsortiumDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        return await GetDraftConsortiumDetails(request, account) ?? await GetConsortiumDetails(request, account, cancellationToken);
    }

    private static ConsortiumMemberDetails CreateMemberDetails(IConsortiumMember consortiumMember, OrganizationDetailsDto organisationDetails)
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

    private static OrganizationDetailsDto GetOrganisationDetails(IList<OrganizationDetailsDto> organisations, OrganisationId id)
    {
        return organisations.Single(x => x.organisationId == id.ToGuidAsString());
    }

    private async Task<ConsortiumDetails?> GetDraftConsortiumDetails(GetConsortiumDetailsQuery request, UserAccount account)
    {
        var consortium = _draftConsortiumRepository.Get(request.ConsortiumId, account);
        if (consortium.IsNotProvided())
        {
            return null;
        }

        var organisations = await FetchOrganisationAddress([consortium!.LeadPartner, .. consortium.Members], request.FetchAddress);

        return new ConsortiumDetails(
            consortium.Id,
            consortium.Programme,
            CreateMemberDetails(consortium.LeadPartner, GetOrganisationDetails(organisations, consortium.LeadPartner.Id)),
            true,
            consortium.Members.Select(x => CreateMemberDetails(x, GetOrganisationDetails(organisations, x.Id))).ToList());
    }

    private async Task<ConsortiumDetails> GetConsortiumDetails(GetConsortiumDetailsQuery request, UserAccount account, CancellationToken cancellationToken)
    {
        var consortium = await _repository.GetConsortium(request.ConsortiumId, account, cancellationToken);
        var organisations = await FetchOrganisationAddress([consortium.LeadPartner, .. consortium.Members], request.FetchAddress);

        return new ConsortiumDetails(
            consortium.Id,
            consortium.Programme,
            CreateMemberDetails(consortium.LeadPartner, GetOrganisationDetails(organisations, consortium.LeadPartner.Id)),
            false,
            consortium.Members.Select(x => CreateMemberDetails(x, GetOrganisationDetails(organisations, x.Id))).ToList());
    }

    private async Task<IList<OrganizationDetailsDto>> FetchOrganisationAddress(IEnumerable<IConsortiumMember> organisations, bool fetchAddress)
    {
        if (fetchAddress)
        {
            return await _organisationSearchService.GetOrganizationFromCrmByOrganisationId(organisations.Select(x => x.Id.ToGuidAsString()));
        }

        return organisations.Select(x =>
                new OrganizationDetailsDto
                {
                    organisationId = x.Id.ToGuidAsString(),
                    registeredCompanyName = x.OrganisationName,
                    addressLine1 = string.Empty,
                    city = string.Empty,
                    postalcode = string.Empty,
                    companyRegistrationNumber = string.Empty,
                })
            .ToList();
    }
}
