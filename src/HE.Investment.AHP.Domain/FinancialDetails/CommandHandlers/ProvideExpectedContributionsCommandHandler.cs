using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class ProvideExpectedContributionsCommandHandler : FinancialDetailsCommandHandlerBase,
    IRequestHandler<ProvideExpectedContributionsCommand, OperationResult>
{
    private readonly IConsortiumUserContext _accountUserContext;

    public ProvideExpectedContributionsCommandHandler(
        IFinancialDetailsRepository repository,
        IConsortiumUserContext accountUserContext,
        ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, accountUserContext, logger)
    {
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(ProvideExpectedContributionsCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var isUnregisteredBody = account.Organisation?.IsUnregisteredBody == true;

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
                    isUnregisteredBody ? MapProvidedValues(request.HomesTransferValue, ExpectedContributionFields.HomesTransferValue) : null,
                    financialDetails.ApplicationBasicInfo.Tenure,
                    isUnregisteredBody);

                result.CheckErrors();

                financialDetails.ProvideExpectedContributions(contributions);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
