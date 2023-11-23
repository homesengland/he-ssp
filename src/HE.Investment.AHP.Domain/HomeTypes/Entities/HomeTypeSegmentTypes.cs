namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

internal static class HomeTypeSegmentTypes
{
    public static readonly IReadOnlyCollection<HomeTypeSegmentType> None = Array.Empty<HomeTypeSegmentType>();

    public static readonly IReadOnlyCollection<HomeTypeSegmentType> All = new[]
    {
        HomeTypeSegmentType.HomeInformation,
        HomeTypeSegmentType.DisabledAndVulnerablePeople,
        HomeTypeSegmentType.OlderPeople,
        HomeTypeSegmentType.SupportedHousingInformation,
    };
}
