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
            PurchasePrice = financialDetails.PurchasePrice.ActualPrice ?? financialDetails.PurchasePrice.ExpectedPrice,
            IsSchemaOnPublicLand = financialDetails.LandValue.IsLandPublic,
            LandValue = financialDetails.LandValue.Value,
            ExpectedWorkCost = financialDetails.ExpectedCosts?.WorksCosts,
            ExpectedOnCost = financialDetails.ExpectedCosts?.OnCosts,
            RentalIncomeContribution = financialDetails.Contributions.RentalIncomeBorrowing,
            SubsidyFromSaleOnThisScheme = financialDetails.Contributions.SalesOfHomesOnThisScheme,
            SubsidyFromSaleOnOtherSchemes = financialDetails.Contributions.SalesOfHomesOnOtherSchemes,
            OwnResourcesContribution = financialDetails.Contributions.OwnResources,
            RecycledCapitalGarntFundContribution = financialDetails.Contributions.RCGFContributions,
            OtherCapitalContributions = financialDetails.Contributions.OtherCapitalSources,
            SharedOwnershipSalesContribution = financialDetails.Contributions.SharedOwnershipSales,
            TransferValueOfHomes = financialDetails.Contributions.HomesTransferValue,
            CountyCouncilGrants = financialDetails.Grants.CountyCouncil,
            DHSCExtraCareGrants = financialDetails.Grants.DHSCExtraCare,
            LocalAuthorityGrants = financialDetails.Grants.LocalAuthority,
            SocialServicesGrants = financialDetails.Grants.SocialServices,
            HealthRelatedGrants = financialDetails.Grants.HealthRelated,
            LotteryFunding = financialDetails.Grants.Lottery,
            OtherPublicGrants = financialDetails.Grants.OtherPublicBodies,
            TotalExpectedCosts = financialDetails.TotalExpectedCost,
            TotalExpectedContributions = financialDetails.TotalContributions,
            TotalRecievedGrands = financialDetails.TotalGrants,
        };
    }
}
