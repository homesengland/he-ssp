using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Common.Utils.Pagination;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public interface IApplicationRepository
{
    Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken);

    Task<bool> IsExist(ApplicationName applicationName, CancellationToken cancellationToken);

    Task<ApplicationBasicInfo> GetApplicationBasicInfo(ApplicationId id, CancellationToken cancellationToken);

    Task<PaginationResult<ApplicationWithFundingDetails>> GetApplicationsWithFundingDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken);

    Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken);
}
