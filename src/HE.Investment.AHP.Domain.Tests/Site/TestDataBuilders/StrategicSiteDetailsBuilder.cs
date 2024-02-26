using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class StrategicSiteDetailsBuilder : TestObjectBuilder<StrategicSiteDetailsBuilder, StrategicSiteDetails>
{
    private StrategicSiteDetailsBuilder()
        : base(new StrategicSiteDetails())
    {
    }

    protected override StrategicSiteDetailsBuilder Builder => this;

    public static StrategicSiteDetailsBuilder New() => new();

    public StrategicSiteDetailsBuilder WithIsStrategicSite(bool value) => SetProperty(x => x.IsStrategicSite, value);
}
