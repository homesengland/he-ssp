using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypeRepository
{
    Task<HomeTypesEntity> GetByApplicationId(
        string applicationId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);

    Task<IHomeTypeEntity> GetById(
        string applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);

    Task<IHomeTypeEntity> Save(
        string applicationId,
        IHomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);
}
