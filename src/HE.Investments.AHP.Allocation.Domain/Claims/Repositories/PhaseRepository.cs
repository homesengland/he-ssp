using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Allocation.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Repositories;

public class PhaseRepository : IPhaseRepository
{
    private readonly IAllocationCrmContext _allocationCrmContext;

    private readonly IPhaseCrmMapper _phaseCrmMapper;

    public PhaseRepository(
        IAllocationCrmContext allocationCrmContext,
        IPhaseCrmMapper phaseCrmMapper)
    {
        _allocationCrmContext = allocationCrmContext;
        _phaseCrmMapper = phaseCrmMapper;
    }

    public async Task<PhaseEntity> GetById(PhaseId phaseId, AllocationId allocationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var allocation = await _allocationCrmContext.GetById(allocationId.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);
        var phase = allocation.ListOfPhaseClaims.FirstOrDefault(); // TODO: AB#89858 Find phase by Id when CRM not mocked

        if (phase != null)
        {
            return _phaseCrmMapper.MapToDomain(phase);
        }

        throw new NotFoundException(nameof(PhaseEntity), phaseId);
    }

    public Task Save(PhaseEntity phaseEntity, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!phaseEntity.IsModified)
        {
            return Task.CompletedTask;
        }

        throw new NotImplementedException(); // todo implement when save milestones will be added
    }
}
