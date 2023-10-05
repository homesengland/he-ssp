using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

public class ProjectWorkflow : IStateRouting<ProjectState>
{
    private readonly StateMachine<ProjectState, Trigger> _machine;

    private readonly ProjectViewModel _model;

    public ProjectWorkflow()
    {
        _machine = new StateMachine<ProjectState, Trigger>(ProjectState.Index);

        ConfigureTransitions();
    }

    public ProjectWorkflow(ProjectState currentState, ProjectViewModel model)
    {
        _machine = new StateMachine<ProjectState, Trigger>(currentState);

        _model = model;

        ConfigureTransitions();
    }

    public static ProjectWorkflow ForStartPage() => new();

    public Task<ProjectState> NextState(Trigger trigger)
    {
        _machine.Fire(trigger);

        return Task.FromResult(_machine.State);
    }

    public Task<bool> StateCanBeAccessed(ProjectState nextState)
    {
        return nextState switch
        {
            _ => Task.FromResult(true),
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1005:Single line comments should begin with single space",
        Justification = "Commented lines will help in next refactoring steps")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.LayoutRules",
        "SA1515:Single-line comment should be preceded by blank line",
        Justification = "Commented lines will help in next refactoring steps")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.LayoutRules",
        "SA1512:Single-line comments should not be followed by blank line",
        Justification = "Commented lines will help in next refactoring steps")]
    private void ConfigureTransitions()
    {
        _machine.Configure(ProjectState.Index)
            .Permit(Trigger.Continue, ProjectState.Name);

        _machine.Configure(ProjectState.Name)
            .Permit(Trigger.Continue, ProjectState.StartDate)
            .Permit(Trigger.Back, ProjectState.Index);

        _machine.Configure(ProjectState.StartDate)
           .Permit(Trigger.Continue, ProjectState.ManyHomes)
           .Permit(Trigger.Back, ProjectState.Name);

        _machine.Configure(ProjectState.ManyHomes)
            .Permit(Trigger.Continue, ProjectState.TypeHomes)
            .Permit(Trigger.Back, ProjectState.StartDate)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.TypeHomes)
            .Permit(Trigger.Continue, ProjectState.Type)
            .Permit(Trigger.Back, ProjectState.ManyHomes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.Type)
            .Permit(Trigger.Continue, ProjectState.PlanningRef)
            .Permit(Trigger.Back, ProjectState.TypeHomes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.PlanningRef)
            .PermitIf(Trigger.Continue, ProjectState.PlanningRefEnter, () => _model.PlanningReferenceNumberExists == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, ProjectState.Location, () => _model.PlanningReferenceNumberExists == CommonResponse.No)
            .Permit(Trigger.Back, ProjectState.Type);

        _machine.Configure(ProjectState.PlanningRefEnter)
            .Permit(Trigger.Continue, ProjectState.PlanningPermissionStatus)
            .Permit(Trigger.Back, ProjectState.PlanningRef);

        _machine.Configure(ProjectState.PlanningPermissionStatus)
            .Permit(Trigger.Continue, ProjectState.Location)
            .Permit(Trigger.Back, ProjectState.PlanningRefEnter);
        //    .PermitIf(Trigger.Change, State.Location, () => _site.Location == null)
        //    .PermitIf(Trigger.Change, State.CheckAnswers, () => _site.Location != null);

        _machine.Configure(ProjectState.Location)
            .Permit(Trigger.Continue, ProjectState.Ownership)
            .PermitIf(Trigger.Back, ProjectState.PlanningPermissionStatus, () => _model.PlanningReferenceNumberExists == CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.PlanningRef, () => _model.PlanningReferenceNumberExists != CommonResponse.Yes);
        //.Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.Ownership)
            .PermitIf(Trigger.Continue, ProjectState.Additional, () => _model.Ownership == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, ProjectState.GrantFunding, () => _model.Ownership != CommonResponse.Yes)
            .Permit(Trigger.Back, ProjectState.Location);

        _machine.Configure(ProjectState.Additional)
            .Permit(Trigger.Continue, ProjectState.GrantFunding)
            .Permit(Trigger.Back, ProjectState.Ownership);

        _machine.Configure(ProjectState.GrantFunding)
           .PermitIf(Trigger.Continue, ProjectState.GrantFundingMore, () => _model.GrantFundingStatus == CommonResponse.Yes)
           .PermitIf(Trigger.Continue, ProjectState.ChargesDebt, () => _model.GrantFundingStatus != CommonResponse.Yes)
           .PermitIf(Trigger.Back, ProjectState.Additional, () => _model.Ownership == CommonResponse.Yes)
           .PermitIf(Trigger.Back, ProjectState.Ownership, () => _model.Ownership != CommonResponse.Yes);

        _machine.Configure(ProjectState.GrantFundingMore)
            .Permit(Trigger.Continue, ProjectState.ChargesDebt)
            .Permit(Trigger.Back, ProjectState.GrantFunding);

        //_machine.Configure(State.ChargesDebt)
        //    .Permit(Trigger.Continue, State.AffordableHomes)
        //    .PermitIf(Trigger.Back, State.GrantFundingMore, () => _site.GrantFunding == CommonResponse.Yes)
        //    .PermitIf(Trigger.Back, State.GrantFunding, () => _site.GrantFunding != CommonResponse.Yes)
        //    .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(ProjectState.ChargesDebt)
            .Permit(Trigger.Continue, ProjectState.AffordableHomes)
            .PermitIf(Trigger.Back, ProjectState.GrantFundingMore) // () => _site.GrantFunding == CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.GrantFunding) //, () => _site.GrantFunding != CommonResponse.Yes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.AffordableHomes)
           .Permit(Trigger.Continue, ProjectState.CheckAnswers)
           .Permit(Trigger.Back, ProjectState.ChargesDebt)
           .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.CheckAnswers)
            .Permit(Trigger.Continue, ProjectState.Complete)
            .PermitIf(Trigger.Back, ProjectState.AffordableHomes, () => _model.IsEditable())
            .PermitIf(Trigger.Back, ProjectState.Complete, () => _model.IsReadOnly());

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
