using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Queries;

[SuppressMessage("Code Smell", "S2094", Justification = "Allowed for records.")]
public record GetAvailableProgrammesQuery : IRequest<AvailableProgramme[]>;

public record AvailableProgramme(string ProgrammeId, string ProgrammeName);
