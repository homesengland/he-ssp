extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Contract.User.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;

public class ProvideUserDetailsCommandHandler : IRequestHandler<ProvideUserDetailsCommand>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly ILoanUserRepository _loanUserRepository;

    public ProvideUserDetailsCommandHandler(ILoanUserContext loanUserContext, ILoanUserRepository loanUserRepository)
    {
        _loanUserContext = loanUserContext;
        _loanUserRepository = loanUserRepository;
    }

    public async Task Handle(ProvideUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();

        var userDetails = await _loanUserRepository.GetUserDetails(selectedAccount.UserGlobalId);

        userDetails.ProvideUserDetails(
            request.FirstName,
            request.Surname,
            request.JobTitle,
            request.TelephoneNumber,
            request.SecondaryTelephoneNumber,
            selectedAccount.UserEmail);

        await _loanUserRepository.SaveAsync(userDetails, selectedAccount.UserGlobalId, cancellationToken);

        _loanUserContext.RefreshUserData();
    }
}
