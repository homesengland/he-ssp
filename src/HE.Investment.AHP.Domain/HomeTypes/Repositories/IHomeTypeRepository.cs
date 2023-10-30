using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public interface IHomeTypeRepository
{
    Task<HomeTypeEntity> GetById(
        string applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);

    Task<HomeTypeEntity> Save(
        string applicationId,
        HomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken);
}
