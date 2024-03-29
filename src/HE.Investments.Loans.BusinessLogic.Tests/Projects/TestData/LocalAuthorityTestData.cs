extern alias Org;

using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;

internal static class LocalAuthorityTestData
{
    public static readonly LocalAuthority LocalAuthorityOne = new(LocalAuthorityCode.From("1"), "Liverpool");
    public static readonly LocalAuthority LocalAuthorityTwo = new(LocalAuthorityCode.From("2"), "London");
    public static readonly LocalAuthority LocalAuthorityThree = new(LocalAuthorityCode.From("3"), "Manchester");
    public static readonly LocalAuthority LocalAuthorityFour = new(LocalAuthorityCode.From("4"), "Brighton");
    public static readonly LocalAuthority LocalAuthorityFive = new(LocalAuthorityCode.From("5"), "Leicester");

    public static readonly IList<LocalAuthority> LocalAuthoritiesList = new List<LocalAuthority>
    {
        LocalAuthorityOne,
        LocalAuthorityTwo,
        LocalAuthorityThree,
        LocalAuthorityFour,
        LocalAuthorityFive,
    };
}
