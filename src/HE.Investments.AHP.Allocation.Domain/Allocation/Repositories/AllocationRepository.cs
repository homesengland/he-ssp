using HE.Common.IntegrationModel.PortalIntegrationModel;
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
using AhpProgramme = HE.Investments.Programme.Contract.Programme;
using AllocationBasicInfo = HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;

public class AllocationRepository : IAllocationRepository
{
    private readonly IAllocationCrmContext _allocationCrmContext;

    private readonly IMediator _mediator;

    private readonly IPhaseCrmMapper _phaseCrmMapper;

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public AllocationRepository(
        IAllocationCrmContext allocationCrmContext,
        IMediator mediator,
        IPhaseCrmMapper phaseCrmMapper,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy)
    {
        _allocationCrmContext = allocationCrmContext;
        _mediator = mediator;
        _phaseCrmMapper = phaseCrmMapper;
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
    }

    public async Task<AllocationEntity> GetById(AllocationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.ToGuidAsString();
        var allocation = await _allocationCrmContext.GetById(id.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);

        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocation.ProgrammeId)), cancellationToken);

        return CreateEntity(allocation, programme, organisation);
    }

    public async Task<AllocationOverview> GetOverview(AllocationId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        // TODO: Change method after #103595
        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        var allocationDto = await _allocationCrmContext.GetById(id.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);

        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(allocationDto.ProgrammeId)), cancellationToken);
        var allocationBasicInfo = new Contract.AllocationBasicInfo(
            AllocationId.From(allocationDto.Id),
            allocationDto.Name,
            allocationDto.ReferenceNumber,
            allocationDto.LocalAuthority.name,
            programme.ShortName,
            AllocationTenureMapper.ToDomain(allocationDto.Tenure).Value);

        return new AllocationOverview(
            allocationBasicInfo,
            new ModificationDetails("Carq", "Power", DateTime.Now),
            true,
            userAccount.SelectedOrganisation().RegisteredCompanyName,
            string.Empty,
            false);
    }

    private AllocationEntity CreateEntity(AllocationClaimsDto allocationDto, AhpProgramme programme, OrganisationBasicInfo organisation)
    {
        var allocationId = AllocationId.From(allocationDto.Id);
        var tenure = AllocationTenureMapper.ToDomain(allocationDto.Tenure);
        var allocationBasicInfo = new AllocationBasicInfo(
            allocationId,
            allocationDto.Name,
            allocationDto.ReferenceNumber,
            allocationDto.LocalAuthority.name,
            programme,
            tenure.Value);

        return new AllocationEntity(
            allocationId,
            new AllocationName(allocationDto.Name),
            new AllocationReferenceNumber(allocationDto.ReferenceNumber),
            new LocalAuthority(allocationDto.LocalAuthority.code, allocationDto.LocalAuthority.name),
            programme,
            tenure,
            new GrantDetails(allocationDto.GrantDetails.TotalGrantAllocated, allocationDto.GrantDetails.AmountPaid, allocationDto.GrantDetails.AmountRemaining),
            allocationDto.ListOfPhaseClaims
                .Select(x => _phaseCrmMapper.MapToDomain(x, allocationBasicInfo, organisation, _onlyCompletionMilestonePolicy))
                .ToList());
    }
}
