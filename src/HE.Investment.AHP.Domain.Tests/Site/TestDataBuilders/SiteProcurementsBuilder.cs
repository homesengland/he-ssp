using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteProcurementsBuilder : TestObjectBuilder<SiteProcurementsBuilder, SiteProcurements>
{
    private SiteProcurementsBuilder()
        : base(new SiteProcurements())
    {
    }

    protected override SiteProcurementsBuilder Builder => this;

    public static SiteProcurementsBuilder New() => new();

    public SiteProcurementsBuilder WithProcurements(params SiteProcurement[] value) => SetProperty(x => x.Procurements, value);
}
