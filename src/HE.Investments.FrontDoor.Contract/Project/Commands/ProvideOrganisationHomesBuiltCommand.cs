namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideOrganisationHomesBuiltCommand(FrontDoorProjectId ProjectId, string? OrganisationHomesBuilt) : IProvideProjectDetailsCommand;
