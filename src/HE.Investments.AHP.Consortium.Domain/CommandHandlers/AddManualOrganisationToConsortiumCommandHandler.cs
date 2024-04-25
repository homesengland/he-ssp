using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class AddManualOrganisationToConsortiumCommandHandler : IRequestHandler<AddManualOrganisationToConsortiumCommand, OperationResult>
{
    public Task<OperationResult> Handle(AddManualOrganisationToConsortiumCommand request, CancellationToken cancellationToken)
    {
#pragma warning disable S1481
        var organisation = Organisation.CreateManual(request.Name, request.AddressLine1, request.AddressLine2, request.TownOrCity, request.County, request.Postcode);
#pragma warning restore S1481

        // TODO: save organisation in CRM + add to consortium
        return Task.FromResult(OperationResult.Success());
    }
}
