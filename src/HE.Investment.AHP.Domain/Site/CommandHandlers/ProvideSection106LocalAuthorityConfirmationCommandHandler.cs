using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSection106LocalAuthorityConfirmationCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106LocalAuthorityConfirmationCommand, OperationResult>
{
    public ProvideSection106LocalAuthorityConfirmationCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public Task<OperationResult> Handle(ProvideSection106LocalAuthorityConfirmationCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                var currentSection106 = site.Section106;
                var newSection106 = new Section106(
                                            currentSection106.GeneralAgreement,
                                            currentSection106.AffordableHousing,
                                            currentSection106.OnlyAffordableHousing,
                                            currentSection106.AdditionalAffordableHousing,
                                            currentSection106.CapitalFundingEligibility,
                                            request.LocalAuthorityConfirmation);

                site.ProvideSection106(newSection106);
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}