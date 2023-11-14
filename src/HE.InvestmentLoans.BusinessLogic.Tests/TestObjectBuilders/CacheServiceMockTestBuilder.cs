using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;

public class CacheServiceMockTestBuilder
{
    private readonly Mock<ICacheService> _mock;

    private CacheServiceMockTestBuilder()
    {
        _mock = new Mock<ICacheService>();
    }

    public static CacheServiceMockTestBuilder New() => new();

    public CacheServiceMockTestBuilder MockSearchLocalAuthorityRequest(IList<LocalAuthority> response)
    {
        _mock
            .Setup(x => x.GetValueAsync("local-authorities", It.IsAny<Func<Task<IList<LocalAuthority>>>>()))
            .ReturnsAsync(response);

        return this;
    }

    public Mock<ICacheService> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }

    public ICacheService Build()
    {
        return _mock.Object;
    }
}
