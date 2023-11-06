using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    private static readonly IDictionary<string, IList<HomeTypeEntity>> HomeTypes = new ConcurrentDictionary<string, IList<HomeTypeEntity>>();

    public Task<HomeTypesEntity> GetByApplicationId(
        string applicationId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        if (!HomeTypes.TryGetValue(applicationId, out var homeTypes))
        {
            return Task.FromResult(new HomeTypesEntity(Enumerable.Empty<HomeTypeEntity>()));
        }

        return Task.FromResult(new HomeTypesEntity(homeTypes));
    }

    public Task<IHomeTypeEntity> GetById(
        string applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var homeType = Get(applicationId, homeTypeId);
        if (homeType != null)
        {
            return Task.FromResult<IHomeTypeEntity>(homeType);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public Task<IHomeTypeEntity> Save(
        string applicationId,
        IHomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var entity = (HomeTypeEntity)homeType;
        if (entity.IsNew)
        {
            entity.Id = new HomeTypeId(Guid.NewGuid().ToString("D"));
        }

        // TODO: update fields in CRM depending on SegmentTypes
        Save(applicationId, entity);
        return Task.FromResult(homeType);
    }

    private HomeTypeEntity? Get(string applicationId, HomeTypeId homeTypeId)
    {
        if (HomeTypes.TryGetValue(applicationId, out var homeTypes))
        {
            return homeTypes.FirstOrDefault(x => x.Id == homeTypeId);
        }

        return null;
    }

    private void Save(string applicationId, HomeTypeEntity entity)
    {
        if (HomeTypes.TryGetValue(applicationId, out var homeTypes))
        {
            var existingHomeType = homeTypes.FirstOrDefault(x => x.Id == entity.Id);
            if (existingHomeType != null)
            {
                homeTypes.Remove(existingHomeType);
            }

            homeTypes.Add(entity);
        }
        else
        {
            HomeTypes.Add(applicationId, new List<HomeTypeEntity> { entity });
        }
    }
}
