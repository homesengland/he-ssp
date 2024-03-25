using HE.Investments.Common.Infrastructure.Events;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;

public class EventDispatcherTestBuilder
{
    private readonly Mock<IEventDispatcher> _mock;

    private EventDispatcherTestBuilder()
    {
        _mock = new Mock<IEventDispatcher>();
    }

    public static EventDispatcherTestBuilder New() => new();

    public IEventDispatcher Build()
    {
        return _mock.Object;
    }
}
