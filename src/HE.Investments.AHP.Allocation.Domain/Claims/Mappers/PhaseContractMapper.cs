using System.Globalization;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public sealed class PhaseContractMapper : IPhaseContractMapper
{
    private readonly IMilestoneClaimContractMapper _milestoneClaimMapper;

    private readonly IDateTimeProvider _dateTimeProvider;

    public PhaseContractMapper(IMilestoneClaimContractMapper milestoneClaimMapper, IDateTimeProvider dateTimeProvider)
    {
        _milestoneClaimMapper = milestoneClaimMapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public Phase Map(PhaseEntity phase)
    {
        var today = _dateTimeProvider.Now.Date;
        var milestoneClaims = new[] { MilestoneType.Acquisition, MilestoneType.StartOnSite, MilestoneType.Completion }
            .Select(x => _milestoneClaimMapper.Map(x, phase, today))
            .Where(x => x.IsProvided())
            .Select(x => x!)
            .ToList();

        return new Phase(
            phase.Id,
            phase.Name.Value,
            MapAllocationInfo(phase.Allocation),
            phase.NumberOfHomes.Value.ToString(CultureInfo.InvariantCulture),
            phase.BuildActivityType.Value,
            milestoneClaims);
    }

    private static AllocationBasicInfo MapAllocationInfo(Allocation.ValueObjects.AllocationBasicInfo allocationBasicInfo)
    {
        return new(
            allocationBasicInfo.Id,
            allocationBasicInfo.Name,
            allocationBasicInfo.ReferenceNumber,
            allocationBasicInfo.LocalAuthority,
            allocationBasicInfo.Programme.ShortName,
            allocationBasicInfo.Tenure);
    }
}
