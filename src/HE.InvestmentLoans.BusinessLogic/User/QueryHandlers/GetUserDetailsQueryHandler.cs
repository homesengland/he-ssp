using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Contract.User.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsResponse>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly ILoanUserRepository _loanUserRepository;

    public GetUserDetailsQueryHandler(ILoanUserContext loanUserContext, ILoanUserRepository loanUserRepository)
    {
        _loanUserContext = loanUserContext;
        _loanUserRepository = loanUserRepository;
    }

    public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var userDetails = await _loanUserRepository.GetUserDetails(selectedAccount.UserGlobalId);

        return new GetUserDetailsResponse(UserDetailsMapper.MapToViewModel(userDetails));
    }
}
