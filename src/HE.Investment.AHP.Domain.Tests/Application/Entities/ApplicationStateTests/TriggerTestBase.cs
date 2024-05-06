using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationStateTests;

public abstract class TriggerTestBase
{
    protected ApplicationState BuildApplicationState(
        ApplicationStatus applicationStatus,
        bool canEditApplication = true,
        bool canSubmitApplication = true,
        ApplicationStatus? previousApplicationStatus = null,
        bool wasSubmitted = false)
    {
        var userAccount = new Mock<IUserAccount>();
        userAccount.Setup(x => x.CanEditApplication).Returns(canEditApplication);
        userAccount.Setup(x => x.CanSubmitApplication).Returns(canSubmitApplication);

        return new ApplicationState(
            applicationStatus,
            userAccount.Object,
            previousApplicationStatus,
            wasSubmitted);
    }
}
