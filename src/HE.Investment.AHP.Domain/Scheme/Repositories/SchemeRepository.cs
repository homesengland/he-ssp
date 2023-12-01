using System.Runtime.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.Mock;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public class SchemeRepository : ISchemeRepository
{
    private readonly IApplicationCrmContext _repository;
    private readonly IAccountUserContext _accountUserContext;
    private readonly IFileService _fileService;

    public SchemeRepository(IApplicationCrmContext repository, IAccountUserContext accountUserContext, IFileService fileService)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _fileService = fileService;
    }

    public async Task<SchemeEntity> GetByApplicationId(DomainApplicationId id, CancellationToken cancellationToken)
    {
        var application = await _repository.GetById(id.Value, CrmFields.SchemeToRead, cancellationToken);

        var stakeholderDiscussionsFiles = await _fileService.GetByApplicationId(id, cancellationToken);

        return CreateEntity(application, stakeholderDiscussionsFiles.Any() ? stakeholderDiscussionsFiles.First() : null);
    }

    public async Task<SchemeEntity> Save(SchemeEntity entity, CancellationToken cancellationToken)
    {
        if (!entity.IsModified)
        {
            return entity;
        }

        var account = await _accountUserContext.GetSelectedAccount();
        var dto = new AhpApplicationDto
        {
            id = entity.Application.Id.Value,
            organisationId = account.AccountId.ToString(),
            schemeInformationSectionCompletionStatus = SectionStatusMapper.ToDto(entity.Status),
            fundingRequested = entity.Funding?.RequiredFunding,
            noOfHomes = entity.Funding?.HousesToDeliver,
            affordabilityEvidence = entity.AffordabilityEvidence?.Evidence,
            discussionsWithLocalStakeholders = entity.StakeholderDiscussions?.Report,
            meetingLocalProrities = entity.HousingNeeds?.SchemeAndProposalJustification,
            meetingLocalHousingNeed = entity.HousingNeeds?.TypeAndTenureJustification,
            sharedOwnershipSalesRisk = entity.SalesRisk?.Value,
        };

        await _repository.Save(dto, CrmFields.SchemeToUpdate, cancellationToken);

        await entity.StakeholderDiscussionsFiles.SaveChanges(entity.Application.Id, _fileService, cancellationToken);

        return entity;
    }

    private static SchemeEntity CreateEntity(AhpApplicationDto application, UploadedFile? stakeholderDiscussionsFile)
    {
        return new SchemeEntity(
            new ApplicationBasicDetails(new DomainApplicationId(application.id), new ApplicationName(application.name), ApplicationTenureMapper.ToDomain(application.tenure)),
            CreateRequiredFunding(application.fundingRequested, application.noOfHomes),
            SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus),
            new AffordabilityEvidence(application.affordabilityEvidence),
            new SalesRisk(application.sharedOwnershipSalesRisk),
            new HousingNeeds(application.meetingLocalProrities, application.meetingLocalHousingNeed),
            new StakeholderDiscussions(application.discussionsWithLocalStakeholders),
            new StakeholderDiscussionsFiles(stakeholderDiscussionsFile));
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
