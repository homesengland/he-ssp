namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public enum HomeTypeSectionType
{
    Undefined = 0,
    HousingType,
    HomeInformation,
    DisabledAndVulnerablePeople,
    OlderPeople,
}

internal static class HomeTypeSectionTypes
{
    public static readonly IReadOnlyCollection<HomeTypeSectionType> All = new[]
    {
        HomeTypeSectionType.HousingType,
        HomeTypeSectionType.HomeInformation,
        HomeTypeSectionType.DisabledAndVulnerablePeople,
        HomeTypeSectionType.OlderPeople,
    };
}
