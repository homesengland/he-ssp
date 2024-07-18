using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

internal sealed class CancelClaimCommandHandler : IRequestHandler<CancelClaimCommand, OperationResult>
{
    private readonly IPhaseRepository _phaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    public CancelClaimCommandHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext)
    {
        _phaseRepository = phaseRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(CancelClaimCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var phase = await _phaseRepository.GetById(request.PhaseId, request.AllocationId, userAccount, cancellationToken);

        phase.CancelMilestoneClaim(request.MilestoneType);
        await _phaseRepository.Save(phase, userAccount, cancellationToken);

        return OperationResult.Success();
    }
}
