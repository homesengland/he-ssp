using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypeRepository
{
    Task<HomeTypesEntity> GetByApplicationId(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task<IHomeTypeEntity> GetById(
        AhpApplicationId applicationId,
        HomeTypeId homeTypeId,
        UserAccount userAccount,
        CancellationToken cancellationToken,
        bool loadFiles = false);

    Task<IHomeTypeEntity> Save(
        IHomeTypeEntity homeType,
        OrganisationId organisationId,
        CancellationToken cancellationToken);

    Task Save(HomeTypesEntity homeTypes, OrganisationId organisationId, CancellationToken cancellationToken);
}
