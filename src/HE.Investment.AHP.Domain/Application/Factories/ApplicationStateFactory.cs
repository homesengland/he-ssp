using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.Factories;

public class ApplicationStateFactory : IApplicationStateFactory
{
    private readonly IUserAccount _userAccount;

    private readonly ApplicationStatus? _previousStatus;

    public ApplicationStateFactory(IUserAccount userAccount, ApplicationStatus? previousStatus = null)
    {
        _userAccount = userAccount;
        _previousStatus = previousStatus;
    }

    public ApplicationState Create(ApplicationStatus status)
    {
        return new ApplicationState(status, _userAccount, _previousStatus);
    }
}
