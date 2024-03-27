using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Data;

public record ProjectPrefillData(
    FrontDoorProjectId Id,
    string Name,
    IReadOnlyCollection<SupportActivityType> SupportActivities,
    FrontDoorSiteId? SiteId);
