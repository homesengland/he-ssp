using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class LandAcquisitionStatusBuilder : TestObjectBuilder<LandAcquisitionStatusBuilder, LandAcquisitionStatus>
{
    private LandAcquisitionStatusBuilder()
        : base(new LandAcquisitionStatus())
    {
    }

    protected override LandAcquisitionStatusBuilder Builder => this;

    public static LandAcquisitionStatusBuilder New() => new();

    public LandAcquisitionStatusBuilder WithStatus(SiteLandAcquisitionStatus value) => SetProperty(x => x.Value, value);
}
