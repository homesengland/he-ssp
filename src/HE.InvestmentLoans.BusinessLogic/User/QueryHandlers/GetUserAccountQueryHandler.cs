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
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var userDetails = await _loanUserContext.GetUserDetails();

        return new GetUserAccountResponse(
                            selectedAccount.UserEmail,
                            _loanUserContext.UserGlobalId.ToString(),
                            await _loanUserContext.GetSelectedAccountId(),
                            (await _loanUserContext.GetAllAccounts()).Select(x => x.AccountId!.Value).ToList(),
                            userDetails.FirstName?.ToString(),
                            userDetails.LastName?.ToString(),
                            userDetails.TelephoneNumber?.ToString());
    }
}
