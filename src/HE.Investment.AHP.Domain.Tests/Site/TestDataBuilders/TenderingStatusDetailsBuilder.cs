using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class TenderingStatusDetailsBuilder : TestObjectBuilder<TenderingStatusDetailsBuilder, TenderingStatusDetails>
{
    private TenderingStatusDetailsBuilder()
        : base(new TenderingStatusDetails())
    {
    }

    protected override TenderingStatusDetailsBuilder Builder => this;

    public static TenderingStatusDetailsBuilder New() => new();

    public TenderingStatusDetailsBuilder WithStatus(SiteTenderingStatus value) => SetProperty(x => x.TenderingStatus, value);
}
