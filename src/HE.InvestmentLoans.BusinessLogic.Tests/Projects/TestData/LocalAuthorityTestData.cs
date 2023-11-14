using HE.InvestmentLoans.Contract.Projects.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal sealed class LocalAuthorityTestData
{
    public static readonly LocalAuthority LocalAuthorityOne = new("1", "Liverpool");
    public static readonly LocalAuthority LocalAuthorityTwo = new("2", "London");
    public static readonly LocalAuthority LocalAuthorityThree = new("3", "Manchester");
    public static readonly LocalAuthority LocalAuthorityFour = new("4", "Brighton");
    public static readonly LocalAuthority LocalAuthorityFive = new("5", "Leicester");

    public static readonly IList<LocalAuthority> LocalAuthoritiesList = new List<LocalAuthority>
    {
        LocalAuthorityOne,
        LocalAuthorityTwo,
        LocalAuthorityThree,
        LocalAuthorityFour,
        LocalAuthorityFive,
    };
}
