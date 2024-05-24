using HE.Investments.Programme.Contract.Enums;
using MediatR;

namespace HE.Investments.Programme.Contract.Queries;

public record GetProgrammesQuery(ProgrammeType ProgrammeType) : IRequest<IList<Programme>>;
