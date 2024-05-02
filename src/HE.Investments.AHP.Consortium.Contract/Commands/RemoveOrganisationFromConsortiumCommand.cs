using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record RemoveOrganisationFromConsortiumCommand(ConsortiumId ConsortiumId, OrganisationId OrganisationId, bool? IsConfirmed) : IConsortiumCommand;
