using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Application.Factories;

public class ApplicationStateFactory : IApplicationStateFactory
{
    private readonly IConsortiumUserAccount _userAccount;

    private readonly ApplicationStatus? _previousStatus;

    private readonly bool _wasSubmitted;

    public ApplicationStateFactory(IConsortiumUserAccount userAccount, ApplicationStatus? previousStatus = null, bool? wasSubmitted = null)
    {
        _userAccount = userAccount;
        _previousStatus = previousStatus;
        _wasSubmitted = wasSubmitted ?? false;
    }

    public ApplicationState Create(ApplicationStatus status)
    {
        return new ApplicationState(status, _previousStatus, _userAccount.CanEdit, _userAccount.CanSubmit, _wasSubmitted);
    }
}
