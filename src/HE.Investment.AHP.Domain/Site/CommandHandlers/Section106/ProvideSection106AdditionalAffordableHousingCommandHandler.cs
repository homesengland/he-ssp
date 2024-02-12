using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Section106;

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
                site.ProvideSection106(site.Section106.WithAdditionalAffordableHousing(request.AdditionalAffordableHousing));
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
