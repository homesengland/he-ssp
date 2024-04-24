using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Mappers;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private readonly IApplicationCrmContext _crmContext;

    private readonly IApplicationRepository _applicationRepository;

    private readonly ISchemeRepository _schemeRepository;

    private readonly ISiteRepository _siteRepository;

    public FinancialDetailsRepository(
        IApplicationCrmContext crmContext,
        IApplicationRepository applicationRepository,
        ISchemeRepository schemeRepository,
        ISiteRepository siteRepository)
    {
        _crmContext = crmContext;
        _applicationRepository = applicationRepository;
        _schemeRepository = schemeRepository;
        _siteRepository = siteRepository;
    }

    public async Task<FinancialDetailsEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationApplicationById(
                id.Value,
                organisationId,
                cancellationToken)
            : await _crmContext.GetUserApplicationById(id.Value, organisationId, cancellationToken);
        var applicationBasicInfo = await _applicationRepository.GetApplicationBasicInfo(id, userAccount, cancellationToken);
        var schemeFunding = (await _schemeRepository.GetByApplicationId(id, userAccount, false, cancellationToken)).Funding;
        var siteBasicInfo = await _siteRepository.GetSiteBasicInfo(new SiteId(application.siteId), userAccount, cancellationToken);

        return CreateEntity(application, applicationBasicInfo, siteBasicInfo, schemeFunding);
    }

    public async Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (financialDetails.IsModified is false)
        {
            return financialDetails;
        }

        var dto = await _crmContext.GetOrganisationApplicationById(financialDetails.ApplicationBasicInfo.Id.Value, organisationId.Value, cancellationToken);
        dto.actualAcquisitionCost = financialDetails.LandStatus.PurchasePrice?.Value;
        dto.expectedAcquisitionCost = financialDetails.LandStatus.ExpectedPurchasePrice?.Value;
        dto.isPublicLand = financialDetails.LandValue.IsPublicLand;
        dto.currentLandValue = financialDetails.LandValue.CurrentLandValue?.Value;
        dto.expectedOnWorks = financialDetails.OtherApplicationCosts.ExpectedWorksCosts?.Value;
        dto.expectedOnCosts = financialDetails.OtherApplicationCosts.ExpectedOnCosts?.Value;
        dto.financialDetailsSectionCompletionStatus = SectionStatusMapper.ToDto(financialDetails.SectionStatus);

        ExpectedContributionsToSchemeMapper.MapFromExpectedContributions(financialDetails.ExpectedContributions, dto);
        PublicGrantsMapper.MapFromPublicGrants(financialDetails.PublicGrants, dto);

        _ = await _crmContext.Save(dto, organisationId.Value, cancellationToken);

        return financialDetails;
    }

    private static FinancialDetailsEntity CreateEntity(
        AhpApplicationDto application,
        ApplicationBasicInfo applicationBasicInfo,
        SiteBasicInfo siteBasicInfo,
        SchemeFunding schemeFunding)
    {
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
