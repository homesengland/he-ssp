using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.FinancialDetails.Mappers;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Programme.Contract.Config;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IProgrammeSettings _settings;

    private readonly IEventDispatcher _eventDispatcher;

    public ApplicationRepository(IApplicationCrmContext applicationCrmContext, IEventDispatcher eventDispatcher, IProgrammeSettings settings)
    {
        _applicationCrmContext = applicationCrmContext;
        _eventDispatcher = eventDispatcher;
        _settings = settings;
    }

    public async Task<ApplicationEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var application = await GetAhpApplicationDto(id, userAccount, cancellationToken);

        return CreateEntity(application, userAccount);
    }

    public async Task<bool> IsNameExist(ApplicationName applicationName, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        return await _applicationCrmContext.IsNameExist(applicationName.Value, organisationId.ToGuidAsString(), cancellationToken);
    }

    public async Task<bool> IsExist(AhpApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        try
        {
            var application = await _applicationCrmContext.GetUserApplicationById(
                applicationId.ToGuidAsString(),
                userAccount.SelectedOrganisationId().ToGuidAsString(),
                userAccount.UserGlobalId.ToString(),
                cancellationToken);

            return application.IsProvided();
        }
        catch (NotFoundException)
        {
            return false;
        }
    }

    public async Task<ApplicationBasicInfo> GetApplicationBasicInfo(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var application = await GetById(id, userAccount, cancellationToken);

        return new ApplicationBasicInfo(
            application.Id,
            application.SiteId,
            application.Name,
            application.Tenure.Value,
            application.Status,
            application.Sections,
            new ApplicationStateFactory(userAccount, wasSubmitted: application.LastSubmitted.IsProvided()));
    }

    public async Task<ApplicationWithFundingDetails> GetApplicationWithFundingDetailsById(
        AhpApplicationId id,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var application = await GetAhpApplicationDto(id, userAccount, cancellationToken);

        return CreateApplicationWithFundingDetails(application);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (application.IsStatusModified)
        {
            await _applicationCrmContext.ChangeApplicationStatus(
                application.Id.ToGuidAsString(),
                userAccount.SelectedOrganisationId().ToGuidAsString(),
                userAccount.UserGlobalId.ToString(),
                application.Status,
                application.ChangeStatusReason,
                application.RepresentationsAndWarranties.Value,
                cancellationToken);
        }

        if (application is { IsModified: false, IsNew: false })
        {
            return application;
        }

        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        var dto = application.Id.IsNew
            ? new AhpApplicationDto { programmeId = _settings.AhpProgrammeId }
            : await _applicationCrmContext.GetOrganisationApplicationById(application.Id.ToGuidAsString(), organisationId, cancellationToken);
        dto.name = application.Name.Value;
        dto.tenure = ApplicationTenureMapper.ToDto(application.Tenure);
        dto.organisationId = organisationId;
        dto.applicationStatus = AhpApplicationStatusMapper.MapToCrmStatus(application.Status);
        dto.siteId = application.SiteId.ToGuidAsString();
        dto.developingPartnerId = application.ApplicationPartners.DevelopingPartner.Id.ToGuidAsString();
        dto.ownerOfTheLandDuringDevelopmentId = application.ApplicationPartners.OwnerOfTheLand.Id.ToGuidAsString();
        dto.ownerOfTheHomesAfterCompletionId = application.ApplicationPartners.OwnerOfTheHomes.Id.ToGuidAsString();
        dto.applicationPartnerConfirmation = application.ApplicationPartners.ArePartnersConfirmed;

        var id = await _applicationCrmContext.Save(dto, organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);
        if (application.Id.IsNew)
        {
            var applicationId = AhpApplicationId.From(id);
            application.SetId(applicationId);

            await _eventDispatcher.Publish(new ApplicationHasBeenCreatedEvent(applicationId), cancellationToken);
        }

        return application;
    }

    public async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }

    private static ApplicationEntity CreateEntity(AhpApplicationDto application, UserAccount userAccount)
    {
        var applicationStatus = AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus);
        ApplicationStatus? previousStatus = application.previousExternalStatus.IsProvided()
            ? AhpApplicationStatusMapper.MapToPortalStatus(application.previousExternalStatus)
            : null;
        var lastSubmitted = application.dateSubmitted.IsProvided()
            ? new AuditEntry(
                application.lastExternalSubmittedBy?.firstName,
                application.lastExternalSubmittedBy?.lastName,
                application.dateSubmitted)
            : null;

        return new ApplicationEntity(
            FrontDoorProjectId.From(application.fdProjectId),
            string.IsNullOrEmpty(application.siteId) ? SiteId.New() : SiteId.From(application.siteId),
            AhpApplicationId.From(application.id),
            new ApplicationName(application.name ?? "Unknown"),
            applicationStatus,
            ApplicationTenureMapper.ToDomain(application.tenure)!,
            ApplicationPartnersMapper.ToDomain(application),
            new ApplicationStateFactory(userAccount, previousStatus, application.dateSubmitted.IsProvided()),
            new ApplicationReferenceNumber(application.referenceNumber),
            new ApplicationSections(
                [
                    new(SectionType.Scheme, SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus, applicationStatus)),
                    new(SectionType.HomeTypes, SectionStatusMapper.ToDomain(application.homeTypesSectionCompletionStatus, applicationStatus)),
                    new(SectionType.FinancialDetails, SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus, applicationStatus)),
                    new(SectionType.DeliveryPhases, SectionStatusMapper.ToDomain(application.deliveryPhasesSectionCompletionStatus, applicationStatus)),
                ]),
            new AuditEntry(
                application.lastExternalModificationBy?.firstName,
                application.lastExternalModificationBy?.lastName,
                application.lastExternalModificationOn),
            lastSubmitted,
            new RepresentationsAndWarranties(application.representationsandwarranties ?? false));
    }

    private static ApplicationWithFundingDetails CreateApplicationWithFundingDetails(AhpApplicationDto ahpApplicationDto)
    {
        var otherApplicationCosts = OtherApplicationCostsMapper.MapToOtherApplicationCosts(ahpApplicationDto);

        return new ApplicationWithFundingDetails(
            FrontDoorProjectId.From(ahpApplicationDto.fdProjectId),
            string.IsNullOrEmpty(ahpApplicationDto.siteId) ? SiteId.New() : SiteId.From(ahpApplicationDto.siteId),
            AhpApplicationId.From(ahpApplicationDto.id),
            ahpApplicationDto.name,
            ahpApplicationDto.referenceNumber,
            AhpApplicationStatusMapper.MapToPortalStatus(ahpApplicationDto.applicationStatus),
            ApplicationTenureMapper.ToDomain(ahpApplicationDto.tenure)!.Value,
            ahpApplicationDto.noOfHomes,
            ahpApplicationDto.fundingRequested,
            otherApplicationCosts.ExpectedTotalCosts(),
            ahpApplicationDto.currentLandValue,
            ahpApplicationDto.representationsandwarranties);
    }

    private async Task<AhpApplicationDto> GetAhpApplicationDto(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().ToGuidAsString();
        return userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplicationById(id.ToGuidAsString(), organisationId, cancellationToken)
            : await _applicationCrmContext.GetUserApplicationById(id.ToGuidAsString(), organisationId, userAccount.UserGlobalId.ToString(), cancellationToken);
    }
}
