using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Utils;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

internal sealed class SubmitClaimCommandHandler : IRequestHandler<SubmitClaimCommand, OperationResult>
{
    private readonly IPhaseRepository _phaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IDateTimeProvider _dateTimeProvider;

    public SubmitClaimCommandHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext, IDateTimeProvider dateTimeProvider)
    {
        _phaseRepository = phaseRepository;
        _accountUserContext = accountUserContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<OperationResult> Handle(SubmitClaimCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var phase = await _phaseRepository.GetById(request.PhaseId, request.AllocationId, userAccount, cancellationToken);

        phase.SubmitMilestoneClaim(request.MilestoneType, _dateTimeProvider.Now);
        await _phaseRepository.Save(phase, userAccount, cancellationToken);

        return OperationResult.Success();
    }
}
