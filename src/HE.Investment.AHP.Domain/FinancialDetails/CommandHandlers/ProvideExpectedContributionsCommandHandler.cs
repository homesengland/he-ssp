using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class ProvideExpectedContributionsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideExpecteContributionsCommand, OperationResult>
{
    public ProvideExpectedContributionsCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideExpecteContributionsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var result = OperationResult.New();

                ExpectedContributionValue? MapProvidedValues(string? value, ExpectedContributionFields field) => value.IsProvided()
                    ? result.CatchResult(() => new ExpectedContributionValue(field, value!))
                    : null;

                var contributions = new ExpectedContributionsToScheme(
                    MapProvidedValues(request.RentalIncomeBorrowing, ExpectedContributionFields.RentalIncomeBorrowing),
                    MapProvidedValues(request.SalesOfHomesOnThisScheme, ExpectedContributionFields.SaleOfHomesOnThisScheme),
                    MapProvidedValues(request.SalesOfHomesOnOtherSchemes, ExpectedContributionFields.SaleOfHomesOnOtherSchemes),
                    MapProvidedValues(request.OwnResources, ExpectedContributionFields.OwnResources),
                    MapProvidedValues(request.RCGFContribution, ExpectedContributionFields.RcgfContribution),
                    MapProvidedValues(request.OtherCapitalSources, ExpectedContributionFields.OtherCapitalSources),
                    MapProvidedValues(request.SharedOwnershipSales, ExpectedContributionFields.SharedOwnershipSales),
                    MapProvidedValues(request.HomesTransferValue, ExpectedContributionFields.HomesTransferValue),
                    financialDetails.ApplicationBasicInfo.Tenure);

                result.CheckErrors();

                financialDetails.ProvideExpectedContributions(contributions);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
