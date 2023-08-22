using HE.InvestmentLoans.Contract.User.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;

public class GetUserAccountQueryHandler : IRequestHandler<GetUserAccountQuery, GetUserAccountResponse>
{
    private readonly ILoanUserContext _loanUserContext;

    public GetUserAccountQueryHandler(ILoanUserContext loanUserContext)
    {
        _loanUserContext = loanUserContext;
    }

    public async Task<GetUserAccountResponse> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
    {
        return new GetUserAccountResponse(
                            _loanUserContext.Email,
                            _loanUserContext.UserGlobalId,
                            await _loanUserContext.GetSelectedAccountId(),
                            await _loanUserContext.GetAllAccountIds());
    }
}
