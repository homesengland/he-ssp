using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class SiteWorkflow
{
    public enum State : int
    {
        Index,
        Name,
        StartDate,
        ManyHomes,
        TypeHomes,
        PlanningRef,
        PlanningRefEnter,
        CheckPlanning,
        PlanningPermissionStatus,
        Location,
        Ownership,
        Additional,
        GrantFunding,
        GrantFundingMore,
        AffordableHomes,
        CheckAnswers,
        Complete,
        Type,
        ChargesDebt,
        DeleteProject,
        TaskList,
    }

    private readonly LoanApplicationViewModel _model;
    private readonly SiteViewModel _site;
    private readonly StateMachine<State, Trigger> _machine;
    private readonly IMediator _mediator;

    public SiteWorkflow(LoanApplicationViewModel model, IMediator mediator, Guid id)
    {
        _model = model;
        _site = _model.Sites.First(x => x.Id == id);
        _machine = new StateMachine<State, Trigger>(_site.State);
        _mediator = mediator;

        ConfigureTransitions();
    }

    public async void NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
    }

    public async void ChangeState(State state, bool isChangeView)
    {
        _site.State = state;
        _site.StateChanged = isChangeView;
        await _mediator.Send(new Commands.Update() { Model = _model });
    }

    public bool IsStateComplete()
    {
        return _site.State == State.Complete;
    }

    public bool IsWorkflowCompleted()
    {
        return IsStateComplete() || _site.IsFlowCompleted;
    }

    public bool IsStarted()
    {
        return _site.AnyBasicInformationIsProvided();
    }

    public string GetName()
    {
        return Enum.GetName(typeof(State), _site.State) ?? string.Empty;
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(State.Index)
            .Permit(Trigger.Continue, State.Name);

        _machine.Configure(State.Name)
            .Permit(Trigger.Continue, State.StartDate)
            .Permit(Trigger.Back, State.Index)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.StartDate)
           .Permit(Trigger.Continue, State.ManyHomes)
           .Permit(Trigger.Back, State.Name)
           .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.ManyHomes)
            .Permit(Trigger.Continue, State.TypeHomes)
            .Permit(Trigger.Back, State.StartDate)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.TypeHomes)
            .Permit(Trigger.Continue, State.Type)
            .Permit(Trigger.Back, State.ManyHomes)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.Type)
            .Permit(Trigger.Continue, State.PlanningRef)
            .Permit(Trigger.Back, State.TypeHomes)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.PlanningRef)
            .PermitIf(Trigger.Continue, State.PlanningRefEnter, () => _site.PlanningRef == "Yes")
            .PermitIf(Trigger.Continue, State.Location, () => _site.PlanningRef == "No")
            .PermitIf(Trigger.Continue, State.Ownership, () => string.IsNullOrEmpty(_site.PlanningRef))
            .PermitIf(Trigger.Change, State.PlanningRefEnter, () => _site.PlanningRef == "Yes")
            .PermitIf(Trigger.Change, State.Location, () => _site.PlanningRef != "Yes")
            .Permit(Trigger.Back, State.Type);

        _machine.Configure(State.PlanningRefEnter)
            .Permit(Trigger.Continue, State.PlanningPermissionStatus)
            .Permit(Trigger.Back, State.PlanningRef)
            .PermitIf(Trigger.Change, State.PlanningPermissionStatus, () => _site.PlanningStatus == null)
            .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.PlanningStatus != null);

        _machine.Configure(State.PlanningPermissionStatus)
            .Permit(Trigger.Continue, State.Location)
            .Permit(Trigger.Back, State.PlanningRefEnter)
            .PermitIf(Trigger.Change, State.Location, () => _site.Location == null)
            .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.Location != null);

        _machine.Configure(State.Location)
            .Permit(Trigger.Continue, State.Ownership)
            .PermitIf(Trigger.Back, State.PlanningPermissionStatus, () => _site.PlanningRef == "Yes")
            .PermitIf(Trigger.Back, State.PlanningRef, () => _site.PlanningRef != "Yes")
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.Ownership)
            .PermitIf(Trigger.Continue, State.Additional, () => _site.Ownership == "Yes")
            .PermitIf(Trigger.Continue, State.GrantFunding, () => _site.Ownership != "Yes")
            .PermitIf(Trigger.Back, State.Location, () => _site.PlanningRef == "Yes")
            .PermitIf(Trigger.Back, State.Location, () => _site.PlanningRef == "No")
            .PermitIf(Trigger.Back, State.PlanningRef, () => string.IsNullOrEmpty(_site.PlanningRef))
            .PermitIf(Trigger.Change, State.Additional, () => _site.Ownership == "Yes")
            .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.Ownership != "Yes");

        _machine.Configure(State.Additional)
            .Permit(Trigger.Continue, State.GrantFunding)
            .Permit(Trigger.Back, State.Ownership)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.GrantFunding)
           .PermitIf(Trigger.Continue, State.GrantFundingMore, () => _site.GrantFunding == "Yes")
           .PermitIf(Trigger.Continue, State.ChargesDebt, () => _site.GrantFunding != "Yes")
           .PermitIf(Trigger.Back, State.Additional, () => _site.Ownership == "Yes")
           .PermitIf(Trigger.Back, State.Ownership, () => _site.Ownership != "Yes")
           .PermitIf(Trigger.Change, State.GrantFundingMore, () => _site.GrantFunding == "Yes")
           .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.GrantFunding != "Yes");

        _machine.Configure(State.GrantFundingMore)
            .Permit(Trigger.Continue, State.ChargesDebt)
            .Permit(Trigger.Back, State.GrantFunding)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.ChargesDebt)
            .Permit(Trigger.Continue, State.AffordableHomes)
            .PermitIf(Trigger.Back, State.GrantFundingMore, () => _site.GrantFunding == "Yes")
            .PermitIf(Trigger.Back, State.GrantFunding, () => _site.GrantFunding != "Yes")
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.AffordableHomes)
           .Permit(Trigger.Continue, State.CheckAnswers)
           .Permit(Trigger.Back, State.ChargesDebt)
           .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.CheckAnswers)
            .PermitIf(Trigger.Continue, State.Complete, () => _site.CheckAnswers == "Yes")
            .IgnoreIf(Trigger.Continue, () => _site.CheckAnswers != "Yes")
            .Permit(Trigger.Back, State.AffordableHomes)
            .OnExit(() =>
            {
                if (_site.CheckAnswers == "Yes")
                {
                    _site.SetFlowCompletion(true);
                }
            });

        _machine.Configure(State.Complete)
            .Permit(Trigger.Back, State.CheckAnswers);

        _machine.Configure(State.DeleteProject)
            .Permit(Trigger.Back, State.TaskList);

        _machine.OnTransitionCompletedAsync(x =>
        {
            _site.State = x.Destination;

            _site.RemoveAlternativeRoutesData();

            return _mediator.Send(new Commands.Update() { Model = _model });
        });
    }
}
