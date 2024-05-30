using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public interface IApplicationRepository
{
    Task<ApplicationEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsNameExist(ApplicationName applicationName, OrganisationId organisationId, CancellationToken cancellationToken);

    Task<bool> IsExist(AhpApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ApplicationBasicInfo> GetApplicationBasicInfo(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ApplicationWithFundingDetails> GetApplicationWithFundingDetailsById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ApplicationEntity> Save(ApplicationEntity application, UserAccount userAccount, CancellationToken cancellationToken);

    Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken);
}
