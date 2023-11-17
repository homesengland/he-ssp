using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.Repositories;

public class SchemeRepository : ISchemeRepository
{
    private readonly IApplicationCrmContext _repository;
    private readonly IAccountUserContext _accountUserContext;

    public SchemeRepository(IApplicationCrmContext repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<SchemeEntity> GetByApplicationId(DomainApplicationId id, CancellationToken cancellationToken)
    {
        var application = await _repository.GetById(id.Value, CrmFields.SchemeToRead, cancellationToken);

        if (application.schemeInformationSectionCompletionStatus == null)
        {
            throw new NotFoundException("Scheme", $"application {id.Value}");
        }

        return CreateEntity(application);
    }

    public async Task<SchemeEntity> Save(SchemeEntity entity, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var dto = new AhpApplicationDto
        {
            id = entity.Application.Id.Value,
            organisationId = account.AccountId.ToString(),
            schemeInformationSectionCompletionStatus = SectionStatusMapper.ToDto(entity.Status),
            fundingRequested = entity.Funding.RequiredFunding,
            noOfHomes = entity.Funding.HousesToDeliver,
            affordabilityEvidence = entity.AffordabilityEvidence?.Evidence,
            discussionsWithLocalStakeholders = entity.StakeholderDiscussions?.Report,
            meetingLocalProrities = entity.HousingNeeds?.SchemeAndProposalJustification,
            meetingLocalHousingNeed = entity.HousingNeeds?.TypeAndTenureJustification,
            sharedOwnershipSalesRisk = entity.SalesRisk?.Value,
        };

        await _repository.Save(dto, CrmFields.SchemeToUpdate, cancellationToken);

        return entity;
    }

    private static SchemeEntity CreateEntity(AhpApplicationDto application)
    {
        return new SchemeEntity(
            new ApplicationBasicDetails(new DomainApplicationId(application.id), new ApplicationName(application.name)),
            new SchemeFunding(application.fundingRequested.ToWholeNumberString(), application.noOfHomes.ToString()),
            SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus),
            new AffordabilityEvidence(application.affordabilityEvidence),
            new SalesRisk(application.sharedOwnershipSalesRisk),
            new HousingNeeds(application.meetingLocalProrities, application.meetingLocalHousingNeed),
            new StakeholderDiscussions(application.discussionsWithLocalStakeholders));
    }
}
