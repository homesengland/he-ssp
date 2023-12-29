using Moq;

namespace HE.Investments.TestsUtils.TestFramework;

public class TestServiceBuilder<T>
    where T : class
{
    public TestServiceBuilder()
    {
        Mock = new Mock<T>();
    }

    public Mock<T> Mock { get; }

    public TestServiceBuilder<T> Register(IRegisterDependency registerDependency)
    {
        registerDependency.RegisterDependency(Build());
        return this;
    }

    public T Build()
    {
        return Mock.Object;
    }
}
