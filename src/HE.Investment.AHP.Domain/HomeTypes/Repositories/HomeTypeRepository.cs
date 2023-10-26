using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    public Task<HomeTypeEntity> GetById(
        string schemeId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSectionType> sectionTypes,
        CancellationToken cancellationToken)
    {
        var homeType = Get(schemeId, homeTypeId);
        if (homeType != null)
        {
            return Task.FromResult(homeType);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public Task<HomeTypeEntity> Save(
        string schemeId,
        HomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSectionType> sectionTypes,
        CancellationToken cancellationToken)
    {
        if (homeType.IsNew)
        {
            homeType.Id = new HomeTypeId(Guid.NewGuid().ToString("D"));
        }

        // TODO: update fields in CRM depending on SectionTypes
        Save(schemeId, homeType);
        return Task.FromResult(homeType);
    }

    private HomeTypeEntity? Get(string schemeId, HomeTypeId homeTypeId)
    {
        if (HomeTypesRepository.HomeTypes.TryGetValue(schemeId, out var homeTypes))
        {
            return homeTypes.FirstOrDefault(x => x.Id == homeTypeId);
        }

        return null;
    }

    private void Save(string schemeId, HomeTypeEntity entity)
    {
        if (HomeTypesRepository.HomeTypes.TryGetValue(schemeId, out var homeTypes))
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
            HomeTypesRepository.HomeTypes.Add(schemeId, new List<HomeTypeEntity> { entity });
        }
    }
}
