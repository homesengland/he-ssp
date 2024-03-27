using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Services;

public interface IEligibilityService
{
    public Task<(OperationResult OperationResult, ApplicationType ApplicationType)> GetEligibleApplication(
        FrontDoorProjectId projectId,
        CancellationToken cancellationToken);
}
