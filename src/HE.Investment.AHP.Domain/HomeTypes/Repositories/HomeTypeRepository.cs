using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    private readonly IApplicationRepository _applicationRepository;

    private readonly IAhpFileService<DesignFileParams> _designFileService;

    private readonly IHomeTypeCrmContext _homeTypeCrmContext;

    private readonly IHomeTypeCrmMapper _homeTypeCrmMapper;

    private readonly IEventDispatcher _eventDispatcher;

    public HomeTypeRepository(
        IApplicationRepository applicationRepository,
        IAhpFileService<DesignFileParams> designFileService,
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
        AhpApplicationId applicationId,
        UserAccount userAccount,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var homeTypes = userAccount.CanViewAllApplications()
            ? await _homeTypeCrmContext.GetAllOrganisationHomeTypes(applicationId.Value, organisationId, _homeTypeCrmMapper.GetCrmFields(segments), cancellationToken)
            : await _homeTypeCrmContext.GetAllUserHomeTypes(applicationId.Value, organisationId, _homeTypeCrmMapper.GetCrmFields(segments), cancellationToken);
        var sectionStatus = await _homeTypeCrmContext.GetHomeTypesStatus(applicationId.Value, organisationId, cancellationToken);

        return new HomeTypesEntity(
            application,
            homeTypes.Select(x => _homeTypeCrmMapper.MapToDomain(application, x, segments, new Dictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>>())),
            SectionStatusMapper.ToDomain(sectionStatus));
    }

    public async Task<IHomeTypeEntity> GetById(
        AhpApplicationId applicationId,
        HomeTypeId homeTypeId,
        UserAccount userAccount,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var homeType = userAccount.CanViewAllApplications()
            ? await _homeTypeCrmContext.GetOrganisationHomeTypeById(applicationId.Value, homeTypeId.Value, organisationId, _homeTypeCrmMapper.GetCrmFields(segments), cancellationToken)
            : await _homeTypeCrmContext.GetUserHomeTypeById(applicationId.Value, homeTypeId.Value, organisationId, _homeTypeCrmMapper.GetCrmFields(segments), cancellationToken);

        var uploadedFiles = await GetUploadedFiles(applicationId, homeTypeId, segments, cancellationToken);
        if (homeType != null)
        {
            return _homeTypeCrmMapper.MapToDomain(application, homeType, segments, uploadedFiles);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public async Task<IHomeTypeEntity> Save(
        IHomeTypeEntity homeType,
        OrganisationId organisationId,
        IReadOnlyCollection<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var entity = (HomeTypeEntity)homeType;
        if (entity.IsNew)
        {
            entity.Id = new HomeTypeId(
                await _homeTypeCrmContext.Save(
                    _homeTypeCrmMapper.MapToDto(entity, segments),
                    organisationId.Value,
                    _homeTypeCrmMapper.SaveCrmFields(entity, segments),
                    cancellationToken));
            await _eventDispatcher.Publish(
                new HomeTypeHasBeenCreatedEvent(homeType.Application.Id, entity.Id, entity.Name.Value),
                cancellationToken);
        }
        else if (entity.IsModified)
        {
            await _homeTypeCrmContext.Save(
                _homeTypeCrmMapper.MapToDto(entity, segments),
                organisationId.Value,
                _homeTypeCrmMapper.SaveCrmFields(entity, segments),
                cancellationToken);
            await _eventDispatcher.Publish(new HomeTypeHasBeenUpdatedEvent(homeType.Application.Id, entity.Id), cancellationToken);
        }

        if (segments.Contains(HomeTypeSegmentType.DesignPlans)
            && entity.HasSegment(HomeTypeSegmentType.DesignPlans)
            && entity.DesignPlans.IsModified)
        {
            await homeType.DesignPlans.SaveFileChanges(homeType, _designFileService, cancellationToken);
        }

        return homeType;
    }

    public async Task Save(HomeTypesEntity homeTypes, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (homeTypes.IsStatusChanged)
        {
            await _homeTypeCrmContext.SaveHomeTypesStatus(homeTypes.ApplicationId.Value, organisationId.Value, SectionStatusMapper.ToDto(homeTypes.Status), cancellationToken);
        }

        foreach (var homeTypeToRemove in homeTypes.ToRemove)
        {
            await _homeTypeCrmContext.Remove(homeTypes.ApplicationId.Value, homeTypeToRemove.Id.Value, organisationId.Value, cancellationToken);
            await _eventDispatcher.Publish(
                new HomeTypeHasBeenRemovedEvent(homeTypeToRemove.Application.Id, homeTypeToRemove.Id),
                cancellationToken);
        }
    }

    private async Task<IDictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>>> GetUploadedFiles(
        AhpApplicationId applicationId,
        HomeTypeId homeTypeId,
        IEnumerable<HomeTypeSegmentType> segments,
        CancellationToken cancellationToken)
    {
        var uploadedFiles = new Dictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>>();
        if (segments.Contains(HomeTypeSegmentType.DesignPlans))
        {
            var designFiles = await _designFileService.GetFiles(new DesignFileParams(applicationId, homeTypeId), cancellationToken);
            uploadedFiles.Add(HomeTypeSegmentType.DesignPlans, designFiles);
        }

        return uploadedFiles;
    }
}
