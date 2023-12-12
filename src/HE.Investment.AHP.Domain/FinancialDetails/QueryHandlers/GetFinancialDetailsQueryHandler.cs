using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

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
            ApplicationName = financialDetails.ApplicationBasicInfo.Name.Name,
            PurchasePrice = financialDetails.PurchasePrice?.Value ?? financialDetails.ExpectedPurchasePrice?.Value,
            IsSchemaOnPublicLand = financialDetails.IsPublicLand,
            LandValue = financialDetails.LandValue?.Value,
            ExpectedWorkCost = financialDetails.OtherApplicationCosts.ExpectedWorksCosts?.Value,
            ExpectedOnCost = financialDetails.OtherApplicationCosts.ExpectedOnCosts?.Value,
            TotalExpectedCosts = financialDetails.ExpectedTotalCosts(),
        };

        MapPublicGrants(financialDetailsDto, financialDetails!.PublicGrants);
        MapExpectedContributions(financialDetailsDto, financialDetails.ExpectedContributions);

        return financialDetailsDto;
    }

    private static void MapPublicGrants(Contract.FinancialDetails.FinancialDetails financialDetailsDto, PublicGrants publicGrants)
    {
        financialDetailsDto.CountyCouncilGrants = publicGrants.CountyCouncil?.Value;
        financialDetailsDto.DHSCExtraCareGrants = publicGrants.DhscExtraCare?.Value;
        financialDetailsDto.LocalAuthorityGrants = publicGrants.LocalAuthority?.Value;
        financialDetailsDto.SocialServicesGrants = publicGrants.SocialServices?.Value;
        financialDetailsDto.HealthRelatedGrants = publicGrants.HealthRelated?.Value;
        financialDetailsDto.LotteryFunding = publicGrants.Lottery?.Value;
        financialDetailsDto.OtherPublicGrants = publicGrants.OtherPublicBodies?.Value;
        financialDetailsDto.TotalRecievedGrands = publicGrants.CalculateTotal();
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
