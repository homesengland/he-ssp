using HE.InvestmentLoans.Common.Utils;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.Common.Tests.ObjectBuilders;
public class DateTimeProviderTestBuilder : IDependencyTestBuilder<IDateTimeProvider>
{
    private readonly Mock<IDateTimeProvider> _mock;

    public DateTimeProviderTestBuilder()
    {
        _mock = new Mock<IDateTimeProvider>();
    }

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
