using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Mappers;

public static class ExpectedContributionsToSchemeMappers
{
    public static void MapFromExpectedContributions(ExpectedContributionsToScheme expectedContributionsToScheme, AhpApplicationDto dto)
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

    public static ExpectedContributionsToScheme MapToExpectedContributionsToScheme(AhpApplicationDto application, Tenure tenure)
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
}
