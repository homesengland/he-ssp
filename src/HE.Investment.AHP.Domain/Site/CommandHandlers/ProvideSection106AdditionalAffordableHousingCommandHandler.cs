using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using Section106 = HE.Investment.AHP.Domain.Site.ValueObjects.Section106;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSection106AdditionalAffordableHousingCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106AdditionalAffordableHousingCommand, OperationResult>
{
    public ProvideSection106AdditionalAffordableHousingCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    public Task<OperationResult> Handle(ProvideSection106AdditionalAffordableHousingCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                var currentSection106 = site.Section106;
                var newSection106 = new Section106(
                                            currentSection106.GeneralAgreement,
                                            currentSection106.AffordableHousing,
                                            currentSection106.OnlyAffordableHousing,
                                            request.AdditionalAffordableHousing,
                                            currentSection106.CapitalFundingEligibility,
                                            currentSection106.LocalAuthorityConfirmation);

                site.ProvideSection106(newSection106);
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
