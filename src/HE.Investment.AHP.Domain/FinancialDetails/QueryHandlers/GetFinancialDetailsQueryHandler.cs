using System.Globalization;
using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
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
            ActualPurchasePrice = financialDetails.ActualPurchasePrice?.Value,
            ExpectedPurchasePrice = financialDetails.ExpectedPurchasePrice?.Value,
            IsSchemaOnPublicLand = financialDetails.LandOwnership?.Value,
            LandValue = financialDetails.LandValue?.Value,
            ExpectedWorkCost = financialDetails.ExpectedWorksCosts?.Value,
            ExpectedOnCost = financialDetails.ExpectedOnCosts?.Value,
            RentalIncomeContribution = financialDetails.RentalIncomeBorrowing?.Value,
            SubsidyFromSaleOnThisScheme = financialDetails.SalesOfHomesOnThisScheme?.Value,
            SubsidyFromSaleOnOtherSchemes = financialDetails.SalesOfHomesOnOtherSchemes?.Value,
            OwnResourcesContribution = financialDetails.OwnResources?.Value,
            RecycledCapitalGarntFundContribution = financialDetails.RCGFContribution?.Value,
            OtherCapitalContributions = financialDetails.OtherCapitalSources?.Value,
            SharedOwnershipSalesContribution = financialDetails.SharedOwnershipSales?.Value,
            TransferValueOfHomes = financialDetails.HomesTransferValue?.Value,
        };
    }
}
