using System.Runtime.Serialization;
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
using ApplicationBasicDetails = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationBasicDetails;

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
            ? await _repository.GetOrganisationApplicationById(id.Value, organisationId, CrmFields.SchemeToRead, cancellationToken)
            : await _repository.GetUserApplicationById(id.Value, organisationId, CrmFields.SchemeToRead, cancellationToken);

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

        await _repository.Save(dto, organisationId.Value, CrmFields.SchemeToUpdate, cancellationToken);

        await entity.StakeholderDiscussions.SaveChanges(entity.Application.Id, _fileService, cancellationToken);

        return entity;
    }

    private static SchemeEntity CreateEntity(AhpApplicationDto application, UploadedFile? stakeholderDiscussionsFile)
    {
        return new SchemeEntity(
            new ApplicationBasicDetails(
                new AhpApplicationId(application.id),
                new ApplicationName(application.name),
                ApplicationTenureMapper.ToDomain(application.tenure)),
            CreateRequiredFunding(application.fundingRequested, application.noOfHomes),
            SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus),
            new AffordabilityEvidence(application.affordabilityEvidence),
            new SalesRisk(application.sharedOwnershipSalesRisk),
            new HousingNeeds(application.meetingLocalProrities, application.meetingLocalHousingNeed),
            new StakeholderDiscussions(
                new StakeholderDiscussionsDetails(application.discussionsWithLocalStakeholders),
                new LocalAuthoritySupportFileContainer(stakeholderDiscussionsFile)));
    }

    private static SchemeFunding CreateRequiredFunding(decimal? fundingRequested, int? noOfHomes)
    {
        var funding = (SchemeFunding)FormatterServices.GetUninitializedObject(typeof(SchemeFunding));
        SetPropertyValue(funding, nameof(SchemeFunding.RequiredFunding), fundingRequested);
        SetPropertyValue(funding, nameof(SchemeFunding.HousesToDeliver), noOfHomes);

        return funding;
    }

    private static void SetPropertyValue(SchemeFunding member, string propName, object? newValue)
    {
        var propertyInfo = typeof(SchemeFunding).GetProperty(propName);
        if (propertyInfo == null)
        {
            return;
        }

        propertyInfo.SetValue(member, newValue);
    }
}
