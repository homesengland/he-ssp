using System.Globalization;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;
public class GetFinancialDetailsQueryHandler : IRequestHandler<GetFinancialDetailsQuery, Contract.FinancialDetails.FinancialDetails>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public GetFinancialDetailsQueryHandler(IFinancialDetailsRepository fianncialDetailsRepository)
    {
        _financialDetailsRepository = fianncialDetailsRepository;
    }

    public async Task<Contract.FinancialDetails.FinancialDetails> Handle(GetFinancialDetailsQuery request, CancellationToken cancellationToken)
    {
        var financialDetails = await _financialDetailsRepository.GetById(ApplicationId.From(request.ApplicationId), cancellationToken);

        return new Contract.FinancialDetails.FinancialDetails()
        {
            ApplicationId = financialDetails.ApplicationId.Value,
            ApplicationName = financialDetails.ApplicationName,
            IsPurchasePriceKnown = financialDetails.IsPurchasePriceKnown,
            PurchasePrice = financialDetails.PurchasePrice?.Value.ToString(CultureInfo.InvariantCulture),
            IsSchemaOnPublicLand = financialDetails.LandOwnership?.Value,
            LandValue = financialDetails.LandValue?.Value.ToString(CultureInfo.InvariantCulture),
            ExpectedWorkCost = financialDetails.ExpectedWorksCosts?.Value.ToString(CultureInfo.InvariantCulture),
            ExpectedOnCost = financialDetails.ExpectedOnCosts?.Value.ToString(CultureInfo.InvariantCulture),
            RentalIncomeContribution = financialDetails.RentalIncomeBorrowing?.Value.ToString(CultureInfo.InvariantCulture),
            SubsidyFromSaleOnThisScheme = financialDetails.SalesOfHomesOnThisScheme?.Value.ToString(CultureInfo.InvariantCulture),
            SubsidyFromSaleOnOtherSchemes = financialDetails.SalesOfHomesOnOtherSchemes?.Value.ToString(CultureInfo.InvariantCulture),
            OwnResourcesContribution = financialDetails.OwnResources?.Value.ToString(CultureInfo.InvariantCulture),
            RecycledCapitalGarntFundContribution = financialDetails.RCGFContribution?.Value.ToString(CultureInfo.InvariantCulture),
            OtherCapitalContributions = financialDetails.OtherCapitalSources?.Value.ToString(CultureInfo.InvariantCulture),
            SharedOwnershipSalesContribution = financialDetails.SharedOwnershipSales?.Value.ToString(CultureInfo.InvariantCulture),
            TransferValueOfHomes = financialDetails.HomesTransferValue?.Value.ToString(CultureInfo.InvariantCulture),
        };
    }
}
