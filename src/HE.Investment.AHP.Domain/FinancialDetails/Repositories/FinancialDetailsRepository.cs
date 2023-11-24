using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
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
            actualAcquisitionCost = financialDetails.ActualPurchasePrice?.Value,
            expectedAcquisitionCost = financialDetails.ExpectedPurchasePrice?.Value,
            isPublicLand = financialDetails.LandOwnership?.Value,
            currentLandValue = financialDetails.LandValue?.Value,
            expectedOnWorks = financialDetails.ExpectedWorksCosts?.Value,
            expectedOnCosts = financialDetails.ExpectedOnCosts?.Value,
            borrowingAgainstRentalIncomeFromThisScheme = financialDetails.RentalIncomeBorrowing?.Value,
            fundingFromOpenMarketHomesOnThisScheme = financialDetails.SalesOfHomesOnThisScheme?.Value,
            fundingFromOpenMarketHomesNotOnThisScheme = financialDetails.SalesOfHomesOnOtherSchemes?.Value,
            ownResources = financialDetails.OwnResources?.Value,
            recycledCapitalGrantFund = financialDetails.RCGFContribution?.Value,
            otherCapitalSources = financialDetails.OtherCapitalSources?.Value,
            totalInitialSalesIncome = financialDetails.SharedOwnershipSales?.Value,
            transferValue = financialDetails.HomesTransferValue?.Value,
        };

        _ = await _applicationCrmContext.Save(dto, CrmFields.FinancialDetailsToUpdate, cancellationToken);

        return financialDetails;
    }

    private static FinancialDetailsEntity CreateEntity(AhpApplicationDto application)
    {
        return new FinancialDetailsEntity(
            ApplicationId.From(application.id),
            application.name ?? "Unknown",
            ActualPurchasePrice.From(application.actualAcquisitionCost),
            ExpectedPurchasePrice.From(application.expectedAcquisitionCost),
            application.isPublicLand.HasValue ? new LandOwnership(application.isPublicLand.Value ? CommonResponse.Yes : CommonResponse.No) : null,
            LandValue.From(application.currentLandValue),
            ExpectedWorksCosts.From(application.expectedOnWorks),
            ExpectedOnCosts.From(application.expectedOnCosts),
            RentalIncomeBorrowing.From(application.borrowingAgainstRentalIncomeFromThisScheme),
            SalesOfHomesOnThisScheme.From(application.fundingFromOpenMarketHomesOnThisScheme),
            SalesOfHomesOnOtherSchemes.From(application.fundingFromOpenMarketHomesNotOnThisScheme),
            OwnResources.From(application.ownResources),
            RCGFContribution.From(application.recycledCapitalGrantFund),
            OtherCapitalSources.From(application.otherCapitalSources),
            SharedOwnershipSales.From(application.totalInitialSalesIncome),
            HomesTransferValue.From(application.transferValue));
    }
}
