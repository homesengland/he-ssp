using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

public class GetFinancialDetailsQueryHandler : IRequestHandler<GetFinancialDetailsQuery, Contract.FinancialDetails.FinancialDetails>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public GetFinancialDetailsQueryHandler(IFinancialDetailsRepository financialDetailsRepository)
    {
        _financialDetailsRepository = financialDetailsRepository;
    }

    public async Task<Contract.FinancialDetails.FinancialDetails> Handle(GetFinancialDetailsQuery request, CancellationToken cancellationToken)
    {
        var financialDetails = await _financialDetailsRepository.GetById(ApplicationId.From(request.ApplicationId), cancellationToken);

        var financialDetailsDto = new Contract.FinancialDetails.FinancialDetails
        {
            ApplicationId = financialDetails.ApplicationId.Value,
            ApplicationName = financialDetails.ApplicationName,
            PurchasePrice = financialDetails.PurchasePrice?.Value ?? financialDetails.ExpectedPurchasePrice?.Value,
            IsSchemaOnPublicLand = financialDetails.IsPublicLand,
            LandValue = financialDetails.LandValue?.Value,
            ExpectedWorkCost = financialDetails.ExpectedWorksCosts?.Value,
            ExpectedOnCost = financialDetails.ExpectedOnCosts?.Value,
            CountyCouncilGrants = financialDetails.Grants.CountyCouncil,
            DHSCExtraCareGrants = financialDetails.Grants.DHSCExtraCare,
            LocalAuthorityGrants = financialDetails.Grants.LocalAuthority,
            SocialServicesGrants = financialDetails.Grants.SocialServices,
            HealthRelatedGrants = financialDetails.Grants.HealthRelated,
            LotteryFunding = financialDetails.Grants.Lottery,
            OtherPublicGrants = financialDetails.Grants.OtherPublicBodies,
            TotalExpectedCosts = financialDetails.ExpectedTotalCosts(),
            TotalRecievedGrands = financialDetails?.Grants?.TotalGrants ?? 0,
        };

        MapExpectedContributions(financialDetailsDto, financialDetails!.ExpectedContributions);

        return financialDetailsDto;
    }

    private static void MapExpectedContributions(
        Contract.FinancialDetails.FinancialDetails financialDetailsDto,
        ExpectedContributionsToScheme expectedContributionsToScheme)
    {
        financialDetailsDto.RentalIncomeContribution = expectedContributionsToScheme.RentalIncome?.Value;
        financialDetailsDto.SubsidyFromSaleOnThisScheme = expectedContributionsToScheme.SalesOfHomesOnThisScheme?.Value;
        financialDetailsDto.SubsidyFromSaleOnOtherSchemes =
            expectedContributionsToScheme.SalesOfHomesOnOtherSchemes?.Value;
        financialDetailsDto.OwnResourcesContribution = expectedContributionsToScheme.OwnResources?.Value;
        financialDetailsDto.RecycledCapitalGarntFundContribution = expectedContributionsToScheme.RcgfContributions?.Value;
        financialDetailsDto.OtherCapitalContributions = expectedContributionsToScheme.OtherCapitalSources?.Value;
        financialDetailsDto.SharedOwnershipSalesContribution = expectedContributionsToScheme.SharedOwnershipSales?.Value;
        financialDetailsDto.TransferValueOfHomes = expectedContributionsToScheme.HomesTransferValue?.Value;
        financialDetailsDto.TotalExpectedContributions = expectedContributionsToScheme.CalculateTotal();
    }
}
