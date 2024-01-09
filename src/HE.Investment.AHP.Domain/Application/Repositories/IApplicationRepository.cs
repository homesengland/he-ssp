using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Utils.Pagination;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public interface IApplicationRepository
{
    Task<ApplicationEntity> GetById(ApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsExist(ApplicationName applicationName, OrganisationId organisationId, CancellationToken cancellationToken);

    Task<ApplicationBasicInfo> GetApplicationBasicInfo(ApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<ApplicationWithFundingDetails>> GetApplicationsWithFundingDetails(
        UserAccount userAccount,
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken);

    Task<ApplicationEntity> Save(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken);
}
