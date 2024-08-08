using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Site;

public record SiteBasicModel(SiteId Id, string Name, FrontDoorProjectId FrontDoorProjectId, string? LocalAuthorityName, SiteStatus Status);
