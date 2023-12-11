using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using MediatR;

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
        var financialDetails = await _financialDetailsRepository.GetById(ValueObjects.ApplicationId.From(request.ApplicationId), cancellationToken);

        var landValueSummary = new LandValueSummary(
            financialDetails.PurchasePrice?.Value,
            financialDetails.LandValue?.Value,
            financialDetails.IsPublicLand);

        var totalSchemeCost = new TotalSchemeCost(
            financialDetails.PurchasePrice?.Value,
            financialDetails.ExpectedWorksCosts?.Value,
            financialDetails.ExpectedOnCosts?.Value,
            financialDetails.ExpectedTotalCosts());

        var totalContributions = new TotalContributions(
            financialDetails.ExpectedContributions.CalculateTotal(),
            financialDetails.PublicGrants.CalculateTotal(),
            financialDetails.ExpectedTotalContributions());

        return new GetFinancialCheckAnswersResult(
            financialDetails.ApplicationName,
            financialDetails.AreAllQuestionsAnswered(),
            landValueSummary,
            totalSchemeCost,
            totalContributions);
    }
}
