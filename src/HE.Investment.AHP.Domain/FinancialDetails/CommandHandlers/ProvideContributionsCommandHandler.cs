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

                if (request.RentalIncomeBorrowing.IsProvided())
                {
                    var rentalIncomeBorrowing = aggregatedResults.CatchResult(() => new RentalIncomeBorrowing(request.RentalIncomeBorrowing));
                    financialDetails.ProvideRentalIncomeBorrowing(rentalIncomeBorrowing);
                }

                if (request.SalesOfHomesOnThisScheme.IsProvided())
                {
                    var salesOnThisScheme = aggregatedResults.CatchResult(() => new SalesOfHomesOnThisScheme(request.SalesOfHomesOnThisScheme));
                    financialDetails.ProvideSalesOfHomesOnThisScheme(salesOnThisScheme);
                }

                if (request.SalesOfHomesOnOtherSchemes.IsProvided())
                {
                    var salesOnOtherSchemes = aggregatedResults.CatchResult(() => new SalesOfHomesOnOtherSchemes(request.SalesOfHomesOnOtherSchemes));
                    financialDetails.ProvideSalesOfHomesOnOtherSchemes(salesOnOtherSchemes);
                }

                if (request.OwnResources.IsProvided())
                {
                    var ownResources = aggregatedResults.CatchResult(() => new OwnResources(request.OwnResources));
                    financialDetails.ProvideOwnResources(ownResources);
                }

                if (request.RCGFContribution.IsProvided())
                {
                    var rCGFContribution = aggregatedResults.CatchResult(() => new RCGFContribution(request.RCGFContribution));
                    financialDetails.ProvideRCGFContribution(rCGFContribution);
                }

                if (request.OtherCapitalSources.IsProvided())
                {
                    var otherCapitalSources = aggregatedResults.CatchResult(() => new OtherCapitalSources(request.OtherCapitalSources));
                    financialDetails.ProvideOtherCapitalSources(otherCapitalSources);
                }

                if (request.SharedOwnershipSales.IsProvided())
                {
                    var sharedOwnershipSales = aggregatedResults.CatchResult(() => new SharedOwnershipSales(request.SharedOwnershipSales));
                    financialDetails.ProvideSharesOwnershipSales(sharedOwnershipSales);
                }

                if (request.HomesTransferValue.IsProvided())
                {
                    var homesTransferValue = aggregatedResults.CatchResult(() => new HomesTransferValue(request.HomesTransferValue));
                    financialDetails.ProvideHomesTransferValue(homesTransferValue);
                }

                aggregatedResults.CheckErrors();
            },
            request.ApplicationId,
            cancellationToken);
    }
}
