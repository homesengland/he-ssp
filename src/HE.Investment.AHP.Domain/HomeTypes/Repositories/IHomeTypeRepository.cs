using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypeRepository
{
    Task<HomeTypesEntity> GetByApplicationId(
        ApplicationId applicationId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);

    Task<IHomeTypeEntity> GetById(
        ApplicationId applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);

    Task<IHomeTypeEntity> Save(
        ApplicationId applicationId,
        IHomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);
}
