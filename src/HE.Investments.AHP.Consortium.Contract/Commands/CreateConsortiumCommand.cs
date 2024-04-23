using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record CreateConsortiumCommand(string Name, string ProgrammeId) : IRequest;
