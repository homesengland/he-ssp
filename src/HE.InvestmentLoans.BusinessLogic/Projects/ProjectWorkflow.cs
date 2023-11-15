using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.Investments.Common.Extensions;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.Projects;

public class ProjectWorkflow : IStateRouting<ProjectState>
{
    private readonly StateMachine<ProjectState, Trigger> _machine;

    private readonly ProjectViewModel _model;

    public ProjectWorkflow()
    {
        _machine = new StateMachine<ProjectState, Trigger>(ProjectState.Index);
        _model = new ProjectViewModel();

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

    public ProjectState CurrentState(ProjectState targetState)
    {
        if (_model.IsReadOnly())
        {
            return ProjectState.CheckAnswers;
        }

        if (targetState == ProjectState.Index && (_model.Status == SectionStatus.NotStarted || _model.Status == SectionStatus.Undefined))
        {
            return targetState;
        }

        return _model switch
        {
            { ProjectName: var x } when x.IsNotProvided() => ProjectState.Name,
            { HasEstimatedStartDate: var x } when x.IsNotProvided() => ProjectState.StartDate,
            { HomesCount: var x } when x.IsNotProvided() => ProjectState.ManyHomes,
            { HomeTypes: var x } when x.IsNotProvided() => ProjectState.TypeHomes,
            { ProjectType: var x } when x.IsNotProvided() => ProjectState.Type,
            { PlanningReferenceNumberExists: var x } when x.IsNotProvided() => ProjectState.PlanningRef,
            { PlanningReferenceNumberExists: CommonResponse.Yes, PlanningReferenceNumber: var x } when x.IsNotProvided() => ProjectState.PlanningRefEnter,
            { PlanningReferenceNumberExists: CommonResponse.No, LocationOption: var x } when x.IsNotProvided() => ProjectState.Location,
            { Ownership: var x } when x.IsNotProvided() => ProjectState.Ownership,
            { LocalAuthorityName: var x } when x.IsNotProvided() => ProjectState.ProvideLocalAuthority,
            { Ownership: CommonResponse.Yes, Source: var x } when x.IsNotProvided() => ProjectState.Additional,
            { Ownership: CommonResponse.No, GrantFundingStatus: var x } when x.IsNotProvided() => ProjectState.GrantFunding,
            { GrantFundingStatus: CommonResponse.Yes, GrantFundingName: var x } when x.IsNotProvided() => ProjectState.GrantFundingMore,
            { ChargesDebt: var x } when x.IsNotProvided() => ProjectState.ChargesDebt,
            { AffordableHomes: var x } when x.IsNotProvided() => ProjectState.AffordableHomes,
            _ => ProjectState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ProjectState.Index)
            .Permit(Trigger.Continue, ProjectState.Name)
            .Permit(Trigger.Back, ProjectState.TaskList);

        _machine.Configure(ProjectState.Name)
            .Permit(Trigger.Continue, ProjectState.StartDate)
            .Permit(Trigger.Back, ProjectState.TaskList);

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

        _machine.Configure(ProjectState.Location)
            .Permit(Trigger.Continue, ProjectState.ProvideLocalAuthority)
            .PermitIf(Trigger.Back, ProjectState.PlanningPermissionStatus, () => _model.PlanningReferenceNumberExists == CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.PlanningRef, () => _model.PlanningReferenceNumberExists != CommonResponse.Yes);

        _machine.Configure(ProjectState.ProvideLocalAuthority)
            .Permit(Trigger.Continue, ProjectState.LocalAuthorityResult)
            .Permit(Trigger.Back, ProjectState.Location);

        _machine.Configure(ProjectState.LocalAuthorityResult)
            .Permit(Trigger.Continue, ProjectState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, ProjectState.ProvideLocalAuthority);

        _machine.Configure(ProjectState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, ProjectState.Ownership)
            .Permit(Trigger.Back, ProjectState.ProvideLocalAuthority);

        _machine.Configure(ProjectState.LocalAuthorityReset)
            .Permit(Trigger.Continue, ProjectState.Ownership)
            .Permit(Trigger.Back, ProjectState.ProvideLocalAuthority);

        _machine.Configure(ProjectState.Ownership)
            .PermitIf(Trigger.Continue, ProjectState.Additional, () => _model.Ownership == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, ProjectState.GrantFunding, () => _model.Ownership != CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.LocalAuthorityConfirm, () => _model.LocalAuthorityName.IsProvided())
            .PermitIf(Trigger.Back, ProjectState.ProvideLocalAuthority, () => _model.LocalAuthorityName.IsNotProvided());

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

        _machine.Configure(ProjectState.ChargesDebt)
            .Permit(Trigger.Continue, ProjectState.AffordableHomes)
            .PermitIf(Trigger.Back, ProjectState.GrantFundingMore, () => _model.GrantFundingStatus == CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.GrantFunding, () => _model.GrantFundingStatus != CommonResponse.Yes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.AffordableHomes)
           .Permit(Trigger.Continue, ProjectState.CheckAnswers)
           .Permit(Trigger.Back, ProjectState.ChargesDebt)
           .Permit(Trigger.Change, ProjectState.CheckAnswers);

        _machine.Configure(ProjectState.CheckAnswers)
            .Permit(Trigger.Continue, ProjectState.Complete)
            .PermitIf(Trigger.Back, ProjectState.AffordableHomes, () => _model.IsEditable())
            .PermitIf(Trigger.Back, ProjectState.Complete, () => _model.IsReadOnly());
    }
}
