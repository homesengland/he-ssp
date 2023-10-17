using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Organization;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;

public class GetOrganizationChangeRequestDetailsQueryHandler : IRequestHandler<GetOrganizationChangeRequestDetailsQuery, GetOrganizationChangeRequestDetailsQueryResponse>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetOrganizationChangeRequestDetailsQueryHandler(IOrganizationRepository organizationRepository, ILoanUserContext loanUserContext)
    {
        _organizationRepository = organizationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetOrganizationChangeRequestDetailsQueryResponse> Handle(GetOrganizationChangeRequestDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetOrganisationChangeRequestDetails(await _loanUserContext.GetSelectedAccount(), cancellationToken);
        return new GetOrganizationChangeRequestDetailsQueryResponse(result);
    }
}
