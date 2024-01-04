using HE.Investment.AHP.Contract.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteNameExistMockBuilder
{
    private readonly Mock<ISiteNameExist> _mock;

    private SiteNameExistMockBuilder()
    {
        _mock = new Mock<ISiteNameExist>();
    }

    public static SiteNameExistMockBuilder New() => new();

    public SiteNameExistMockBuilder WithIsExistAsFalse()
    {
        _mock.Setup(x => x.IsExist(It.IsAny<SiteName>(), It.IsAny<SiteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        return this;
    }

    public SiteNameExistMockBuilder WithIsExistAsTrue()
    {
        _mock.Setup(x => x.IsExist(It.IsAny<SiteName>(), It.IsAny<SiteId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        return this;
    }

    public ISiteNameExist BuildObject() => _mock.Object;
}
