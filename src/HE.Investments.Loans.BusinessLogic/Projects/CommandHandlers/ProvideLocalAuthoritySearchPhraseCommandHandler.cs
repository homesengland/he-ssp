using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideLocalAuthoritySearchPhraseCommandHandler : IRequestHandler<ProvideLocalAuthoritySearchPhraseCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideLocalAuthoritySearchPhraseCommand request, CancellationToken cancellationToken)
    {
        if (request.Phrase.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(LocalAuthoritiesViewModel.Phrase), ValidationErrorMessage.LocalAuthorityNameIsEmpty)
                .CheckErrors();
        }

        return Task.FromResult(OperationResult.Success());
    }
}
