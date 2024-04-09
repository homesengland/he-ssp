using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Mappers;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IAhpProgrammeRepository _programmeRepository;

    private readonly ISchemeRepository _schemeRepository;

    private readonly ISiteRepository _siteRepository;

    public FinancialDetailsRepository(
        IApplicationCrmContext applicationCrmContext,
        IAhpProgrammeRepository programmeRepository,
        ISchemeRepository schemeRepository,
        ISiteRepository siteRepository)
    {
        _applicationCrmContext = applicationCrmContext;
        _programmeRepository = programmeRepository;
        _schemeRepository = schemeRepository;
        _siteRepository = siteRepository;
    }

    public async Task<FinancialDetailsEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplicationById(id.Value,
                organisationId,
                CrmFields.FinancialDetailsToRead.ToList(),
                cancellationToken)
            : await _applicationCrmContext.GetUserApplicationById(id.Value, organisationId, CrmFields.FinancialDetailsToRead.ToList(), cancellationToken);
        var schemeFunding = (await _schemeRepository.GetByApplicationId(id, userAccount, false, cancellationToken)).Funding;
        var siteBasicInfo = await _siteRepository.GetSiteBasicInfo(new SiteId(application.siteId), userAccount, cancellationToken);

        return await CreateEntity(application, userAccount, siteBasicInfo, schemeFunding, cancellationToken);
    }

    public async Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (financialDetails.IsModified is false)
        {
            return financialDetails;
        }

        var dto = new AhpApplicationDto
        {
            id = financialDetails.ApplicationBasicInfo.Id.Value,
            name = financialDetails.ApplicationBasicInfo.Name.Name,
            actualAcquisitionCost = financialDetails.LandStatus.PurchasePrice?.Value,
            expectedAcquisitionCost = financialDetails.LandStatus.ExpectedPurchasePrice?.Value,
            isPublicLand = financialDetails.LandValue.IsPublicLand,
            currentLandValue = financialDetails.LandValue.CurrentLandValue?.Value,
            expectedOnWorks = financialDetails.OtherApplicationCosts.ExpectedWorksCosts?.Value,
            expectedOnCosts = financialDetails.OtherApplicationCosts.ExpectedOnCosts?.Value,
            financialDetailsSectionCompletionStatus = SectionStatusMapper.ToDto(financialDetails.SectionStatus),
        };

        ExpectedContributionsToSchemeMapper.MapFromExpectedContributions(financialDetails.ExpectedContributions, dto);
        PublicGrantsMapper.MapFromPublicGrants(financialDetails.PublicGrants, dto);

        _ = await _applicationCrmContext.Save(dto, organisationId.Value, CrmFields.FinancialDetailsToUpdate.ToList(), cancellationToken);

        return financialDetails;
    }

    private async Task<FinancialDetailsEntity> CreateEntity(
        AhpApplicationDto application,
        UserAccount userAccount,
        SiteBasicInfo siteBasicInfo,
        SchemeFunding schemeFunding,
        CancellationToken cancellationToken)
    {
        var applicationId = AhpApplicationId.From(application.id);
        var applicationBasicInfo = new ApplicationBasicInfo(
            applicationId,
            new SiteId(application.siteId),
            new ApplicationName(application.name),
            ApplicationTenureMapper.ToDomain(application.tenure)!.Value,
            AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus),
            await _programmeRepository.GetProgramme(applicationId, cancellationToken),
            new ApplicationStateFactory(userAccount));

        return new FinancialDetailsEntity(
            applicationBasicInfo,
            siteBasicInfo,
            schemeFunding,
            new LandStatus(
                application.actualAcquisitionCost.IsProvided() ? new PurchasePrice(application.actualAcquisitionCost!.Value) : null,
                application.expectedAcquisitionCost.IsProvided() ? new ExpectedPurchasePrice(application.expectedAcquisitionCost!.Value) : null),
            new LandValue(
                application.currentLandValue.IsProvided() ? new CurrentLandValue(application.currentLandValue!.Value) : null,
                application.isPublicLand),
            OtherApplicationCostsMapper.MapToOtherApplicationCosts(application),
            ExpectedContributionsToSchemeMapper.MapToExpectedContributionsToScheme(application, applicationBasicInfo.Tenure),
            PublicGrantsMapper.MapToPublicGrants(application),
            SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus, applicationBasicInfo.Status));
    }
}
