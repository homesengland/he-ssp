using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;

public interface IAllocationBasicInfoMapper
{
    Task<AllocationBasicInfo> Map(AhpAllocationDto allocation, CancellationToken cancellationToken);
}
