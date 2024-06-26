using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investments.TestsUtils.TestFramework;
using SiteModernMethodsOfConstruction = HE.Investment.AHP.Domain.Site.ValueObjects.Mmc.SiteModernMethodsOfConstruction;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteModernMethodsOfConstructionBuilder : TestObjectBuilder<SiteModernMethodsOfConstructionBuilder, SiteModernMethodsOfConstruction>
{
    private SiteModernMethodsOfConstructionBuilder()
        : base(new SiteModernMethodsOfConstruction())
    {
    }

    protected override SiteModernMethodsOfConstructionBuilder Builder => this;

    public static SiteModernMethodsOfConstructionBuilder New() => new();

    public SiteModernMethodsOfConstructionBuilder WithIsUsingMmc(SiteUsingModernMethodsOfConstruction value) => SetProperty(x => x.SiteUsingModernMethodsOfConstruction, value);

    public SiteModernMethodsOfConstructionBuilder WithInformation(string barriers, string impact) => SetProperty(
        x => x.Information,
        new ModernMethodsOfConstructionInformation(new ModernMethodsOfConstructionBarriers(barriers), new ModernMethodsOfConstructionImpact(impact)));
}
