using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.HomeTypes.Repositories;

public class HomeTypeRepository : IHomeTypeRepository
{
    private readonly IApplicationRepository _applicationRepository;

    private readonly IApplicationSectionStatusChanger _sectionStatusChanger;

    private readonly ISiteRepository _siteRepository;

    private readonly IAhpFileService<DesignFileParams> _designFileService;

    private readonly IHomeTypeCrmContext _homeTypeCrmContext;

    private readonly IHomeTypeCrmMapper _homeTypeCrmMapper;

    private readonly IEventDispatcher _eventDispatcher;

    public HomeTypeRepository(
        IApplicationRepository applicationRepository,
        IApplicationSectionStatusChanger sectionStatusChanger,
        ISiteRepository siteRepository,
        IAhpFileService<DesignFileParams> designFileService,
        IHomeTypeCrmContext homeTypeCrmContext,
        IHomeTypeCrmMapper homeTypeCrmMapper,
        IEventDispatcher eventDispatcher)
    {
        _applicationRepository = applicationRepository;
        _sectionStatusChanger = sectionStatusChanger;
        _siteRepository = siteRepository;
        _designFileService = designFileService;
        _homeTypeCrmContext = homeTypeCrmContext;
        _homeTypeCrmMapper = homeTypeCrmMapper;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<HomeTypesEntity> GetByApplicationId(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var site = await _siteRepository.GetSiteBasicInfo(application.SiteId, userAccount, cancellationToken);
        var homeTypes = userAccount.CanViewAllApplications()
            ? await _homeTypeCrmContext.GetAllOrganisationHomeTypes(applicationId.Value, organisationId, cancellationToken)
            : await _homeTypeCrmContext.GetAllUserHomeTypes(applicationId.Value, organisationId, cancellationToken);

        return new HomeTypesEntity(
            application,
            site,
            homeTypes.Select(x => _homeTypeCrmMapper.MapToDomain(application, site, x, new Dictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>>())),
            application.Sections.HomeTypesStatus);
    }

    public async Task<IHomeTypeEntity> GetById(
        AhpApplicationId applicationId,
        HomeTypeId homeTypeId,
        UserAccount userAccount,
        CancellationToken cancellationToken,
        bool loadFiles = false)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var site = await _siteRepository.GetSiteBasicInfo(application.SiteId, userAccount, cancellationToken);
        var homeType = userAccount.CanViewAllApplications()
            ? await _homeTypeCrmContext.GetOrganisationHomeTypeById(applicationId.Value, homeTypeId.Value, organisationId, cancellationToken)
            : await _homeTypeCrmContext.GetUserHomeTypeById(applicationId.Value, homeTypeId.Value, organisationId, cancellationToken);

        var uploadedFiles = await GetUploadedFiles(applicationId, homeTypeId, loadFiles, cancellationToken);
        if (homeType != null)
        {
            return _homeTypeCrmMapper.MapToDomain(application, site, homeType, uploadedFiles);
        }

        throw new NotFoundException(nameof(HomeTypeEntity), homeTypeId);
    }

    public async Task<IHomeTypeEntity> Save(
        IHomeTypeEntity homeType,
        OrganisationId organisationId,
        CancellationToken cancellationToken)
    {
        var entity = (HomeTypeEntity)homeType;
        if (entity.IsNew)
        {
            entity.Id = HomeTypeId.From(await _homeTypeCrmContext.Save(_homeTypeCrmMapper.MapToDto(entity), organisationId.Value, cancellationToken));
            await _eventDispatcher.Publish(
                new HomeTypeHasBeenCreatedEvent(homeType.Application.Id, entity.Id, entity.Name.Value),
                cancellationToken);
        }
        else if (entity.IsModified)
        {
            await _homeTypeCrmContext.Save(_homeTypeCrmMapper.MapToDto(entity), organisationId.Value, cancellationToken);
            await _eventDispatcher.Publish(entity, cancellationToken);
            await _eventDispatcher.Publish(new HomeTypeHasBeenUpdatedEvent(homeType.Application.Id, entity.Id), cancellationToken);
        }

        if (entity.DesignPlans.IsModified)
        {
            await homeType.DesignPlans.SaveFileChanges(homeType, _designFileService, cancellationToken);
        }

        return homeType;
    }

    public async Task Save(HomeTypesEntity homeTypes, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (homeTypes.IsStatusChanged)
        {
            await _sectionStatusChanger.ChangeSectionStatus(
                homeTypes.Application.Id,
                organisationId,
                SectionType.HomeTypes,
                homeTypes.Status,
                cancellationToken);
        }

        var homeTypeToRemove = homeTypes.PopRemovedHomeType();
        while (homeTypeToRemove != null)
        {
            await _homeTypeCrmContext.Remove(homeTypes.Application.Id.Value, homeTypeToRemove.Id.Value, organisationId.Value, cancellationToken);
            await _eventDispatcher.Publish(
                new HomeTypeHasBeenRemovedEvent(homeTypeToRemove.Application.Id, homeTypeToRemove.Id),
                cancellationToken);

            homeTypeToRemove = homeTypes.PopRemovedHomeType();
        }
    }

    private async Task<IDictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>>> GetUploadedFiles(
        AhpApplicationId applicationId,
        HomeTypeId homeTypeId,
        bool loadFiles,
        CancellationToken cancellationToken)
    {
        var uploadedFiles = new Dictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>>();
        if (loadFiles)
        {
            var designFiles = await _designFileService.GetFiles(new DesignFileParams(applicationId, homeTypeId), cancellationToken);
            uploadedFiles.Add(HomeTypeSegmentType.DesignPlans, designFiles);
        }

        return uploadedFiles;
    }
}
