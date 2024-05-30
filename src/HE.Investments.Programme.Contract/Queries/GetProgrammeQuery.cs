using MediatR;

namespace HE.Investments.Programme.Contract.Queries;

public record GetProgrammeQuery(ProgrammeId ProgrammeId) : IRequest<Programme>;
