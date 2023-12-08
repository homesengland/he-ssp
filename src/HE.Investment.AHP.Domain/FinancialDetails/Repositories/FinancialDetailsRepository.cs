using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    public FinancialDetailsRepository(IApplicationCrmContext applicationCrmContext)
    {
        _applicationCrmContext = applicationCrmContext;
    }

    public async Task<FinancialDetailsEntity> GetById(ApplicationId id, CancellationToken cancellationToken)
    {
        var application = await _applicationCrmContext.GetById(id.Value.ToString(), CrmFields.FinancialDetailsToRead, cancellationToken);

        return CreateEntity(application);
    }

    public async Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, CancellationToken cancellationToken)
    {
        var dto = new AhpApplicationDto
        {
            id = financialDetails.ApplicationId.Value.ToString(),
            name = financialDetails.ApplicationName,
            actualAcquisitionCost = financialDetails.PurchasePrice?.Value,
            expectedAcquisitionCost = financialDetails.ExpectedPurchasePrice?.Value,
            isPublicLand = financialDetails.IsPublicLand,
            currentLandValue = financialDetails.LandValue?.Value,
            expectedOnWorks = financialDetails.ExpectedWorksCosts?.Value,
            expectedOnCosts = financialDetails.ExpectedOnCosts?.Value,
            howMuchReceivedFromCountyCouncil = financialDetails.Grants?.CountyCouncil,
            howMuchReceivedFromDhscExtraCareFunding = financialDetails.Grants?.DHSCExtraCare,
            howMuchReceivedFromLocalAuthority1 = financialDetails.Grants?.LocalAuthority,
            howMuchReceivedFromSocialServices = financialDetails.Grants?.SocialServices,
            howMuchReceivedFromDepartmentOfHealth = financialDetails.Grants?.HealthRelated,
            howMuchReceivedFromLotteryFunding = financialDetails.Grants?.Lottery,
            howMuchReceivedFromOtherPublicBodies = financialDetails.Grants?.OtherPublicBodies,
            financialDetailsSectionCompletionStatus = SectionStatusMapper.ToDto(financialDetails.SectionStatus),
        };

        MapExpectedContributions(financialDetails.ExpectedContributions, dto);

        _ = await _applicationCrmContext.Save(dto, CrmFields.FinancialDetailsToUpdate, cancellationToken);

        return financialDetails;
    }

    private static void MapExpectedContributions(ExpectedContributionsToScheme expectedContributionsToScheme, AhpApplicationDto dto)
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

    private static FinancialDetailsEntity CreateEntity(AhpApplicationDto application)
    {
        return new FinancialDetailsEntity(
            ApplicationId.From(application.id),
            application.name ?? "Unknown",
            application.actualAcquisitionCost.IsProvided() ? new PurchasePrice(application.actualAcquisitionCost!.Value) : null,
            application.expectedAcquisitionCost.IsProvided() ? new ExpectedPurchasePrice(application.expectedAcquisitionCost!.Value) : null,
            application.currentLandValue.IsProvided() ? new CurrentLandValue(application.currentLandValue!.Value) : null,
            application.isPublicLand,
            application.expectedOnWorks.IsProvided() ? new ExpectedWorksCosts(application.expectedOnWorks!.Value) : null,
            application.expectedOnCosts.IsProvided() ? new ExpectedOnCosts(application.expectedOnCosts!.Value) : null,
            MapToExpectedContributionsToScheme(application),
            Grants.From(
                application.howMuchReceivedFromCountyCouncil,
                application.howMuchReceivedFromDhscExtraCareFunding,
                application.howMuchReceivedFromLocalAuthority1,
                application.howMuchReceivedFromSocialServices,
                application.howMuchReceivedFromDepartmentOfHealth,
                application.howMuchReceivedFromLotteryFunding,
                application.howMuchReceivedFromOtherPublicBodies),
            SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus));
    }

    private static ExpectedContributionsToScheme MapToExpectedContributionsToScheme(AhpApplicationDto application)
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
            MapProvidedValues(application.transferValue, ExpectedContributionFields.HomesTransferValue));
    }
}
