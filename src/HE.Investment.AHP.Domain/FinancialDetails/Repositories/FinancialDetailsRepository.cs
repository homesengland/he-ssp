using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Mappers;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IAhpProgrammeRepository _programmeRepository;

    public FinancialDetailsRepository(IApplicationCrmContext applicationCrmContext, IAhpProgrammeRepository programmeRepository)
    {
        _applicationCrmContext = applicationCrmContext;
        _programmeRepository = programmeRepository;
    }

    public async Task<FinancialDetailsEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplicationById(id.Value, organisationId, CrmFields.FinancialDetailsToRead.ToList(), cancellationToken)
            : await _applicationCrmContext.GetUserApplicationById(id.Value, organisationId, CrmFields.FinancialDetailsToRead.ToList(), cancellationToken);

        return await CreateEntity(application, cancellationToken);
    }

    public async Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var dto = new AhpApplicationDto
        {
            id = financialDetails.ApplicationBasicInfo.Id.Value,
            name = financialDetails.ApplicationBasicInfo.Name.Name,
            actualAcquisitionCost = financialDetails.LandStatus.PurchasePrice?.Value,
            expectedAcquisitionCost = financialDetails.LandStatus.ExpectedPurchasePrice?.Value,
            isPublicLand = YesNoTypeMapper.Map(financialDetails.LandValue.IsPublicLand),
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

    private async Task<FinancialDetailsEntity> CreateEntity(AhpApplicationDto application, CancellationToken cancellationToken)
    {
        var applicationId = AhpApplicationId.From(application.id);
        var applicationBasicInfo = new ApplicationBasicInfo(
            applicationId,
            new SiteId(application.siteId),
            new ApplicationName(application.name),
            ApplicationTenureMapper.ToDomain(application.tenure)!.Value,
            AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus),
            await _programmeRepository.GetProgramme(applicationId, cancellationToken));

        return new FinancialDetailsEntity(
            applicationBasicInfo,
            new LandStatus(
                application.actualAcquisitionCost.IsProvided() ? new PurchasePrice(application.actualAcquisitionCost!.Value) : null,
                application.expectedAcquisitionCost.IsProvided() ? new ExpectedPurchasePrice(application.expectedAcquisitionCost!.Value) : null),
            new LandValue(
                application.currentLandValue.IsProvided() ? new CurrentLandValue(application.currentLandValue!.Value) : null,
                YesNoTypeMapper.Map(application.isPublicLand)),
            OtherApplicationCostsMapper.MapToOtherApplicationCosts(application),
            ExpectedContributionsToSchemeMapper.MapToExpectedContributionsToScheme(application, applicationBasicInfo.Tenure),
            PublicGrantsMapper.MapToPublicGrants(application),
            SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus, applicationBasicInfo.Status));
    }
}
