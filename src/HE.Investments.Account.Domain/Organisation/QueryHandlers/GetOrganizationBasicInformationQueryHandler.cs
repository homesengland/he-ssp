using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.QueryHandlers;

public class GetOrganizationBasicInformationQueryHandler : IRequestHandler<GetOrganizationBasicInformationQuery, GetOrganizationBasicInformationQueryResponse>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetOrganizationBasicInformationQueryHandler(IOrganizationRepository organizationRepository, IAccountUserContext accountUserContext)
    {
        _organizationRepository = organizationRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<GetOrganizationBasicInformationQueryResponse> Handle(GetOrganizationBasicInformationQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var result = await _organizationRepository.GetBasicInformation(account.SelectedOrganisationId(), cancellationToken);

        return new GetOrganizationBasicInformationQueryResponse(result);
    }
}
