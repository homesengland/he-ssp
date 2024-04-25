using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class RemoveOrganisationFromConsortiumCommandHandler : IRequestHandler<RemoveOrganisationFromConsortiumCommand, OperationResult>
{
    public Task<OperationResult> Handle(RemoveOrganisationFromConsortiumCommand request, CancellationToken cancellationToken)
    {
        if (request.IsConfirmed.IsNotProvided())
        {
            // TODO: move to domain object
            OperationResult.ThrowValidationError(nameof(request.IsConfirmed), "Select whether you want to remove this organisation from consortium");
        }

        return Task.FromResult(OperationResult.Success());
    }
}
