using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetConsortiumDetailsQueryHandler : IRequestHandler<GetConsortiumDetailsQuery, ConsortiumDetails>
{
    private readonly IConsortiumRepository _repository;

    private readonly IDraftConsortiumRepository _draftConsortiumRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IOrganizationCrmSearchService _organisationSearchService;

    private readonly IMediator _mediator;

    public GetConsortiumDetailsQueryHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IAccountUserContext accountUserContext,
        IOrganizationCrmSearchService organisationSearchService,
        IMediator mediator)
    {
        _repository = repository;
        _draftConsortiumRepository = draftConsortiumRepository;
        _accountUserContext = accountUserContext;
        _organisationSearchService = organisationSearchService;
        _mediator = mediator;
    }

    public async Task<ConsortiumDetails> Handle(GetConsortiumDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        return await GetDraftConsortiumDetails(request, account, cancellationToken) ?? await GetConsortiumDetails(request, account, cancellationToken);
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

    private async Task<ConsortiumDetails?> GetDraftConsortiumDetails(GetConsortiumDetailsQuery request, UserAccount account, CancellationToken cancellationToken)
    {
        var consortium = _draftConsortiumRepository.Get(request.ConsortiumId, account);
        if (consortium.IsNotProvided())
        {
            return null;
        }

        var programme = await _mediator.Send(new GetProgrammeQuery(consortium!.ProgrammeId), cancellationToken);
        var organisations = await FetchOrganisationAddress([consortium.LeadPartner, .. consortium.Members], request.FetchAddress);

        return new ConsortiumDetails(
            consortium.Id,
            programme,
            CreateMemberDetails(consortium.LeadPartner, GetOrganisationDetails(organisations, consortium.LeadPartner.Id)),
            true,
            consortium.Members.Select(x => CreateMemberDetails(x, GetOrganisationDetails(organisations, x.Id))).ToList());
    }

    private async Task<ConsortiumDetails> GetConsortiumDetails(GetConsortiumDetailsQuery request, UserAccount account, CancellationToken cancellationToken)
    {
        var consortium = await _repository.GetConsortium(request.ConsortiumId, account, cancellationToken);
        var organisations = await FetchOrganisationAddress([consortium.LeadPartner, .. consortium.Members], request.FetchAddress);
        var programme = await _mediator.Send(new GetProgrammeQuery(consortium.ProgrammeId), cancellationToken);

        return new ConsortiumDetails(
            consortium.Id,
            programme,
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
