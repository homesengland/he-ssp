using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    public Task<HomeTypeEntity> GetById(
        string applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var homeType = Get(applicationId, homeTypeId);
        if (homeType != null)
        {
            return Task.FromResult(homeType);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public Task<HomeTypeEntity> Save(
        string applicationId,
        HomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        if (homeType.IsNew)
        {
            homeType.Id = new HomeTypeId(Guid.NewGuid().ToString("D"));
        }

        // TODO: update fields in CRM depending on SegmentTypes
        Save(applicationId, homeType);
        return Task.FromResult(homeType);
    }

    private HomeTypeEntity? Get(string applicationId, HomeTypeId homeTypeId)
    {
        if (HomeTypesRepository.HomeTypes.TryGetValue(applicationId, out var homeTypes))
        {
            return homeTypes.FirstOrDefault(x => x.Id == homeTypeId);
        }

        return null;
    }

    private void Save(string applicationId, HomeTypeEntity entity)
    {
        if (HomeTypesRepository.HomeTypes.TryGetValue(applicationId, out var homeTypes))
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
            HomeTypesRepository.HomeTypes.Add(applicationId, new List<HomeTypeEntity> { entity });
        }
    }
}
