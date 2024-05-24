using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class LocalAuthorityBuilder : TestObjectBuilder<LocalAuthorityBuilder, LocalAuthority>
{
    private LocalAuthorityBuilder()
        : base(LocalAuthority.New("1234", "My authority"))
    {
    }

    protected override LocalAuthorityBuilder Builder => this;

    public static LocalAuthorityBuilder New() => new();
}
