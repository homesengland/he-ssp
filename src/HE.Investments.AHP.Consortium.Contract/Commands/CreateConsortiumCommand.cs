using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Programme.Contract;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record CreateConsortiumCommand(ProgrammeId? ProgrammeId) : IRequest<OperationResult<ConsortiumId>>;
