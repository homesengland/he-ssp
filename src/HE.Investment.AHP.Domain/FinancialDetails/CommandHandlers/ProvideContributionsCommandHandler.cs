using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common;
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
                var contributions = new Contributions(
                    request.RentalIncomeBorrowing,
                    request.SalesOfHomesOnThisScheme,
                    request.SalesOfHomesOnOtherSchemes,
                    request.OwnResources,
                    request.RCGFContribution,
                    request.OtherCapitalSources,
                    request.SharedOwnershipSales,
                    request.HomesTransferValue);

                financialDetails.ProvideContributions(contributions);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
