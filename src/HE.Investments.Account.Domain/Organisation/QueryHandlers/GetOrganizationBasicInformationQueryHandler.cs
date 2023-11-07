using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.QueryHandlers;

public class GetOrganizationBasicInformationQueryHandler : IRequestHandler<GetOrganizationBasicInformationQuery, GetOrganizationBasicInformationQueryResponse>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetOrganizationBasicInformationQueryHandler(IOrganizationRepository organizationRepository, ILoanUserContext loanUserContext)
    {
        _organizationRepository = organizationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetOrganizationBasicInformationQueryResponse> Handle(GetOrganizationBasicInformationQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetBasicInformation(await _loanUserContext.GetSelectedAccount(), cancellationToken);
        return new GetOrganizationBasicInformationQueryResponse(result);
    }
}
