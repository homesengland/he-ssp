using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.CommandHandlers;

public class ProvideOrganisationSearchPhraseCommandHandler : IRequestHandler<ProvideOrganisationSearchPhraseCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideOrganisationSearchPhraseCommand request, CancellationToken cancellationToken)
    {
        if (request.Name.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(OrganisationSearchModel.Name), ValidationErrorMessage.OrganisationNameIsEmpty)
                .CheckErrors();
        }

        return Task.FromResult(OperationResult.Success());
    }
}
