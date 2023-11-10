using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    private static readonly IDictionary<string, IList<HomeTypeEntity>> HomeTypes = new ConcurrentDictionary<string, IList<HomeTypeEntity>>();

    private readonly IApplicationRepository _applicationRepository;

    private readonly IDesignFileService _designFileService;

    public HomeTypeRepository(IApplicationRepository applicationRepository, IDesignFileService designFileService)
    {
        _applicationRepository = applicationRepository;
        _designFileService = designFileService;
    }

    public async Task<HomeTypesEntity> GetByApplicationId(
        ApplicationId applicationId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, cancellationToken);
        if (!HomeTypes.TryGetValue(applicationId.Value, out var homeTypes))
        {
            return new HomeTypesEntity(application, Enumerable.Empty<HomeTypeEntity>());
        }

        return new HomeTypesEntity(application, homeTypes);
    }

    public Task<IHomeTypeEntity> GetById(
        ApplicationId applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var homeType = Get(applicationId.Value, homeTypeId);
        if (homeType != null)
        {
            return Task.FromResult<IHomeTypeEntity>(homeType);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public async Task<IHomeTypeEntity> Save(
        ApplicationId applicationId,
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
        Save(applicationId.Value, entity);

        if (segments.Contains(HomeTypeSegmentType.DesignPlans))
        {
            await homeType.DesignPlans.SaveFileChanges(homeType, _designFileService, cancellationToken);
        }

        return homeType;
    }

    private HomeTypeEntity? Get(string applicationId, HomeTypeId homeTypeId)
    {
        if (HomeTypes.TryGetValue(applicationId, out var homeTypes))
        {
            var homeType = homeTypes.FirstOrDefault(x => x.Id == homeTypeId);

            // TODO: this is temporary change while entities are stored in memory
            // remove DiscardFileChanges function after integration with CRM
            homeType?.DesignPlans.DiscardFileChanges();
            return homeType;
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
