using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class LocationTestData
{
    public const string IncorrectLocationType = "incorrect location type";

    public const string NoCoordinates = "";

    public const string CorrectCoordinates = "coordinates";

    public const string NoLandRegistryNumber = "";

    public const string CorrectLandRegistryNumber = "CorrectLandRegistryNumber";

    public static readonly Coordinates AnyCoordinates = new(CorrectCoordinates);

    public static readonly LandRegistryTitleNumber AnyLandRegistryTitleNumber = new(CorrectLandRegistryNumber);
}
