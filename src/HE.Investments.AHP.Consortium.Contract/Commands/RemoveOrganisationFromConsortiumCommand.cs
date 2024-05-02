using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record RemoveOrganisationFromConsortiumCommand(ConsortiumId ConsortiumId, OrganisationId OrganisationId, bool? IsConfirmed) : IConsortiumCommand;
