using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSection106CapitalFundingEligibilityCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106CapitalFundingEligibilityCommand, OperationResult>
{
    public ProvideSection106CapitalFundingEligibilityCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    public Task<OperationResult> Handle(ProvideSection106CapitalFundingEligibilityCommand request, CancellationToken cancellationToken)
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
                                            request.CapitalFundingEligibility,
                                            currentSection106.LocalAuthorityConfirmation);

                site.ProvideSection106(newSection106);
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
