using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class StrategicSiteNameExistMockBuilder
{
    private readonly Mock<IStrategicSiteNameExists> _mock;

    private StrategicSiteNameExistMockBuilder()
    {
        _mock = new Mock<IStrategicSiteNameExists>();
    }

    public static StrategicSiteNameExistMockBuilder New() => new();

    public StrategicSiteNameExistMockBuilder WithIsExistAsFalse()
    {
        _mock.Setup(x => x.IsExist(It.IsAny<StrategicSiteName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        return this;
    }

    public StrategicSiteNameExistMockBuilder WithIsExistAsTrue()
    {
        _mock.Setup(x => x.IsExist(It.IsAny<StrategicSiteName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        return this;
    }

    public IStrategicSiteNameExists BuildObject() => _mock.Object;
}
