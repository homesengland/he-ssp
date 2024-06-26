using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideLocalAuthorityCommand, OperationResult>
{
    public ProvideLocalAuthorityCommandHandler(
        ISiteRepository siteRepository,
        IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
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

                if (request.LocalAuthorityCode.IsNotProvided() || request.LocalAuthorityName.IsNotProvided())
                {
                    site.ProvideLocalAuthority(null);
                }
                else
                {
                    site.ProvideLocalAuthority(LocalAuthority.New(request.LocalAuthorityCode!, request.LocalAuthorityName!));
                }

                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
