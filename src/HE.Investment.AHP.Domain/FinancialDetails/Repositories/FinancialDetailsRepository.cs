using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
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

        MapFromExpectedContributions(financialDetails.ExpectedContributions, dto);
        MapFromPublicGrants(financialDetails.PublicGrants, dto);

        _ = await _applicationCrmContext.Save(dto, organisationId.Value, CrmFields.FinancialDetailsToUpdate.ToList(), cancellationToken);

        return financialDetails;
    }

    private static void MapFromPublicGrants(PublicGrants publicGrants, AhpApplicationDto dto)
    {
        dto.howMuchReceivedFromCountyCouncil = publicGrants.CountyCouncil?.Value;
        dto.howMuchReceivedFromDhscExtraCareFunding = publicGrants.DhscExtraCare?.Value;
        dto.howMuchReceivedFromLocalAuthority1 = publicGrants.LocalAuthority?.Value;
        dto.howMuchReceivedFromSocialServices = publicGrants.SocialServices?.Value;
        dto.howMuchReceivedFromDepartmentOfHealth = publicGrants.HealthRelated?.Value;
        dto.howMuchReceivedFromLotteryFunding = publicGrants.Lottery?.Value;
        dto.howMuchReceivedFromOtherPublicBodies = publicGrants.OtherPublicBodies?.Value;
    }

    private static OtherApplicationCosts MapToOtherApplicationCosts(AhpApplicationDto application)
    {
        return new OtherApplicationCosts(
            application.expectedOnWorks.IsProvided() ? new ExpectedWorksCosts(application.expectedOnWorks!.Value) : null,
            application.expectedOnCosts.IsProvided() ? new ExpectedOnCosts(application.expectedOnCosts!.Value) : null);
    }

    private static void MapFromExpectedContributions(ExpectedContributionsToScheme expectedContributionsToScheme, AhpApplicationDto dto)
    {
        dto.borrowingAgainstRentalIncomeFromThisScheme = expectedContributionsToScheme.RentalIncome?.Value;
        dto.fundingFromOpenMarketHomesOnThisScheme = expectedContributionsToScheme.SalesOfHomesOnThisScheme?.Value;
        dto.fundingFromOpenMarketHomesNotOnThisScheme = expectedContributionsToScheme.SalesOfHomesOnOtherSchemes?.Value;
        dto.ownResources = expectedContributionsToScheme.OwnResources?.Value;
        dto.recycledCapitalGrantFund = expectedContributionsToScheme.RcgfContributions?.Value;
        dto.otherCapitalSources = expectedContributionsToScheme.OtherCapitalSources?.Value;
        dto.totalInitialSalesIncome = expectedContributionsToScheme.SharedOwnershipSales?.Value;
        dto.transferValue = expectedContributionsToScheme.HomesTransferValue?.Value;
    }

    private static ExpectedContributionsToScheme MapToExpectedContributionsToScheme(AhpApplicationDto application, Tenure tenure)
    {
        static ExpectedContributionValue? MapProvidedValues(decimal? value, ExpectedContributionFields field) => value.IsProvided()
            ? new ExpectedContributionValue(field, value!.Value)
            : null;

        return new ExpectedContributionsToScheme(
            MapProvidedValues(application.borrowingAgainstRentalIncomeFromThisScheme, ExpectedContributionFields.RentalIncomeBorrowing),
            MapProvidedValues(application.fundingFromOpenMarketHomesOnThisScheme, ExpectedContributionFields.SaleOfHomesOnThisScheme),
            MapProvidedValues(application.fundingFromOpenMarketHomesNotOnThisScheme, ExpectedContributionFields.SaleOfHomesOnOtherSchemes),
            MapProvidedValues(application.ownResources, ExpectedContributionFields.OwnResources),
            MapProvidedValues(application.recycledCapitalGrantFund, ExpectedContributionFields.RcgfContribution),
            MapProvidedValues(application.otherCapitalSources, ExpectedContributionFields.OtherCapitalSources),
            MapProvidedValues(application.totalInitialSalesIncome, ExpectedContributionFields.SharedOwnershipSales),
            MapProvidedValues(application.transferValue, ExpectedContributionFields.HomesTransferValue),
            tenure);
    }

    private static PublicGrants MapToPublicGrants(AhpApplicationDto application)
    {
        static PublicGrantValue? MapProvidedValues(decimal? value, PublicGrantFields field) => value.IsProvided()
            ? new PublicGrantValue(field, value!.Value)
            : null;

        return new PublicGrants(
            MapProvidedValues(application.howMuchReceivedFromCountyCouncil, PublicGrantFields.CountyCouncilGrants),
            MapProvidedValues(application.howMuchReceivedFromDhscExtraCareFunding, PublicGrantFields.DhscExtraCareGrants),
            MapProvidedValues(application.howMuchReceivedFromLocalAuthority1, PublicGrantFields.LocalAuthorityGrants),
            MapProvidedValues(application.howMuchReceivedFromSocialServices, PublicGrantFields.SocialServicesGrants),
            MapProvidedValues(application.howMuchReceivedFromDepartmentOfHealth, PublicGrantFields.HealthRelatedGrants),
            MapProvidedValues(application.howMuchReceivedFromLotteryFunding, PublicGrantFields.LotteryGrants),
            MapProvidedValues(application.howMuchReceivedFromOtherPublicBodies, PublicGrantFields.OtherPublicBodiesGrants));
    }

    private async Task<FinancialDetailsEntity> CreateEntity(AhpApplicationDto application, CancellationToken cancellationToken)
    {
        var applicationId = AhpApplicationId.From(application.id);
        var applicationBasicInfo = new ApplicationBasicInfo(
            applicationId,
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
            MapToOtherApplicationCosts(application),
            MapToExpectedContributionsToScheme(application, applicationBasicInfo.Tenure),
            MapToPublicGrants(application),
            SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus, applicationBasicInfo.Status));
    }
}
