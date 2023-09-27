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
        var userDetails = await _loanUserRepository.GetUserDetails(_loanUserContext.UserGlobalId);

        userDetails.ProvideUserDetails(
            request.FirstName,
            request.LastName,
            request.JobTitle,
            request.TelephoneNumber,
            request.SecondaryTelephoneNumber,
            _loanUserContext.Email);

        await _loanUserRepository.SaveAsync(userDetails, _loanUserContext.UserGlobalId, cancellationToken);

        _loanUserContext.RefreshUserData();
    }
}
