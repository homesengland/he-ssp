using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvideContributionsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideContributionsCommand, OperationResult>
{
    public ProvideContributionsCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideContributionsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var aggregatedResults = OperationResult.New();

                var rentalIncomeBorrowing = request.RentalIncomeBorrowing.IsProvided() ? aggregatedResults.CatchResult(() => new RentalIncomeBorrowing(request.RentalIncomeBorrowing)) : null;
                var salesOfHomesOnThisScheme = request.SalesOfHomesOnThisScheme.IsProvided() ? aggregatedResults.CatchResult(() => new SalesOfHomesOnThisScheme(request.SalesOfHomesOnThisScheme)) : null;
                var salesOfHomesOnOtherSchemes = request.SalesOfHomesOnOtherSchemes.IsProvided() ? aggregatedResults.CatchResult(() => new SalesOfHomesOnOtherSchemes(request.SalesOfHomesOnOtherSchemes)) : null;
                var ownResources = request.OwnResources.IsProvided() ? aggregatedResults.CatchResult(() => new OwnResources(request.OwnResources)) : null;
                var rCGFContribution = request.RCGFContribution.IsProvided() ? aggregatedResults.CatchResult(() => new RCGFContribution(request.RCGFContribution)) : null;
                var otherCapitalSources = request.OtherCapitalSources.IsProvided() ? aggregatedResults.CatchResult(() => new OtherCapitalSources(request.OtherCapitalSources)) : null;
                var sharedOwnershipSales = request.SharedOwnershipSales.IsProvided() ? aggregatedResults.CatchResult(() => new SharedOwnershipSales(request.SharedOwnershipSales)) : null;
                var homesTransferValue = request.HomesTransferValue.IsProvided() ? aggregatedResults.CatchResult(() => new HomesTransferValue(request.HomesTransferValue)) : null;

                aggregatedResults.CheckErrors();

                financialDetails.ProvideContributions(
                    rentalIncomeBorrowing,
                    salesOfHomesOnThisScheme,
                    salesOfHomesOnOtherSchemes,
                    ownResources,
                    rCGFContribution,
                    otherCapitalSources,
                    sharedOwnershipSales,
                    homesTransferValue);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
