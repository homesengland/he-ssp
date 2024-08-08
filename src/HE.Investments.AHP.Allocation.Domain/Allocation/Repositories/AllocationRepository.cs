using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Overview;
using HE.Investments.AHP.Allocation.Domain.Allocation.Crm;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;

public class AllocationRepository : IAllocationRepository
{
    private readonly IAllocationCrmContext _allocationCrmContext;

    private readonly IMediator _mediator;

    private readonly IPhaseCrmMapper _phaseCrmMapper;

    private readonly IAllocationBasicInfoMapper _allocationBasicInfoMapper;

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public AllocationRepository(
        IAllocationCrmContext allocationCrmContext,
        IMediator mediator,
        IPhaseCrmMapper phaseCrmMapper,
        IAllocationBasicInfoMapper allocationBasicInfoMapper,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy)
    {
        _allocationCrmContext = allocationCrmContext;
        _mediator = mediator;
        _phaseCrmMapper = phaseCrmMapper;
        _allocationBasicInfoMapper = allocationBasicInfoMapper;
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
    }

    public async Task<AllocationEntity> GetById(AllocationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.ToGuidAsString();
        var allocation = await _allocationCrmContext.GetAllocationClaims(id.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);

        return await CreateEntity(allocation, organisation, cancellationToken);
    }

    public async Task<AllocationOverview> GetOverview(AllocationId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        var allocationDto = await _allocationCrmContext.GetAllocation(
            id.ToGuidAsString(),
            userAccount.SelectedOrganisationId().ToGuidAsString(),
            userAccount.UserGlobalId.ToString(),
            cancellationToken);
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocationDto.ProgrammeId)), cancellationToken);
        var allocationBasicInfo = new Contract.AllocationBasicInfo(
            AllocationId.From(allocationDto.Id),
            allocationDto.Name,
            allocationDto.ReferenceNumber,
            allocationDto.LocalAuthority.name,
            programme.ShortName,
            ApplicationTenureMapper.ToDomain(allocationDto.Tenure)!.Value);

        return new AllocationOverview(
            allocationBasicInfo,
            new ModificationDetails(
                allocationDto.LastExternalModificationBy?.firstName,
                allocationDto.LastExternalModificationBy?.lastName,
                allocationDto.LastExternalModificationOn),
            true, // TODO: AB#104786 use allocationDto.IsInContract when flag will be set in Integration Tests
            allocationDto.OrganisationName,
            allocationDto.FDProjectId,
            allocationDto.HasDraftAllocation);
    }

    private async Task<AllocationEntity> CreateEntity(AllocationClaimsDto allocationDto, OrganisationBasicInfo organisation, CancellationToken cancellationToken)
    {
        var allocationBasicInfo = await _allocationBasicInfoMapper.Map(allocationDto, cancellationToken);

        return new AllocationEntity(
            allocationBasicInfo,
            new GrantDetails(allocationDto.GrantDetails.TotalGrantAllocated, allocationDto.GrantDetails.AmountPaid, allocationDto.GrantDetails.AmountRemaining),
            allocationDto.ListOfPhaseClaims
                .Select(x => _phaseCrmMapper.MapToDomain(x, allocationBasicInfo, organisation, _onlyCompletionMilestonePolicy))
                .ToList());
    }
}
