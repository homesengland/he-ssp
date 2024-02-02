using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

public class GetFinancialDetailsQueryHandler : IRequestHandler<GetFinancialDetailsQuery, Contract.FinancialDetails.FinancialDetails>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetFinancialDetailsQueryHandler(IFinancialDetailsRepository financialDetailsRepository, IAccountUserContext accountUserContext)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<Contract.FinancialDetails.FinancialDetails> Handle(GetFinancialDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var financialDetails = await _financialDetailsRepository.GetById(request.ApplicationId, account, cancellationToken);

        var financialDetailsDto = new Contract.FinancialDetails.FinancialDetails
        {
            ApplicationName = financialDetails.ApplicationBasicInfo.Name.Name,
            PurchasePrice = financialDetails.LandStatus.PurchasePrice?.Value ?? financialDetails.LandStatus.ExpectedPurchasePrice?.Value,
            IsSchemaOnPublicLand = financialDetails.LandValue.IsPublicLand,
            LandValue = financialDetails.LandValue.CurrentLandValue?.Value,
            ExpectedWorkCost = financialDetails.OtherApplicationCosts.ExpectedWorksCosts?.Value,
            ExpectedOnCost = financialDetails.OtherApplicationCosts.ExpectedOnCosts?.Value,
            TotalExpectedCosts = financialDetails.ExpectedTotalCosts(),
            SectionStatus = financialDetails.SectionStatus,
            IsReadOnly = financialDetails.IsReadOnly(),
        };

        MapPublicGrants(financialDetailsDto, financialDetails.PublicGrants);
        MapExpectedContributions(financialDetailsDto, financialDetails.ExpectedContributions);

        return financialDetailsDto;
    }

    private static void MapPublicGrants(Contract.FinancialDetails.FinancialDetails financialDetailsDto, PublicGrants publicGrants)
    {
        financialDetailsDto.CountyCouncilGrants = publicGrants.CountyCouncil?.Value;
        financialDetailsDto.DhscExtraCareGrants = publicGrants.DhscExtraCare?.Value;
        financialDetailsDto.LocalAuthorityGrants = publicGrants.LocalAuthority?.Value;
        financialDetailsDto.SocialServicesGrants = publicGrants.SocialServices?.Value;
        financialDetailsDto.HealthRelatedGrants = publicGrants.HealthRelated?.Value;
        financialDetailsDto.LotteryFunding = publicGrants.Lottery?.Value;
        financialDetailsDto.OtherPublicGrants = publicGrants.OtherPublicBodies?.Value;
        financialDetailsDto.TotalReceivedGrants = publicGrants.CalculateTotal();
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
        financialDetailsDto.RecycledCapitalGrantFundContribution = expectedContributionsToScheme.RcgfContributions?.Value;
        financialDetailsDto.OtherCapitalContributions = expectedContributionsToScheme.OtherCapitalSources?.Value;
        financialDetailsDto.SharedOwnershipSalesContribution = expectedContributionsToScheme.SharedOwnershipSales?.Value;
        financialDetailsDto.TransferValueOfHomes = expectedContributionsToScheme.HomesTransferValue?.Value;
        financialDetailsDto.TotalExpectedContributions = expectedContributionsToScheme.CalculateTotal();
    }
}
