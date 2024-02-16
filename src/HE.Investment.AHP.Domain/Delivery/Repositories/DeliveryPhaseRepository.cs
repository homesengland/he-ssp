using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public class DeliveryPhaseRepository : IDeliveryPhaseRepository
{
    private readonly IDeliveryPhaseCrmContext _crmContext;

    private readonly IDeliveryPhaseCrmMapper _crmMapper;

    private readonly IApplicationRepository _applicationRepository;

    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly ISchemeRepository _schemeRepository;

    private readonly IEventDispatcher _eventDispatcher;

    public DeliveryPhaseRepository(
        IDeliveryPhaseCrmContext crmContext,
        IDeliveryPhaseCrmMapper crmMapper,
        IApplicationRepository applicationRepository,
        IHomeTypeRepository homeTypeRepository,
        ISchemeRepository schemeRepository,
        IEventDispatcher eventDispatcher)
    {
        _applicationRepository = applicationRepository;
        _homeTypeRepository = homeTypeRepository;
        _eventDispatcher = eventDispatcher;
        _schemeRepository = schemeRepository;
        _crmContext = crmContext;
        _crmMapper = crmMapper;
    }

    public async Task<DeliveryPhasesEntity> GetByApplicationId(AhpApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var deliveryPhases = userAccount.CanViewAllApplications()
            ? await _crmContext.GetAllOrganisationDeliveryPhases(applicationId.Value, organisationId, _crmMapper.CrmFields, cancellationToken)
            : await _crmContext.GetAllUserDeliveryPhases(applicationId.Value, organisationId, _crmMapper.CrmFields, cancellationToken);
        var sectionStatus = await _crmContext.GetDeliveryStatus(applicationId.Value, organisationId, cancellationToken);
        var homesToDeliver = await GetHomesToDeliver(applicationId, userAccount, cancellationToken);
        var schemeFunding = await GetSchemeFunding(applicationId, userAccount, cancellationToken);

        return new DeliveryPhasesEntity(
            application,
            deliveryPhases.Select(x => _crmMapper.MapToDomain(application, organisation, x, schemeFunding)).ToList(),
            homesToDeliver,
            SectionStatusMapper.ToDomain(sectionStatus, application.Status));
    }

    public async Task<IDeliveryPhaseEntity> GetById(
        AhpApplicationId applicationId,
        DeliveryPhaseId deliveryPhaseId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisation = userAccount.SelectedOrganisation();
        var organisationId = organisation.OrganisationId.Value;
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var deliveryPhase = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationDeliveryPhaseById(applicationId.Value, deliveryPhaseId.Value, organisationId, _crmMapper.CrmFields, cancellationToken)
            : await _crmContext.GetUserDeliveryPhaseById(applicationId.Value, deliveryPhaseId.Value, organisationId, _crmMapper.CrmFields, cancellationToken);
        var schemeFunding = await GetSchemeFunding(applicationId, userAccount, cancellationToken);

        if (deliveryPhase != null)
        {
            return _crmMapper.MapToDomain(application, organisation, deliveryPhase, schemeFunding);
        }

        throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);
    }

    public async Task<DeliveryPhaseId> Save(IDeliveryPhaseEntity deliveryPhase, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var entity = (DeliveryPhaseEntity)deliveryPhase;
        if (entity.IsNew)
        {
            entity.Id = new DeliveryPhaseId(await _crmContext.Save(_crmMapper.MapToDto(entity), organisationId.Value, _crmMapper.CrmFields, cancellationToken));
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenCreatedEvent(entity.Application.Id, entity.Name.Value), cancellationToken);
        }
        else if (entity.IsModified)
        {
            await _crmContext.Save(_crmMapper.MapToDto(entity), organisationId.Value, _crmMapper.CrmFields, cancellationToken);
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenUpdatedEvent(entity.Application.Id), cancellationToken);
        }

        return entity.Id;
    }

    public async Task Save(DeliveryPhasesEntity deliveryPhases, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (deliveryPhases.IsStatusChanged)
        {
            await _crmContext.SaveDeliveryStatus(
                deliveryPhases.ApplicationId.Value,
                organisationId.Value,
                SectionStatusMapper.ToDto(deliveryPhases.Status),
                cancellationToken);
        }

        foreach (var deliveryPhase in deliveryPhases.DeliveryPhases.Where(x => x.IsModified))
        {
            await Save(deliveryPhase, organisationId, cancellationToken);
        }

        var deliveryPhaseToRemove = deliveryPhases.PopRemovedDeliveryPhase();
        while (deliveryPhaseToRemove != null)
        {
            await _crmContext.Remove(deliveryPhases.ApplicationId.Value, deliveryPhaseToRemove.Id.Value, organisationId.Value, cancellationToken);
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenRemovedEvent(deliveryPhaseToRemove.Application.Id), cancellationToken);

            deliveryPhaseToRemove = deliveryPhases.PopRemovedDeliveryPhase();
        }
    }

    private async Task<IEnumerable<HomesToDeliver>> GetHomesToDeliver(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var homeTypes = await _homeTypeRepository.GetByApplicationId(
            applicationId,
            userAccount,
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);

        return homeTypes.HomeTypes.Select(x => new HomesToDeliver(x.Id, x.Name, x.HomeInformation.NumberOfHomes?.Value ?? 0));
    }

    private async Task<SchemeFunding> GetSchemeFunding(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        return (await _schemeRepository.GetByApplicationId(applicationId, userAccount, false, cancellationToken)).Funding;
    }
}
