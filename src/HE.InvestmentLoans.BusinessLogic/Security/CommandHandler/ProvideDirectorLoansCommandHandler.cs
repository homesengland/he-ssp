using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class ProvideDirectorLoansCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideDirectorLoansCommand, OperationResult>
{
    public ProvideDirectorLoansCommandHandler(ISecurityRepository securityRepository, ILoanUserContext loanUserContext)
        : base(securityRepository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideDirectorLoansCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            security =>
            {
                var directorLoans = request.Exists.IsProvided() ?
                    DirectorLoans.FromString(request.Exists) :
                    null;

                security.ProvideDirectorLoans(directorLoans!);
            },
            request.Id,
            cancellationToken);
    }
}
