using Moq;

namespace HE.Investments.TestsUtils.TestFramework;

public class TestObjectBuilder<T>
    where T : class
{
    public TestObjectBuilder()
    {
        Mock = new Mock<T>();
    }

    public Mock<T> Mock { get; }

    public TestObjectBuilder<T> Register(IRegisterDependency registerDependency)
    {
        registerDependency.RegisterDependency(Build());
        return this;
    }

    public T Build()
    {
        return Mock.Object;
    }
}
