using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Crm;

public interface IPhaseCrmMapper
{
    PhaseEntity MapToDomain(
        PhaseClaimsDto dto,
        AllocationBasicInfo allocation,
        OrganisationBasicInfo organisation,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy);

    PhaseClaimsDto MapToDto(PhaseEntity entity);
}
