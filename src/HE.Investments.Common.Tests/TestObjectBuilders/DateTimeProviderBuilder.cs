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

    public DateTimeProviderBuilder ReturnDate(DateTime date)
    {
        _mock.Setup(d => d.UtcNow)
            .Returns(date);

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
