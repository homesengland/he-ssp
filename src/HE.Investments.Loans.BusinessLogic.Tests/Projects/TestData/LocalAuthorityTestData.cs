extern alias Org;

using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;
using LocalAuthorityId = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthorityId;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class LocalAuthorityTestData
{
    public static readonly LocalAuthority LocalAuthorityOne = new(LocalAuthorityId.From("1"), "Liverpool");
    public static readonly LocalAuthority LocalAuthorityTwo = new(LocalAuthorityId.From("2"), "London");
    public static readonly LocalAuthority LocalAuthorityThree = new(LocalAuthorityId.From("3"), "Manchester");
    public static readonly LocalAuthority LocalAuthorityFour = new(LocalAuthorityId.From("4"), "Brighton");
    public static readonly LocalAuthority LocalAuthorityFive = new(LocalAuthorityId.From("5"), "Leicester");

    public static readonly IList<LocalAuthority> LocalAuthoritiesList = new List<LocalAuthority>
    {
        LocalAuthorityOne,
        LocalAuthorityTwo,
        LocalAuthorityThree,
        LocalAuthorityFour,
        LocalAuthorityFive,
    };
}
