using HE.Investment.AHP.Contract.Application;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.QueryHandlers;

internal sealed class GetPhaseClaimsQueryHandler : IRequestHandler<GetPhaseClaimsQuery, Phase>
{
    private readonly IPhaseRepository _phaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetPhaseClaimsQueryHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext)
    {
        _phaseRepository = phaseRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<Phase> Handle(GetPhaseClaimsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var phase = await _phaseRepository.GetById(request.PhaseId, request.AllocationId, userAccount, cancellationToken);
        var milestones = new[]
            {
                MapMilestoneClaim(MilestoneType.Acquisition, phase.AcquisitionMilestone),
                MapMilestoneClaim(MilestoneType.StartOnSite, phase.StartOnSiteMilestone),
                MapMilestoneClaim(MilestoneType.Completion, phase.CompletionMilestone),
            }
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();

        return new Phase(
            phase.Name.Value,
            new AllocationBasicInfo(
                request.AllocationId,
                "Allocation one", // TODO: AB#85082 Add AllocationBasicInfo to PhaseEntity and map
                "G0001231",
                "Oxford",
                "AHP 21-26 CME",
                Tenure.SharedOwnership),
            milestones);
    }

    private static MilestoneClaim? MapMilestoneClaim(MilestoneType milestoneType, Domain.Claims.ValueObjects.MilestoneClaim? milestoneClaim)
    {
        if (milestoneClaim.IsNotProvided())
        {
            return null;
        }

        return new MilestoneClaim(
            milestoneType,
            MilestoneStatus.Due, // TODO: AB#102109 Calculate display status
            milestoneClaim!.GrantApportioned.Amount,
            milestoneClaim.GrantApportioned.Percentage,
            DateDetails.FromDateTime(milestoneClaim.ClaimDate.ForecastClaimDate)!,
            DateDetails.FromDateTime(milestoneClaim.ClaimDate.ActualClaimDate),
            null, // TODO: AB#85082 Map Submission date when available
            false); // TODO: AB#102144 Calculate whether can be claimed
    }
}
