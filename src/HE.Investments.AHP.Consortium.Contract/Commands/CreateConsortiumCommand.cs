using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record CreateConsortiumCommand(string ProgrammeId) : IRequest;
