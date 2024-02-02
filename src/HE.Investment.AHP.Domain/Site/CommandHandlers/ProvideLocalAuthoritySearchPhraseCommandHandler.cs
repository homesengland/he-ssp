using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideLocalAuthoritySearchPhraseCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideLocalAuthoritySearchPhraseCommand, OperationResult>
{
    public ProvideLocalAuthoritySearchPhraseCommandHandler(
        ISiteRepository siteRepository,
        IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideLocalAuthoritySearchPhraseCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            _ =>
            {
                if (request.Phrase.IsNotProvided())
                {
                    OperationResult
                        .New()
                        .AddValidationError(nameof(LocalAuthorities.Phrase), ValidationErrorMessage.LocalAuthorityNameIsEmpty)
                        .CheckErrors();
                }

                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
