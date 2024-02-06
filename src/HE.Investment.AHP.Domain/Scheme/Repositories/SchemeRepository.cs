using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.CRM.Mappers;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public class SchemeRepository : ISchemeRepository
{
    private readonly IApplicationCrmContext _repository;

    private readonly IAhpFileService<LocalAuthoritySupportFileParams> _fileService;

    public SchemeRepository(IApplicationCrmContext repository, IAhpFileService<LocalAuthoritySupportFileParams> fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<SchemeEntity> GetByApplicationId(AhpApplicationId id, UserAccount userAccount, bool includeFiles, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = userAccount.CanViewAllApplications()
            ? await _repository.GetOrganisationApplicationById(id.Value, organisationId, CrmFields.SchemeToRead.ToList(), cancellationToken)
            : await _repository.GetUserApplicationById(id.Value, organisationId, CrmFields.SchemeToRead.ToList(), cancellationToken);

        UploadedFile? file = null;
        if (includeFiles)
        {
            var stakeholderDiscussionsFiles = await _fileService.GetFiles(new LocalAuthoritySupportFileParams(id), cancellationToken);

            file = stakeholderDiscussionsFiles.Any() ? stakeholderDiscussionsFiles.First() : null;
        }

        return CreateEntity(application, file);
    }

    public async Task<SchemeEntity> Save(SchemeEntity entity, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (!entity.IsModified)
        {
            return entity;
        }

        var dto = new AhpApplicationDto
        {
            id = entity.Application.Id.Value,
            organisationId = organisationId.ToString(),
            schemeInformationSectionCompletionStatus = SectionStatusMapper.ToDto(entity.Status),
            fundingRequested = entity.Funding.RequiredFunding,
            noOfHomes = entity.Funding.HousesToDeliver,
            affordabilityEvidence = entity.AffordabilityEvidence.Evidence,
            discussionsWithLocalStakeholders = entity.StakeholderDiscussions.StakeholderDiscussionsDetails.Report,
            meetingLocalProrities = entity.HousingNeeds.MeetingLocalPriorities,
            meetingLocalHousingNeed = entity.HousingNeeds.MeetingLocalHousingNeed,
            sharedOwnershipSalesRisk = entity.SalesRisk.Value,
        };

        await _repository.Save(dto, organisationId.Value, CrmFields.SchemeToUpdate.ToList(), cancellationToken);

        await entity.StakeholderDiscussions.SaveChanges(entity.Application.Id, _fileService, cancellationToken);

        return entity;
    }

    private static SchemeEntity CreateEntity(AhpApplicationDto application, UploadedFile? stakeholderDiscussionsFile)
    {
        var applicationBasicInfo = new ApplicationBasicInfo(
            AhpApplicationId.From(application.id),
            new ApplicationName(application.name),
            ApplicationTenureMapper.ToDomain(application.tenure)!.Value,
            AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus));

        return new SchemeEntity(
            applicationBasicInfo,
            new SchemeFunding((int?)application.fundingRequested, application.noOfHomes),
            SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus, applicationBasicInfo.Status),
            new AffordabilityEvidence(application.affordabilityEvidence),
            new SalesRisk(application.sharedOwnershipSalesRisk),
            new HousingNeeds(application.meetingLocalProrities, application.meetingLocalHousingNeed),
            new StakeholderDiscussions(
                new StakeholderDiscussionsDetails(application.discussionsWithLocalStakeholders),
                new LocalAuthoritySupportFileContainer(stakeholderDiscussionsFile)));
    }
}
