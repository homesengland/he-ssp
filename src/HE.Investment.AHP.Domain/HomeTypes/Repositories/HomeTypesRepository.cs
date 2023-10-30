using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypesRepository : IHomeTypesRepository
{
    public static readonly IDictionary<string, IList<HomeTypeEntity>> HomeTypes = new ConcurrentDictionary<string, IList<HomeTypeEntity>>();

    public Task<HomeTypesEntity> GetByApplicationId(string applicationId, CancellationToken cancellationToken)
    {
        if (!HomeTypes.TryGetValue(applicationId, out var homeTypes))
        {
            return Task.FromResult(new HomeTypesEntity(Enumerable.Empty<HomeTypeBasicDetailsEntity>()));
        }

        return Task.FromResult(new HomeTypesEntity(homeTypes.Select(x =>
            new HomeTypeBasicDetailsEntity(x.Id!, x.Name, x.HousingType))));
    }
}
