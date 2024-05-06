using HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record AddOrganisationToConsortiumCommand(ConsortiumId ConsortiumId, OrganisationIdentifier? SelectedMember) : IConsortiumCommand;
