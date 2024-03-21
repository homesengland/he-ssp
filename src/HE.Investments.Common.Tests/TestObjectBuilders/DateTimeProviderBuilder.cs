using HE.Investments.Common.Utils;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Common.Tests.TestObjectBuilders;

public class DateTimeProviderBuilder
{
    private readonly Mock<IDateTimeProvider> _mock;

    private DateTimeProviderBuilder()
    {
        _mock = new Mock<IDateTimeProvider>();
    }

    public static DateTimeProviderBuilder New() => new();

    public DateTimeProviderBuilder ReturnCurrentDate()
    {
        _mock.Setup(d => d.UtcNow)
            .Returns(new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Unspecified));

        return this;
    }

    public IDateTimeProvider Build()
    {
        return _mock.Object;
    }

    public DateTimeProviderBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }
}
