using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public sealed class ProvideSearchOrganisationPhraseCommandHandler : IRequestHandler<ProvideSearchOrganisationPhraseCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideSearchOrganisationPhraseCommand request, CancellationToken cancellationToken)
    {
        if (request.Phrase.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(request.Phrase),
                ValidationErrorMessage.MustProvideRequiredField("organisation name"));
        }

        return Task.FromResult(OperationResult.Success());
    }
}
