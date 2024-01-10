using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class ProvideGrantsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideGrantsCommand, OperationResult>
{
    public ProvideGrantsCommandHandler(IFinancialDetailsRepository repository, IAccountUserContext accountUserContext, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideGrantsCommand request, CancellationToken cancellationToken)
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

                financialDetails.ProvideGrants(publicGrants);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
