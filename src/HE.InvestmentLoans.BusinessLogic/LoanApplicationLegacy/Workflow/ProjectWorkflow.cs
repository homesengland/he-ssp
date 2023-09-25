using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.Projects;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
public class ProjectWorkflow : IStateRouting<ProjectState>
{
    private readonly StateMachine<ProjectState, Trigger> _machine;

    public ProjectWorkflow(ProjectState currentState)
    {
        _machine = new StateMachine<ProjectState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public Task<ProjectState> NextState(Trigger trigger)
    {
        _machine.Fire(trigger);

        return Task.FromResult(_machine.State);
    }

    public async Task<bool> StateCanBeAccessed(ProjectState nextState)
    {
        return nextState switch
        {
            _ => true,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ProjectState.Index)
            .Permit(Trigger.Continue, ProjectState.Name);

        _machine.Configure(ProjectState.Name)
            .Permit(Trigger.Continue, ProjectState.StartDate)
            .Permit(Trigger.Back, ProjectState.Index)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.StartDate)
           .Permit(Trigger.Continue, ProjectState.ManyHomes)
           .Permit(Trigger.Back, ProjectState.Name)
           .Permit(Trigger.Change, ProjectState.CheckAnswers);

        //_machine.Configure(State.ManyHomes)
        //    .Permit(Trigger.Continue, State.TypeHomes)
        //    .Permit(Trigger.Back, State.StartDate)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.TypeHomes)
        //    .Permit(Trigger.Continue, State.Type)
        //    .Permit(Trigger.Back, State.ManyHomes)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.Type)
        //    .Permit(Trigger.Continue, State.PlanningRef)
        //    .Permit(Trigger.Back, State.TypeHomes)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.PlanningRef)
        //    .PermitIf(Trigger.Continue, State.PlanningRefEnter, () => _site.PlanningRef == CommonResponse.Yes)
        //    .PermitIf(Trigger.Continue, State.Location, () => _site.PlanningRef == CommonResponse.No)
        //    .PermitIf(Trigger.Continue, State.Ownership, () => string.IsNullOrEmpty(_site.PlanningRef))
        //    .PermitIf(Trigger.Change, State.PlanningRefEnter, () => _site.PlanningRef == CommonResponse.Yes)
        //    .PermitIf(Trigger.Change, State.Location, () => _site.PlanningRef != CommonResponse.Yes)
        //    .Permit(Trigger.Back, State.Type);

        //_machine.Configure(State.PlanningRefEnter)
        //    .Permit(Trigger.Continue, State.PlanningPermissionStatus)
        //    .Permit(Trigger.Back, State.PlanningRef)
        //    .PermitIf(Trigger.Change, State.PlanningPermissionStatus, () => _site.PlanningStatus == null)
        //    .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.PlanningStatus != null);

        //_machine.Configure(State.PlanningPermissionStatus)
        //    .Permit(Trigger.Continue, State.Location)
        //    .Permit(Trigger.Back, State.PlanningRefEnter)
        //    .PermitIf(Trigger.Change, State.Location, () => _site.Location == null)
        //    .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.Location != null);

        //_machine.Configure(State.Location)
        //    .Permit(Trigger.Continue, State.Ownership)
        //    .PermitIf(Trigger.Back, State.PlanningPermissionStatus, () => _site.PlanningRef == CommonResponse.Yes)
        //    .PermitIf(Trigger.Back, State.PlanningRef, () => _site.PlanningRef != CommonResponse.Yes)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.Ownership)
        //    .PermitIf(Trigger.Continue, State.Additional, () => _site.Ownership == CommonResponse.Yes)
        //    .PermitIf(Trigger.Continue, State.GrantFunding, () => _site.Ownership != CommonResponse.Yes)
        //    .PermitIf(Trigger.Back, State.Location, () => _site.PlanningRef == CommonResponse.Yes)
        //    .PermitIf(Trigger.Back, State.Location, () => _site.PlanningRef == CommonResponse.No)
        //    .PermitIf(Trigger.Back, State.PlanningRef, () => string.IsNullOrEmpty(_site.PlanningRef))
        //    .PermitIf(Trigger.Change, State.Additional, () => _site.Ownership == CommonResponse.Yes)
        //    .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.Ownership != CommonResponse.Yes);

        //_machine.Configure(State.Additional)
        //    .Permit(Trigger.Continue, State.GrantFunding)
        //    .Permit(Trigger.Back, State.Ownership)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.GrantFunding)
        //   .PermitIf(Trigger.Continue, State.GrantFundingMore, () => _site.GrantFunding == CommonResponse.Yes)
        //   .PermitIf(Trigger.Continue, State.ChargesDebt, () => _site.GrantFunding != CommonResponse.Yes)
        //   .PermitIf(Trigger.Back, State.Additional, () => _site.Ownership == CommonResponse.Yes)
        //   .PermitIf(Trigger.Back, State.Ownership, () => _site.Ownership != CommonResponse.Yes)
        //   .PermitIf(Trigger.Change, State.GrantFundingMore, () => _site.GrantFunding == CommonResponse.Yes)
        //   .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.GrantFunding != CommonResponse.Yes);

        //_machine.Configure(State.GrantFundingMore)
        //    .Permit(Trigger.Continue, State.ChargesDebt)
        //    .Permit(Trigger.Back, State.GrantFunding)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.ChargesDebt)
        //    .Permit(Trigger.Continue, State.AffordableHomes)
        //    .PermitIf(Trigger.Back, State.GrantFundingMore, () => _site.GrantFunding == CommonResponse.Yes)
        //    .PermitIf(Trigger.Back, State.GrantFunding, () => _site.GrantFunding != CommonResponse.Yes)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.AffordableHomes)
        //   .Permit(Trigger.Continue, State.CheckAnswers)
        //   .Permit(Trigger.Back, State.ChargesDebt)
        //   .Permit(Trigger.Change, State.CheckAnswers);

        //_machine.Configure(State.CheckAnswers)
        //    .PermitIf(Trigger.Continue, State.Complete, () => _site.CheckAnswers == CommonResponse.Yes)
        //    .IgnoreIf(Trigger.Continue, () => _site.CheckAnswers != CommonResponse.Yes)
        //    .Permit(Trigger.Back, State.AffordableHomes)
        //    .OnExit(() =>
        //    {
        //        if (_site.CheckAnswers == CommonResponse.Yes)
        //        {
        //            _site.SetFlowCompletion(true);
        //        }
        //    });

        //_machine.Configure(State.Complete)
        //    .Permit(Trigger.Back, State.CheckAnswers);

        //_machine.Configure(State.DeleteProject)
        //    .Permit(Trigger.Back, State.TaskList);

        //_machine.OnTransitionCompletedAsync(x =>
        //{
        //    _site.State = x.Destination;

        //    _site.RemoveAlternativeRoutesData();

        //    return _mediator.Send(new Commands.Update() { Model = _model });
        //});
    }
}
