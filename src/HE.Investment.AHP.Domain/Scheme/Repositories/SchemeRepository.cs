extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public class SchemeRepository : ISchemeRepository
{
    private readonly IApplicationCrmContext _crmContext;

    private readonly IApplicationRepository _applicationRepository;

    private readonly IAhpFileService<LocalAuthoritySupportFileParams> _fileService;

    private readonly IEventDispatcher _eventDispatcher;

    public SchemeRepository(
        IApplicationCrmContext crmContext,
        IApplicationRepository applicationRepository,
        IAhpFileService<LocalAuthoritySupportFileParams> fileService,
        IEventDispatcher eventDispatcher)
    {
        _crmContext = crmContext;
        _applicationRepository = applicationRepository;
        _fileService = fileService;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<SchemeEntity> GetByApplicationId(AhpApplicationId id, UserAccount userAccount, bool includeFiles, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationApplicationById(id.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserApplicationById(id.Value, organisationId, cancellationToken);
        var applicationBasicInfo = await _applicationRepository.GetApplicationBasicInfo(id, userAccount, cancellationToken);

        UploadedFile? file = null;
        if (includeFiles)
        {
            var stakeholderDiscussionsFiles = await _fileService.GetFiles(new LocalAuthoritySupportFileParams(id), cancellationToken);

            file = stakeholderDiscussionsFiles.Count != 0 ? stakeholderDiscussionsFiles.First() : null;
        }

        return CreateEntity(application, applicationBasicInfo, file);
    }

    public async Task<SchemeEntity> Save(SchemeEntity entity, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (!entity.IsModified)
        {
            return entity;
        }

        var dto = await _crmContext.GetOrganisationApplicationById(entity.Application.Id.Value, organisationId.Value, cancellationToken);
        dto.schemeInformationSectionCompletionStatus = SectionStatusMapper.ToDto(entity.Status);
        dto.fundingRequested = entity.Funding.RequiredFunding;
        dto.noOfHomes = entity.Funding.HousesToDeliver;
        dto.developingPartnerId = entity.ApplicationPartners.DevelopingPartner.Id.ToGuidAsString();
        dto.ownerOfTheLandDuringDevelopmentId = entity.ApplicationPartners.OwnerOfTheLand.Id.ToGuidAsString();
        dto.ownerOfTheHomesAfterCompletionId = entity.ApplicationPartners.OwnerOfTheHomes.Id.ToGuidAsString();
        dto.applicationPartnerConfirmation = entity.ApplicationPartners.ArePartnersConfirmed;
        dto.affordabilityEvidence = entity.AffordabilityEvidence.Evidence;
        dto.discussionsWithLocalStakeholders = entity.StakeholderDiscussions.StakeholderDiscussionsDetails.Report;
        dto.meetingLocalProrities = entity.HousingNeeds.MeetingLocalPriorities;
        dto.meetingLocalHousingNeed = entity.HousingNeeds.MeetingLocalHousingNeed;
        dto.sharedOwnershipSalesRisk = entity.SalesRisk.Value;

        await _crmContext.Save(dto, organisationId.Value, cancellationToken);
        await entity.StakeholderDiscussions.SaveChanges(entity.Application.Id, _fileService, cancellationToken);
        await _eventDispatcher.Publish(entity, cancellationToken);

        return entity;
    }

    private static SchemeEntity CreateEntity(AhpApplicationDto dto, ApplicationBasicInfo applicationBasicInfo, UploadedFile? stakeholderDiscussionsFile)
    {
        return new SchemeEntity(
            applicationBasicInfo,
            SectionStatusMapper.ToDomain(dto.schemeInformationSectionCompletionStatus, applicationBasicInfo.Status),
            new SchemeFunding((int?)dto.fundingRequested, dto.noOfHomes),
            ApplicationPartnersMapper.ToDomain(dto),
            new AffordabilityEvidence(dto.affordabilityEvidence),
            new SalesRisk(dto.sharedOwnershipSalesRisk),
            new HousingNeeds(dto.meetingLocalProrities, dto.meetingLocalHousingNeed),
            new StakeholderDiscussions(
                new StakeholderDiscussionsDetails(dto.discussionsWithLocalStakeholders),
                new LocalAuthoritySupportFileContainer(stakeholderDiscussionsFile)));
    }
}
