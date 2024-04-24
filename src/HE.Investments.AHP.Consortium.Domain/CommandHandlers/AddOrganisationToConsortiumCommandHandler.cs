using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class AddOrganisationToConsortiumCommandHandler : IRequestHandler<AddOrganisationToConsortiumCommand, OperationResult>
{
    public Task<OperationResult> Handle(AddOrganisationToConsortiumCommand request, CancellationToken cancellationToken)
    {
        if (request.OrganisationId.IsNotProvided() && request.CompanyHouseNumber.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                "SelectedItem",
                ValidationErrorMessage.MustBeSelected("organisation"));
        }

        // TODO: validate whether organisation is already added to consortium
        return Task.FromResult(OperationResult.Success());
    }
}
