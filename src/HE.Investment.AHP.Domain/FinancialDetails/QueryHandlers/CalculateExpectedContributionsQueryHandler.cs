using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

internal sealed class CalculateExpectedContributionsQueryHandler : CalculateQueryHandlerBase,
    IRequestHandler<CalculateExpectedContributionsQuery, (OperationResult OperationResult, CalculationResult CalculationResult)>
{
    public CalculateExpectedContributionsQueryHandler(
        IFinancialDetailsRepository financialDetailsRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<CalculateExpectedContributionsQueryHandler> logger)
        : base(financialDetailsRepository, accountUserContext, logger)
    {
    }

    public async Task<(OperationResult OperationResult, CalculationResult CalculationResult)> Handle(
        CalculateExpectedContributionsQuery request,
        CancellationToken cancellationToken)
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
                    MapProvidedValues(request.RcgfContribution, ExpectedContributionFields.RcgfContribution),
                    MapProvidedValues(request.OtherCapitalSources, ExpectedContributionFields.OtherCapitalSources),
                    MapProvidedValues(request.SharedOwnershipSales, ExpectedContributionFields.SharedOwnershipSales),
                    MapProvidedValues(request.HomesTransferValue, ExpectedContributionFields.HomesTransferValue),
                    financialDetails.ApplicationBasicInfo.Tenure);

                result.CheckErrors();

                var total = contributions.CalculateTotal();

                return new CalculationResult(null, total);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
