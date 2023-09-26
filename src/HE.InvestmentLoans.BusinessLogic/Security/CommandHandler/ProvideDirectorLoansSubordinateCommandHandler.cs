using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
internal class ProvideDirectorLoansSubordinateCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ProvideDirectorLoansSubordinateCommand, OperationResult>
{
    public ProvideDirectorLoansSubordinateCommandHandler(ISecurityRepository repository, ILoanUserContext loanUserContext, ILogger<SecurityBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideDirectorLoansSubordinateCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            security =>
            {
                var directorLoansSubordinate = request.CanBeSubordinated.IsProvided() ? DirectorLoansSubordinate.FromString(request.CanBeSubordinated, request.ReasonWhyCannotBeSubordinated) : null;

                security.ProvideDirectorLoansSubordinate(directorLoansSubordinate!);
            },
            request.Id,
            cancellationToken);
    }
}
