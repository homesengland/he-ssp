using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Delivery.Strategies;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Crm;

public interface IPhaseCrmMapper
{
    PhaseEntity MapToDomain(PhaseClaimsDto dto, AllocationBasicInfo allocation, bool isUnregisteredBody, IMilestoneAvailabilityStrategy strategy);

    PhaseClaimsDto MapToDto(PhaseEntity entity);
}
