using HE.Investments.Common.Services.Notifications;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;

public class NotificationServiceMockTestBuilder
{
    private readonly Mock<INotificationService> _mock;

    private NotificationServiceMockTestBuilder()
    {
        _mock = new Mock<INotificationService>();
    }

    public static NotificationServiceMockTestBuilder New() => new();

    public Mock<INotificationService> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }

    public INotificationService Build()
    {
        return _mock.Object;
    }
}
