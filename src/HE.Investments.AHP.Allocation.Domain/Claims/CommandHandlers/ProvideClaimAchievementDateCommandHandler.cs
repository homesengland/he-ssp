using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Utils;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

public sealed class ProvideClaimAchievementDateCommandHandler : IRequestHandler<ProvideClaimAchievementDateCommand, OperationResult>
{
    private readonly IPhaseRepository _phaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IAllocationRepository _allocationRepository;

    private readonly DateTimeProvider _dateTimeProvider;

    public ProvideClaimAchievementDateCommandHandler(
        IPhaseRepository phaseRepository,
        IAccountUserContext accountUserContext,
        IAllocationRepository allocationRepository,
        DateTimeProvider dateTimeProvider)
    {
        _phaseRepository = phaseRepository;
        _accountUserContext = accountUserContext;
        _allocationRepository = allocationRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<OperationResult> Handle(ProvideClaimAchievementDateCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var allocation = _allocationRepository.GetById(request.AllocationId, userAccount, cancellationToken).Result;
        var phase = await _phaseRepository.GetById(request.PhaseId, request.AllocationId, userAccount, cancellationToken);

        var claim = phase.GetMilestoneClaim(request.MilestoneType)
                    ?? throw new NotFoundException(nameof(MilestoneClaim), request.MilestoneType);
        phase.ProvideMilestoneClaimAchievementDate(claim, allocation.Programme, request.AchievementDate, _dateTimeProvider);
        await _phaseRepository.Save(phase, userAccount, cancellationToken);

        return OperationResult.Success();
    }
}
