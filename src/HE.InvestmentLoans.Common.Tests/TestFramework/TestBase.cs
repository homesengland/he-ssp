using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace HE.InvestmentLoans.Common.Tests.TestFramework;

public class TestBase<TTestClass>
    where TTestClass : class
{
    private readonly Fixture _fixture = new();

    public TestBase(bool configureMembers = false)
    {
        _fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = configureMembers });
    }

    protected TTestClass TestCandidate => _fixture.Create<TTestClass>();

    protected void RegisterDependency<TDependency>(TDependency dependency)
    {
        _fixture.Inject(dependency);
    }

    protected Mock<TDependency> CreateAndRegisterDependencyMock<TDependency>()
        where TDependency : class
    {
        var mock = new Mock<TDependency>();

        _fixture.Inject(mock.Object);

        return mock;
    }
}
