using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteRuralClassificationBuilder : TestObjectBuilder<SiteRuralClassificationBuilder, SiteRuralClassification>
{
    private SiteRuralClassificationBuilder()
        : base(new SiteRuralClassification())
    {
    }

    protected override SiteRuralClassificationBuilder Builder => this;

    public static SiteRuralClassificationBuilder New() => new();

    public SiteRuralClassificationBuilder WithIsWithinRuralSettlement(bool value) => SetProperty(x => x.IsWithinRuralSettlement, value);

    public SiteRuralClassificationBuilder WithIsRuralExceptionSite(bool value) => SetProperty(x => x.IsRuralExceptionSite, value);
}
