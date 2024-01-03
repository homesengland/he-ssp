using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

internal sealed class CalculateGrantsQueryHandler : CalculateQueryHandlerBase,
    IRequestHandler<CalculateGrantsQuery, (OperationResult OperationResult, CalculationResult CalculationResult)>
{
    public CalculateGrantsQueryHandler(
        IFinancialDetailsRepository financialDetailsRepository,
        IAccountUserContext accountUserContext,
        ILogger<CalculateGrantsQueryHandler> logger)
        : base(financialDetailsRepository, accountUserContext, logger)
    {
    }

    public async Task<(OperationResult OperationResult, CalculationResult CalculationResult)> Handle(
        CalculateGrantsQuery request,
        CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var result = OperationResult.New();

                PublicGrantValue? MapProvidedValues(string? value, PublicGrantFields field) => value.IsProvided()
                    ? result.CatchResult(() => new PublicGrantValue(field, value!))
                    : null;

                var publicGrants = new PublicGrants(
                    MapProvidedValues(request.CountyCouncilGrants, PublicGrantFields.CountyCouncilGrants),
                    MapProvidedValues(request.DhscExtraCareGrants, PublicGrantFields.DhscExtraCareGrants),
                    MapProvidedValues(request.LocalAuthorityGrants, PublicGrantFields.LocalAuthorityGrants),
                    MapProvidedValues(request.SocialServicesGrants, PublicGrantFields.SocialServicesGrants),
                    MapProvidedValues(request.HealthRelatedGrants, PublicGrantFields.HealthRelatedGrants),
                    MapProvidedValues(request.LotteryGrants, PublicGrantFields.LotteryGrants),
                    MapProvidedValues(request.OtherPublicBodiesGrants, PublicGrantFields.OtherPublicBodiesGrants));

                result.CheckErrors();

                var total = publicGrants.CalculateTotal();

                return new CalculationResult(total, null);
            },
            ApplicationId.From(request.ApplicationId),
            cancellationToken);
    }
}
