using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class Section106Builder : TestObjectBuilder<Section106Builder, Section106>
{
    public Section106Builder()
        : base(new Section106())
    {
    }

    protected override Section106Builder Builder => this;

    public static Section106Builder New() => new();

    public Section106Builder WithGeneralAgreement(bool? value) => SetProperty(x => x.GeneralAgreement, value);
}
