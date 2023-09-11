using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Security.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
internal class ProvideDirectLoansHandler : IRequestHandler<ProvideDirectLoans>
{
    private readonly ISecurityRepository _securityRepository;
    private readonly ILoanUserContext _loanUserContext;

    public ProvideDirectLoansHandler(ISecurityRepository securityRepository, ILoanUserContext loanUserContext)
    {
        _securityRepository = securityRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(ProvideDirectLoans request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var security = await _securityRepository.GetAsync(request.Id, userAccount, cancellationToken);

        security.ProvideDirectLoans(request.DirectLoans);

        await _securityRepository.SaveAsync(security, userAccount, cancellationToken);
    }
}
