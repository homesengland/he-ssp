using HE.InvestmentLoans.Contract.User.Queries;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;

public class GetUserAccountQueryHandler : IRequestHandler<GetUserAccountQuery, GetUserAccountResponse>
{
    private readonly IAccountUserContext _loanUserContext;

    public GetUserAccountQueryHandler(IAccountUserContext loanUserContext)
    {
        _loanUserContext = loanUserContext;
    }

    public async Task<GetUserAccountResponse> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var profileDetails = await _loanUserContext.GetProfileDetails();

        return new GetUserAccountResponse(
                            selectedAccount.UserEmail,
                            _loanUserContext.UserGlobalId.ToString(),
                            (await _loanUserContext.GetSelectedAccount()).AccountId,
                            Array.Empty<Guid>(),
                            profileDetails.FirstName?.ToString(),
                            profileDetails.LastName?.ToString(),
                            profileDetails.TelephoneNumber?.ToString());
    }
}
