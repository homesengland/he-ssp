using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Organization;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;

public class GetOrganizationDetailsQueryHandler : IRequestHandler<GetOrganizationBasicInformationQuery, GetOrganizationBasicInformationQueryResponse>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetOrganizationDetailsQueryHandler(IOrganizationRepository organizationRepository, ILoanUserContext loanUserContext)
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
