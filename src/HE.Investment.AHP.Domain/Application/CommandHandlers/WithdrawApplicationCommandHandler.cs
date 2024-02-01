using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class WithdrawApplicationCommandHandler : ChangeApplicationStatusCommandHandler, IRequestHandler<WithdrawApplicationCommand, OperationResult>
{
    public WithdrawApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async (applicationRepository, application, organisationId) =>
            {
                var withdrawReason = new WithdrawReason(request.WithdrawReason);

                await application.Withdraw(applicationRepository, withdrawReason, organisationId, cancellationToken);
                await applicationRepository.DispatchEvents(application, cancellationToken);
            },
            request.Id,
            cancellationToken);
    }
}
