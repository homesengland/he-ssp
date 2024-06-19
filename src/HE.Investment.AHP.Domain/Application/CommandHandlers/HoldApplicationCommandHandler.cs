using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class HoldApplicationCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<HoldApplicationCommand, OperationResult>
{
    public HoldApplicationCommandHandler(IApplicationRepository applicationRepository, IConsortiumUserContext accountUserContext)
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
