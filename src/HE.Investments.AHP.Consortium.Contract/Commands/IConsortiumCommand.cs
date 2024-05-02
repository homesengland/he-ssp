using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public interface IConsortiumCommand : IRequest<OperationResult>
{
    ConsortiumId ConsortiumId { get; }
}
