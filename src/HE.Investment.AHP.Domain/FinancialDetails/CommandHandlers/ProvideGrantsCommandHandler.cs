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
public class ProvideGrantsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideGrantsCommand, OperationResult>
{
    public ProvideGrantsCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
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
                    MapProvidedValues(request.DHSCExtraCareGrants, PublicGrantFields.DhscExtraCareGrants),
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
