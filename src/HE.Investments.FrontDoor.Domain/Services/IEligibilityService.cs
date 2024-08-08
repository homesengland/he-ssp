using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Domain.Project;
using HE.UtilsService.BannerNotification.Shared;

namespace HE.Investments.FrontDoor.Domain.Services;

public interface IEligibilityService
{
    public Task<(OperationResult OperationResult, ApplicationType ApplicationType)> GetEligibleApplication(
        ProjectEntity project,
        CancellationToken cancellationToken);
}
