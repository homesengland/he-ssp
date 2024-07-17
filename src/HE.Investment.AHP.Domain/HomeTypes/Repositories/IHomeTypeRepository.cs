using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypeRepository
{
    Task<HomeTypesEntity> GetByApplicationId(
        AhpApplicationId applicationId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken);

    Task<IHomeTypeEntity> GetById(
        AhpApplicationId applicationId,
        HomeTypeId homeTypeId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken,
        bool loadFiles = false);

    Task<IHomeTypeEntity> Save(
        IHomeTypeEntity homeType,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task Save(HomeTypesEntity homeTypes, UserAccount userAccount, CancellationToken cancellationToken);
}
