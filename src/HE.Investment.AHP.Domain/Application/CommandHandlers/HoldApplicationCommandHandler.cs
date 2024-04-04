using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class HoldApplicationCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<HoldApplicationCommand, OperationResult>
{
    public HoldApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(HoldApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            application =>
            {
                application.Hold(new HoldReason(request.HoldReason));

                return Task.CompletedTask;
            },
            request.Id,
            cancellationToken);
    }
}
