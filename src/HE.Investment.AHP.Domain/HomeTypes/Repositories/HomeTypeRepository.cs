using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Common.Infrastructure.Events;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    private readonly IApplicationRepository _applicationRepository;

    private readonly IDesignFileService _designFileService;

    private readonly IHomeTypeCrmContext _homeTypeCrmContext;

    private readonly IHomeTypeCrmMapper _homeTypeCrmMapper;

    private readonly IEventDispatcher _eventDispatcher;

    public HomeTypeRepository(
        IApplicationRepository applicationRepository,
        IDesignFileService designFileService,
        IHomeTypeCrmContext homeTypeCrmContext,
        IHomeTypeCrmMapper homeTypeCrmMapper,
        IEventDispatcher eventDispatcher)
    {
        _applicationRepository = applicationRepository;
        _designFileService = designFileService;
        _homeTypeCrmContext = homeTypeCrmContext;
        _homeTypeCrmMapper = homeTypeCrmMapper;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<HomeTypesEntity> GetByApplicationId(
        ApplicationId applicationId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, cancellationToken);
        var homeTypes = await _homeTypeCrmContext.GetAll(applicationId.Value, _homeTypeCrmMapper.GetCrmFields(segments), cancellationToken);
        var sectionStatus = await _homeTypeCrmContext.GetHomeTypesStatus(applicationId.Value, cancellationToken);

        return new HomeTypesEntity(
            application,
            homeTypes.Select(x => _homeTypeCrmMapper.MapToDomain(application, x, segments)),
            SectionStatusMapper.ToDomain(sectionStatus));
    }

    public async Task<IHomeTypeEntity> GetById(
        ApplicationId applicationId,
        HomeTypeId homeTypeId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, cancellationToken);
        var homeType = await _homeTypeCrmContext.GetById(applicationId.Value, homeTypeId.Value, _homeTypeCrmMapper.GetCrmFields(segments), cancellationToken);
        if (homeType != null)
        {
            return _homeTypeCrmMapper.MapToDomain(application, homeType, segments);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public async Task<IHomeTypeEntity> Save(
        IHomeTypeEntity homeType,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var entity = (HomeTypeEntity)homeType;
        if (entity.IsNew)
        {
            entity.Id = new HomeTypeId(
                await _homeTypeCrmContext.Save(
                    _homeTypeCrmMapper.MapToDto(entity, segments),
                    _homeTypeCrmMapper.SaveCrmFields(entity, segments),
                    cancellationToken));
            await _eventDispatcher.Publish(new HomeTypeHasBeenCreatedEvent(homeType.Application.Id.Value, entity.Name.Value), cancellationToken);
        }
        else if (entity.IsModified)
        {
            await _homeTypeCrmContext.Save(
                _homeTypeCrmMapper.MapToDto(entity, segments),
                _homeTypeCrmMapper.SaveCrmFields(entity, segments),
                cancellationToken);
            await _eventDispatcher.Publish(new HomeTypeHasBeenUpdatedEvent(homeType.Application.Id.Value, entity.Id!.Value), cancellationToken);
        }

        if (segments.Contains(HomeTypeSegmentType.DesignPlans)
            && entity.HasSegment(HomeTypeSegmentType.DesignPlans)
            && entity.DesignPlans.IsModified)
        {
            await homeType.DesignPlans.SaveFileChanges(homeType, _designFileService, cancellationToken);
        }

        return homeType;
    }

    public async Task Save(HomeTypesEntity homeTypes, CancellationToken cancellationToken)
    {
        if (homeTypes.IsStatusChanged)
        {
            await _homeTypeCrmContext.SaveHomeTypesStatus(homeTypes.ApplicationId.Value, SectionStatusMapper.ToDto(homeTypes.Status), cancellationToken);
        }

        foreach (var homeTypeToRemove in homeTypes.ToRemove)
        {
            await _homeTypeCrmContext.Remove(homeTypes.ApplicationId.Value, homeTypeToRemove.Id!.Value, cancellationToken);
            await _eventDispatcher.Publish(
                new HomeTypeHasBeenUpdatedEvent(homeTypeToRemove.Application.Id.Value, homeTypeToRemove.Id!.Value),
                cancellationToken);
        }
    }
}
