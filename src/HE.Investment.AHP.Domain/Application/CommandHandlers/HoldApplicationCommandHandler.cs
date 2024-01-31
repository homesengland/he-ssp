using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class HoldApplicationCommandHandler : ChangeApplicationStatusCommandHandler, IRequestHandler<HoldApplicationCommand, OperationResult>
{
    public HoldApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(HoldApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async (applicationRepository, application, organisationId) =>
            {
                var holdReason = request.HoldReason.IsProvided()
                    ? new HoldReason(request.HoldReason!)
                    : null;

                await application.Hold(applicationRepository, holdReason, organisationId, cancellationToken);
                await applicationRepository.DispatchEvents(application, cancellationToken);
            },
            request.Id,
            cancellationToken);
    }
}
