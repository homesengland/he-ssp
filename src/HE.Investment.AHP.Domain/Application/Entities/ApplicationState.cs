using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using Stateless;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationState
{
    private readonly ApplicationStatus? _previousStatus;

    private readonly bool _wasSubmitted;

    private readonly bool _canEdit;

    private readonly bool _canSubmit;

    private readonly StateMachine<ApplicationStatus, AhpApplicationOperation> _machine;

    public ApplicationState(ApplicationStatus currentStatus, ApplicationStatus? previousStatus, bool canEdit, bool canSubmit, bool wasSubmitted)
    {
        _canEdit = canEdit;
        _canSubmit = canSubmit;
        _previousStatus = previousStatus;
        _wasSubmitted = wasSubmitted;
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
            .PermitReentryIf(AhpApplicationOperation.Modification, () => _canEdit)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Deleted, () => !_wasSubmitted && _canEdit)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _wasSubmitted && _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit)
            .PermitIf(AhpApplicationOperation.Submit, ApplicationStatus.ApplicationSubmitted, () => _canSubmit);

        _machine.Configure(ApplicationStatus.ApplicationSubmitted)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _canEdit);

        _machine.Configure(ApplicationStatus.UnderReview)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _canEdit);

        _machine.Configure(ApplicationStatus.ApplicationUnderReview)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _canEdit);

        _machine.Configure(ApplicationStatus.OnHold)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.Draft, () => _previousStatus == ApplicationStatus.Draft && _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.ApplicationSubmitted, () => _previousStatus == ApplicationStatus.ApplicationSubmitted && _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.UnderReview, () => _previousStatus == ApplicationStatus.UnderReview && _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.ApplicationUnderReview, () => _previousStatus == ApplicationStatus.ApplicationUnderReview && _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.CashflowUnderReview, () => _previousStatus == ApplicationStatus.CashflowUnderReview && _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.ReferredBackToApplicant, () => _previousStatus == ApplicationStatus.ReferredBackToApplicant && _canEdit)
            .PermitIf(AhpApplicationOperation.Reactivate, ApplicationStatus.RequestedEditing, () => _previousStatus == ApplicationStatus.RequestedEditing && _canEdit);

        _machine.Configure(ApplicationStatus.CashflowUnderReview)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit)
            .PermitIf(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing, () => _canEdit);

        _machine.Configure(ApplicationStatus.ReferredBackToApplicant)
            .PermitReentryIf(AhpApplicationOperation.Modification, () => _canEdit)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit)
            .PermitIf(AhpApplicationOperation.Submit, ApplicationStatus.UnderReview, () => _canSubmit);

        _machine.Configure(ApplicationStatus.RequestedEditing)
            .PermitIf(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn, () => _canEdit)
            .PermitIf(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold, () => _canEdit);
    }
}
