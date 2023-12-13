using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

public class GetFinancialCheckAnswersQueryHandler : IRequestHandler<GetFinancialCheckAnswersQuery, GetFinancialCheckAnswersResult>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public GetFinancialCheckAnswersQueryHandler(IFinancialDetailsRepository financialDetailsRepository)
    {
        _financialDetailsRepository = financialDetailsRepository;
    }

    public async Task<GetFinancialCheckAnswersResult> Handle(GetFinancialCheckAnswersQuery request, CancellationToken cancellationToken)
    {
        var financialDetails = await _financialDetailsRepository.GetById(ApplicationId.From(request.ApplicationId), cancellationToken);

        var landValueSummary = new LandValueSummary(
            financialDetails.LandStatus.PurchasePrice?.Value ?? financialDetails.LandStatus.ExpectedPurchasePrice?.Value,
            financialDetails.LandValue.CurrentLandValue?.Value,
            financialDetails.LandValue.IsPublicLand);

        var totalSchemeCost = new TotalSchemeCost(
            financialDetails.LandValue.CurrentLandValue?.Value,
            financialDetails.OtherApplicationCosts.ExpectedWorksCosts?.Value,
            financialDetails.OtherApplicationCosts.ExpectedOnCosts?.Value,
            financialDetails.ExpectedTotalCosts());

        var totalContributions = new TotalContributions(
            financialDetails.ExpectedContributions.CalculateTotal(),
            financialDetails.PublicGrants.CalculateTotal(),
            financialDetails.ExpectedTotalContributions());

        return new GetFinancialCheckAnswersResult(
            financialDetails.ApplicationBasicInfo.Name.Name,
            financialDetails.SectionStatus,
            landValueSummary,
            totalSchemeCost,
            totalContributions);
    }
}
