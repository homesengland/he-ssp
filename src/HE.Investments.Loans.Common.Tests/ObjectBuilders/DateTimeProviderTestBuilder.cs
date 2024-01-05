using HE.Investments.Common.Utils;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Loans.Common.Tests.ObjectBuilders;

public class DateTimeProviderTestBuilder : IDependencyTestBuilder<IDateTimeProvider>
{
    private readonly Mock<IDateTimeProvider> _mock = new();

    public static DateTimeProviderTestBuilder New() => new();

    public DateTimeProviderTestBuilder ReturnsAsNow(DateTime date)
    {
        _mock.Setup(c => c.Now).Returns(date);

        return this;
    }

    public IDateTimeProvider Build()
    {
        return _mock.Object;
    }
}
