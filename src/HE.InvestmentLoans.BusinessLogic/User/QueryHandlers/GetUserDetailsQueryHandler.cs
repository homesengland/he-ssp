using HE.InvestmentLoans.Contract.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsResponse>
{
    private readonly ILoanUserContext _loanUserContext;

    public GetUserDetailsQueryHandler(ILoanUserContext loanUserContext)
    {
        _loanUserContext = loanUserContext;
    }

    public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        return new GetUserDetailsResponse(
                            _loanUserContext.Email,
                            _loanUserContext.UserGlobalId,
                            await _loanUserContext.GetSelectedAccountId(),
                            await _loanUserContext.GetAllAccountIds());
    }
}
