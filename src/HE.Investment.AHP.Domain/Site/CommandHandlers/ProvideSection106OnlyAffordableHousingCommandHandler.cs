using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSection106OnlyAffordableHousingCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106OnlyAffordableHousingCommand, OperationResult>
{
    public ProvideSection106OnlyAffordableHousingCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    public Task<OperationResult> Handle(ProvideSection106OnlyAffordableHousingCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                var currentSection106 = site.Section106;
                var newSection106 = new Section106(
                                            currentSection106.GeneralAgreement,
                                            currentSection106.AffordableHousing,
                                            request.OnlyAffordableHousing,
                                            currentSection106.AdditionalAffordableHousing,
                                            currentSection106.CapitalFundingEligibility,
                                            currentSection106.LocalAuthorityConfirmation);

                site.ProvideSection106(newSection106);

                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
