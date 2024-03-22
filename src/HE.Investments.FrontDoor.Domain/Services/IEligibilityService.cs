using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Services;

public interface IEligibilityService
{
    public Task<ApplicationType> GetEligibleApplication(FrontDoorProjectId projectId, CancellationToken cancellationToken);
}
