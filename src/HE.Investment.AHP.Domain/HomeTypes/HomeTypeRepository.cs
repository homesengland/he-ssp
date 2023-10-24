using System.Collections.Concurrent;
using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;
using HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes;

public class HomeTypeRepository : IHomeTypeRepository
{
    private static readonly IDictionary<string, HomeTypeEntity> HomeTypes = new ConcurrentDictionary<string, HomeTypeEntity>();

    public Task<HomeTypeEntity> Get(
        HomeTypeId homeTypeId,
        HomeTypeSectionType sectionTypes,
        CancellationToken cancellationToken)
    {
        var homeType = Get(homeTypeId);
        if (homeType != null)
        {
            return Task.FromResult(homeType);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public Task<HomeTypeEntity> Save(
        string financialSchemeId,
        HomeTypeEntity homeType,
        HomeTypeSectionType sectionTypes,
        CancellationToken cancellationToken)
    {
        if (homeType.IsNew)
        {
            homeType.Id = new HomeTypeId(Guid.NewGuid().ToString("D"));
        }

        // TODO: update fields in CRM depending on SectionTypes
        Save(homeType);
        return Task.FromResult(homeType);
    }

    private HomeTypeEntity? Get(HomeTypeId homeTypeId)
    {
        if (HomeTypes.TryGetValue(homeTypeId.Value, out var homeTypeEntity))
        {
            return homeTypeEntity;
        }

        return null;
    }

    private void Save(HomeTypeEntity entity)
    {
        if (HomeTypes.ContainsKey(entity.Id!.Value))
        {
            HomeTypes.Remove(entity.Id!.Value);
        }

        HomeTypes[entity.Id!.Value] = entity;
    }
}
