using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Allocation.Crm;
using HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Repositories;

public class PhaseRepository : IPhaseRepository
{
    private readonly IAllocationCrmContext _allocationCrmContext;

    private readonly IPhaseCrmMapper _phaseCrmMapper;

    private readonly IAllocationBasicInfoMapper _allocationBasicInfoMapper;

    private readonly IEventDispatcher _eventDispatcher;

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public PhaseRepository(
        IAllocationCrmContext allocationCrmContext,
        IPhaseCrmMapper phaseCrmMapper,
        IAllocationBasicInfoMapper allocationBasicInfoMapper,
        IEventDispatcher eventDispatcher,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy)
    {
        _allocationCrmContext = allocationCrmContext;
        _phaseCrmMapper = phaseCrmMapper;
        _allocationBasicInfoMapper = allocationBasicInfoMapper;
        _eventDispatcher = eventDispatcher;
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
    }

    public async Task<PhaseEntity> GetById(PhaseId phaseId, AllocationId allocationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var allocation = await _allocationCrmContext.GetById(allocationId.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);
        var phase = allocation.ListOfPhaseClaims.Find(x => x.Id == phaseId.ToGuidAsString())
                    ?? throw new NotFoundException(nameof(PhaseEntity), phaseId);
        var allocationBasicInfo = await _allocationBasicInfoMapper.Map(allocation, cancellationToken);

        return _phaseCrmMapper.MapToDomain(phase, allocationBasicInfo, organisation, _onlyCompletionMilestonePolicy);
    }

    public async Task Save(PhaseEntity phaseEntity, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!phaseEntity.IsModified)
        {
            return;
        }

        var dto = _phaseCrmMapper.MapToDto(phaseEntity);
        await _allocationCrmContext.Save(
            phaseEntity.Allocation.Id.Value,
            dto,
            userAccount.SelectedOrganisationId().Value,
            userAccount.UserGlobalId.Value,
            cancellationToken);
        await _eventDispatcher.Publish(phaseEntity, cancellationToken);
    }
}
