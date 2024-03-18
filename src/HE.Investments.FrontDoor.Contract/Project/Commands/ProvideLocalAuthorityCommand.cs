using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideLocalAuthorityCommand(FrontDoorProjectId ProjectId, LocalAuthorityId LocalAuthorityId) : IProvideProjectDetailsCommand;
