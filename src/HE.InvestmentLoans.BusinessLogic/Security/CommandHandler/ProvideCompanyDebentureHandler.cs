using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Security.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
internal class ProvideCompanyDebentureHandler : IRequestHandler<ProvideCompanyDebenture>
{
    private readonly ISecurityRepository _securityRepository;
    private readonly ILoanUserContext _loanUserContext;

    public ProvideCompanyDebentureHandler(ISecurityRepository securityRepository, ILoanUserContext loanUserContext)
    {
        _securityRepository = securityRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(ProvideCompanyDebenture request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var security = await _securityRepository.GetAsync(request.Id, userAccount, cancellationToken);

        security.ProvideDebenture(request.Debenture);

        await _securityRepository.SaveAsync(security, userAccount, cancellationToken);
    }
}
