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
            borrowingAgainstRentalIncomeFromThisScheme = financialDetails.Contributions.RentalIncomeBorrowing,
            fundingFromOpenMarketHomesOnThisScheme = financialDetails.Contributions.SalesOfHomesOnThisScheme,
            fundingFromOpenMarketHomesNotOnThisScheme = financialDetails.Contributions.SalesOfHomesOnOtherSchemes,
            ownResources = financialDetails.Contributions.OwnResources,
            recycledCapitalGrantFund = financialDetails.Contributions.RCGFContributions,
            otherCapitalSources = financialDetails.Contributions.OtherCapitalSources,
            totalInitialSalesIncome = financialDetails.Contributions.SharedOwnershipSales,
            transferValue = financialDetails.Contributions.HomesTransferValue,
            howMuchReceivedFromCountyCouncil = financialDetails.Grants?.CountyCouncil,
            howMuchReceivedFromDhscExtraCareFunding = financialDetails.Grants?.DHSCExtraCare,
            howMuchReceivedFromLocalAuthority1 = financialDetails.Grants?.LocalAuthority,
            howMuchReceivedFromSocialServices = financialDetails.Grants?.SocialServices,
            howMuchReceivedFromDepartmentOfHealth = financialDetails.Grants?.HealthRelated,
            howMuchReceivedFromLotteryFunding = financialDetails.Grants?.Lottery,
            howMuchReceivedFromOtherPublicBodies = financialDetails.Grants?.OtherPublicBodies,
            financialDetailsSectionCompletionStatus = SectionStatusMapper.ToDto(financialDetails.SectionStatus),
        };

        _ = await _applicationCrmContext.Save(dto, CrmFields.FinancialDetailsToUpdate, cancellationToken);

        return financialDetails;
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
            Contributions.From(
                application.borrowingAgainstRentalIncomeFromThisScheme,
                application.fundingFromOpenMarketHomesOnThisScheme,
                application.fundingFromOpenMarketHomesNotOnThisScheme,
                application.ownResources,
                application.recycledCapitalGrantFund,
                application.otherCapitalSources,
                application.totalInitialSalesIncome,
                application.transferValue),
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
}
