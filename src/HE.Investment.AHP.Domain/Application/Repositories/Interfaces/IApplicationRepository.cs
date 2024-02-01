using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.Repositories.Interfaces;

public interface IApplicationRepository : IApplicationWithdraw, IApplicationHold, IApplicationSubmit
{
    Task<ApplicationEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsNameExist(ApplicationName applicationName, OrganisationId organisationId, CancellationToken cancellationToken);

    Task<bool> IsExist(AhpApplicationId applicationId, OrganisationId organisationId, CancellationToken cancellationToken);

    Task<ApplicationBasicInfo> GetApplicationBasicInfo(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<ApplicationWithFundingDetails>> GetApplicationsWithFundingDetails(
        UserAccount userAccount,
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken);

    Task<ApplicationEntity> Save(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken);

    Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken);
}
