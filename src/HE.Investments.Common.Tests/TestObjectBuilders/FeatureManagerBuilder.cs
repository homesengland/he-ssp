using HE.Investments.TestsUtils.TestFramework;
using Microsoft.FeatureManagement;
using Moq;

namespace HE.Investments.Common.Tests.TestObjectBuilders;

public class FeatureManagerBuilder
{
    private readonly Mock<IFeatureManager> _mock;

    private FeatureManagerBuilder()
    {
        _mock = new Mock<IFeatureManager>();
    }

    public static FeatureManagerBuilder New() => new();

    public FeatureManagerBuilder AlwaysReturnsTrue()
    {
        _mock.Setup(x => x.IsEnabledAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(true);

        _mock.Setup(x => x.IsEnabledAsync(
                It.IsAny<string>()))
            .ReturnsAsync(true);

        return this;
    }

    public IFeatureManager Build()
    {
        return _mock.Object;
    }

    public FeatureManagerBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }
}
