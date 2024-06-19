using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class WithdrawApplicationCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<WithdrawApplicationCommand, OperationResult>
{
    public WithdrawApplicationCommandHandler(IApplicationRepository applicationRepository, IConsortiumUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            application =>
            {
                application.Withdraw(new WithdrawReason(request.WithdrawReason));

                return Task.CompletedTask;
            },
            request.Id,
            cancellationToken);
    }
}
