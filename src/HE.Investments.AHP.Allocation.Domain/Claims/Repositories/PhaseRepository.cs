using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Allocation.Crm;
using HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Repositories;

public class PhaseRepository : IPhaseRepository
{
    private readonly IAllocationCrmContext _allocationCrmContext;

    private readonly IPhaseCrmMapper _phaseCrmMapper;

    private readonly IAllocationBasicInfoMapper _allocationBasicInfoMapper;

    public PhaseRepository(
        IAllocationCrmContext allocationCrmContext,
        IPhaseCrmMapper phaseCrmMapper,
        IAllocationBasicInfoMapper allocationBasicInfoMapper)
    {
        _allocationCrmContext = allocationCrmContext;
        _phaseCrmMapper = phaseCrmMapper;
        _allocationBasicInfoMapper = allocationBasicInfoMapper;
    }

    public async Task<PhaseEntity> GetById(PhaseId phaseId, AllocationId allocationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var allocation = await _allocationCrmContext.GetById(allocationId.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);
        var phase = allocation.ListOfPhaseClaims.Find(x => x.Id == phaseId.ToGuidAsString())
                    ?? throw new NotFoundException(nameof(PhaseEntity), phaseId);
        var allocationBasicInfo = await _allocationBasicInfoMapper.Map(allocation, cancellationToken);

        return _phaseCrmMapper.MapToDomain(phase, allocationBasicInfo);
    }

    public Task Save(PhaseEntity phaseEntity, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!phaseEntity.IsModified)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask; // todo implement when save milestones will be added
    }
}
