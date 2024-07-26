using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

internal abstract class ProvideClaimDetailsBaseCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IProvideClaimDetailsCommand, IRequest<OperationResult>
{
    private readonly IPhaseRepository _phaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    protected ProvideClaimDetailsBaseCommandHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext)
    {
        _phaseRepository = phaseRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var phase = await _phaseRepository.GetById(request.PhaseId, request.AllocationId, userAccount, cancellationToken);
        var claim = phase.GetMilestoneClaim(request.MilestoneType)
                    ?? throw new NotFoundException(nameof(MilestoneClaim), request.MilestoneType);

        phase.ProvideMilestoneClaim(Provide(request, claim));
        await _phaseRepository.Save(phase, userAccount, cancellationToken);

        return OperationResult.Success();
    }

    protected abstract MilestoneClaimBase Provide(TCommand request, MilestoneClaimBase claim);
}
