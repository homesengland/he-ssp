using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public class DeliveryPhaseRepository : IDeliveryPhaseRepository
{
    private readonly IDeliveryPhaseCrmContext _crmContext;

    private readonly IDeliveryPhaseCrmMapper _crmMapper;

    private readonly IApplicationRepository _applicationRepository;

    private readonly IApplicationSectionStatusChanger _sectionStatusChanger;

    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IEventDispatcher _eventDispatcher;

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public DeliveryPhaseRepository(
        IDeliveryPhaseCrmContext crmContext,
        IDeliveryPhaseCrmMapper crmMapper,
        IApplicationRepository applicationRepository,
        IHomeTypeRepository homeTypeRepository,
        IApplicationSectionStatusChanger sectionStatusChanger,
        IEventDispatcher eventDispatcher,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy)
    {
        _applicationRepository = applicationRepository;
        _homeTypeRepository = homeTypeRepository;
        _eventDispatcher = eventDispatcher;
        _sectionStatusChanger = sectionStatusChanger;
        _crmContext = crmContext;
        _crmMapper = crmMapper;
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
    }

    public async Task<DeliveryPhasesEntity> GetByApplicationId(
        AhpApplicationId applicationId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var deliveryPhases = userAccount.CanViewAllApplications()
            ? await _crmContext.GetAllOrganisationDeliveryPhases(applicationId.Value, organisationId, cancellationToken)
            : await _crmContext.GetAllUserDeliveryPhases(applicationId.Value, organisationId, userAccount.UserGlobalId.Value, cancellationToken);
        var homesToDeliver = await GetHomesToDeliver(applicationId, userAccount, cancellationToken);

        return new DeliveryPhasesEntity(
            application,
            deliveryPhases.Select(x => _crmMapper.MapToDomain(application, organisation, x, _onlyCompletionMilestonePolicy)).ToList(),
            homesToDeliver,
            application.Sections.DeliveryStatus);
    }

    public async Task<IDeliveryPhaseEntity> GetById(
        AhpApplicationId applicationId,
        DeliveryPhaseId deliveryPhaseId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var deliveryPhase = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationDeliveryPhaseById(applicationId.Value, deliveryPhaseId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserDeliveryPhaseById(
                applicationId.Value,
                deliveryPhaseId.Value,
                organisationId,
                userAccount.UserGlobalId.Value,
                cancellationToken);

        if (deliveryPhase != null)
        {
            return _crmMapper.MapToDomain(application, organisation, deliveryPhase, _onlyCompletionMilestonePolicy);
        }

        throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);
    }

    public async Task<DeliveryPhaseId> Save(IDeliveryPhaseEntity deliveryPhase, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var entity = (DeliveryPhaseEntity)deliveryPhase;
        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        if (entity.IsNew)
        {
            entity.Id = DeliveryPhaseId.From(await _crmContext.Save(
                _crmMapper.MapToDto(entity),
                organisationId,
                userAccount.UserGlobalId.Value,
                cancellationToken));
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenCreatedEvent(entity.Application.Id, entity.Name.Value), cancellationToken);
        }
        else if (entity.IsModified)
        {
            await _crmContext.Save(_crmMapper.MapToDto(entity), organisationId, userAccount.UserGlobalId.Value, cancellationToken);
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenUpdatedEvent(entity.Application.Id), cancellationToken);
        }

        return entity.Id;
    }

    public async Task Save(DeliveryPhasesEntity deliveryPhases, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        if (deliveryPhases.IsStatusChanged)
        {
            await _sectionStatusChanger.ChangeSectionStatus(
                deliveryPhases.Application.Id,
                userAccount,
                SectionType.DeliveryPhases,
                deliveryPhases.Status,
                cancellationToken);
        }

        foreach (var deliveryPhase in deliveryPhases.DeliveryPhases.Where(x => x.IsModified))
        {
            await Save(deliveryPhase, userAccount, cancellationToken);
        }

        var deliveryPhaseToRemove = deliveryPhases.PopRemovedDeliveryPhase();
        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        while (deliveryPhaseToRemove != null)
        {
            await _crmContext.Remove(
                deliveryPhases.Application.Id.Value,
                deliveryPhaseToRemove.Id.Value,
                organisationId,
                userAccount.UserGlobalId.Value,
                cancellationToken);
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenRemovedEvent(deliveryPhaseToRemove.Application.Id), cancellationToken);

            deliveryPhaseToRemove = deliveryPhases.PopRemovedDeliveryPhase();
        }
    }

    private async Task<IEnumerable<HomesToDeliver>> GetHomesToDeliver(
        AhpApplicationId applicationId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var homeTypes = await _homeTypeRepository.GetByApplicationId(
            applicationId,
            userAccount,
            cancellationToken);

        return homeTypes.HomeTypes.Select(x => new HomesToDeliver(x.Id, x.Name, x.HomeInformation.NumberOfHomes?.Value ?? 0));
    }
}
