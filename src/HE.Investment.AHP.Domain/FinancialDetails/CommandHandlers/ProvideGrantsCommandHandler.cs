using HE.Investment.AHP.Domain.FinancialDetails.Commands;
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
                var grants = new Grants(
                                request.CountyCouncilGrants,
                                request.DHSCExtraCareGrants,
                                request.LocalAuthorityGrants,
                                request.SocialServicesGrants,
                                request.HealthRelatedGrants,
                                request.LotteryGrants,
                                request.OtherPublicBodiesGrants);

                financialDetails.ProvideGrants(grants);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
