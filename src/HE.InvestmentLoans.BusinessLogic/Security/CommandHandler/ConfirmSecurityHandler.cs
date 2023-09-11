using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Security.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class ConfirmSecurityHandler : IRequestHandler<ConfirmSecurity>
{
    private readonly ISecurityRepository _securityRepository;
    private readonly ILoanUserContext _userContext;

    public ConfirmSecurityHandler(ISecurityRepository securityRepository, ILoanUserContext userContext)
    {
        _securityRepository = securityRepository;
        _userContext = userContext;
    }

    public async Task Handle(ConfirmSecurity request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var security = await _securityRepository.GetAsync(request.Id, userAccount, cancellationToken);

        security.Confirm();

        await _securityRepository.SaveAsync(security, userAccount, cancellationToken);
    }
}
