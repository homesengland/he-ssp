using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class CreateConsortiumCommandHandler : IRequestHandler<CreateConsortiumCommand, OperationResult<string>>
{
    public Task<OperationResult<string>> Handle(CreateConsortiumCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ProgrammeId))
        {
            OperationResult.ThrowValidationError("SelectedProgrammeId", "Please select what programme the consortium is related to");
        }

        return Task.FromResult(new OperationResult<string>("test-id"));
    }
}
