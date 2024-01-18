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

public class ProvideSection106AgreementCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106AgreementCommand, OperationResult>
{
    public ProvideSection106AgreementCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public Task<OperationResult> Handle(ProvideSection106AgreementCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                var section106 = site.Section106 ?? new Section106();
                section106.ProvideGeneralAgreement(request.Agreement);
                site.ProvideSection106(section106);
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
