using HE.Investment.AHP.Contract.Application;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using Stateless;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationState
{
    private readonly IUserAccount _userAccount;

    private readonly ApplicationStatus? _previousStatus;

    private readonly StateMachine<ApplicationStatus, AhpApplicationOperation> _machine;

    public ApplicationState(ApplicationStatus currentStatus, IUserAccount userAccount, ApplicationStatus? previousStatus)
    {
        _userAccount = userAccount;
        _previousStatus = previousStatus;
        _machine = new StateMachine<ApplicationStatus, AhpApplicationOperation>(currentStatus);
        ConfigureTransitions();
    }

    public IEnumerable<AhpApplicationOperation> AllowedOperations => _machine.PermittedTriggers;

    public ApplicationStatus Trigger(AhpApplicationOperation operation)
    {
        if (!AllowedOperations.Contains(operation))
        {
            throw new DomainException($"The application cannot be {operation}", $"ApplicationCannotBe{operation}");
        }

        _machine.Fire(operation);
        return _machine.State;
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ApplicationStatus.Draft)
            .PermitReentryIf(AhpApplicationOperation.Modification, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Deleted, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Submit, ApplicationStatus.ApplicationSubmitted, () => _userAccount.CanSubmitApplication);

        _machine.Configure(ApplicationStatus.ApplicationSubmitted)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _userAccount.CanEditApplication);

        _machine.Configure(ApplicationStatus.UnderReview)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _userAccount.CanEditApplication);

        _machine.Configure(ApplicationStatus.ApplicationUnderReview)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _userAccount.CanEditApplication);

        _machine.Configure(ApplicationStatus.OnHold)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.Draft, () => _previousStatus == ApplicationStatus.Draft && _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.ApplicationSubmitted, () => _previousStatus == ApplicationStatus.ApplicationSubmitted && _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.UnderReview, () => _previousStatus == ApplicationStatus.UnderReview && _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.ApplicationUnderReview, () => _previousStatus == ApplicationStatus.ApplicationUnderReview && _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.CashflowUnderReview, () => _previousStatus == ApplicationStatus.CashflowUnderReview && _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.ReferredBackToApplicant, () => _previousStatus == ApplicationStatus.ReferredBackToApplicant && _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.RequestedEditing, () => _previousStatus == ApplicationStatus.RequestedEditing && _userAccount.CanEditApplication);

        _machine.Configure(ApplicationStatus.CashflowUnderReview)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _userAccount.CanEditApplication);

        _machine.Configure(ApplicationStatus.ReferredBackToApplicant)
            .PermitReentryIf(AhpApplicationOperation.Modification, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.Submit, ApplicationStatus.ApplicationSubmitted, () => _userAccount.CanSubmitApplication);

        _machine.Configure(ApplicationStatus.RequestedEditing)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _userAccount.CanEditApplication)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _userAccount.CanEditApplication);
    }
}
