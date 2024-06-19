using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

public class GetFinancialCheckAnswersQueryHandler : IRequestHandler<GetFinancialCheckAnswersQuery, GetFinancialCheckAnswersResult>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    public GetFinancialCheckAnswersQueryHandler(IFinancialDetailsRepository financialDetailsRepository, IConsortiumUserContext accountUserContext)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<GetFinancialCheckAnswersResult> Handle(GetFinancialCheckAnswersQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var financialDetails = await _financialDetailsRepository.GetById(request.ApplicationId, account, cancellationToken);

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
            financialDetails.SchemeFunding.RequiredFunding,
            financialDetails.ExpectedContributions.CalculateTotal(),
            financialDetails.PublicGrants.CalculateTotal(),
            financialDetails.ExpectedTotalContributions());

        return new GetFinancialCheckAnswersResult(
            ApplicationBasicInfoMapper.Map(financialDetails.ApplicationBasicInfo),
            financialDetails.SectionStatus,
            landValueSummary,
            totalSchemeCost,
            totalContributions);
    }
}
