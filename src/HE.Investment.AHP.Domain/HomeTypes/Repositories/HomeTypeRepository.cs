using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    public Task<HomeTypeEntity> GetById(
        string financialSchemeId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSectionType> sectionTypes,
        CancellationToken cancellationToken)
    {
        var homeType = Get(financialSchemeId, homeTypeId);
        if (homeType != null)
        {
            return Task.FromResult(homeType);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public Task<HomeTypeEntity> Save(
        string financialSchemeId,
        HomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSectionType> sectionTypes,
        CancellationToken cancellationToken)
    {
        if (homeType.IsNew)
        {
            homeType.Id = new HomeTypeId(Guid.NewGuid().ToString("D"));
        }

        // TODO: update fields in CRM depending on SectionTypes
        Save(financialSchemeId, homeType);
        return Task.FromResult(homeType);
    }

    private HomeTypeEntity? Get(string financialSchemeId, HomeTypeId homeTypeId)
    {
        if (HomeTypesRepository.HomeTypes.TryGetValue(financialSchemeId, out var homeTypes))
        {
            return homeTypes.FirstOrDefault(x => x.Id == homeTypeId);
        }

        return null;
    }

    private void Save(string financialSchemeId, HomeTypeEntity entity)
    {
        if (HomeTypesRepository.HomeTypes.TryGetValue(financialSchemeId, out var homeTypes))
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
            HomeTypesRepository.HomeTypes.Add(financialSchemeId, new List<HomeTypeEntity> { entity });
        }
    }
}
