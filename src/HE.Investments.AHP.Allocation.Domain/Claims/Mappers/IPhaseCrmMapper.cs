using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IPhaseCrmMapper
{
    PhaseEntity MapToDomain(PhaseClaimsDto dto);

    PhaseClaimsDto MapToDto(PhaseEntity entity);
}
