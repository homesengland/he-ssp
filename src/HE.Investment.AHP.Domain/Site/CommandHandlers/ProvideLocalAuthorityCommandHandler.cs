extern alias Org;

using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideLocalAuthorityCommand, OperationResult>
{
    public ProvideLocalAuthorityCommandHandler(
        ISiteRepository siteRepository,
        IAccountUserContext accountUserContext,
        ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLocalAuthorityCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            site =>
            {
                if (request.Response == null)
                {
                    OperationResult.New()
                        .AddValidationError(nameof(request.Response), "Select if this is the correct local authority")
                        .CheckErrors();
                }

                if (request.LocalAuthorityId.IsNotProvided() || request.LocalAuthorityName.IsNotProvided())
                {
                    site.ProvideLocalAuthority(null);
                }
                else
                {
                    site.ProvideLocalAuthority(LocalAuthority.New(request.LocalAuthorityId!, request.LocalAuthorityName!));
                }

                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
