using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.Projects;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using Stateless;

namespace HE.Investments.Loans.WWW.Workflows;

public class ProjectWorkflow : WorkflowBase<ProjectState, ProjectViewModel>
{
    public ProjectWorkflow()
    {
        Machine = new StateMachine<ProjectState, Trigger>(ProjectState.Index);
        Model = new ProjectViewModel();

        ConfigureTransitions();
    }

    public ProjectWorkflow(ProjectState currentState, ProjectViewModel model)
    {
        Machine = new StateMachine<ProjectState, Trigger>(currentState);
        Model = model;

        ConfigureTransitions();
    }

    public static ProjectWorkflow ForStartPage() => new();

    public override ProjectState CurrentState(ProjectState targetState)
    {
        if (Model.IsReadOnly())
        {
            return ProjectState.CheckAnswers;
        }

        if (targetState != ProjectState.Index || !IsSectionStarted() || Model.Status == SectionStatus.Undefined)
        {
            return targetState;
        }

        return Model switch
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
        Machine.Configure(ProjectState.Index)
            .Permit(Trigger.Continue, ProjectState.Name)
            .Permit(Trigger.Back, ProjectState.TaskList);

        Machine.Configure(ProjectState.Name)
            .Permit(Trigger.Continue, ProjectState.StartDate)
            .PermitIf(Trigger.Back, ProjectState.TaskList, IsSectionStarted)
            .PermitIf(Trigger.Back, ProjectState.Index, () => !IsSectionStarted());

        Machine.Configure(ProjectState.StartDate)
           .Permit(Trigger.Continue, ProjectState.ManyHomes)
           .Permit(Trigger.Back, ProjectState.Name);

        Machine.Configure(ProjectState.ManyHomes)
            .Permit(Trigger.Continue, ProjectState.TypeHomes)
            .Permit(Trigger.Back, ProjectState.StartDate)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        Machine.Configure(ProjectState.TypeHomes)
            .Permit(Trigger.Continue, ProjectState.Type)
            .Permit(Trigger.Back, ProjectState.ManyHomes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        Machine.Configure(ProjectState.Type)
            .Permit(Trigger.Continue, ProjectState.PlanningRef)
            .Permit(Trigger.Back, ProjectState.TypeHomes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        Machine.Configure(ProjectState.PlanningRef)
            .PermitIf(Trigger.Continue, ProjectState.PlanningRefEnter, () => Model.PlanningReferenceNumberExists != CommonResponse.No)
            .PermitIf(Trigger.Continue, ProjectState.Location, () => Model.PlanningReferenceNumberExists == CommonResponse.No)
            .Permit(Trigger.Back, ProjectState.Type);

        Machine.Configure(ProjectState.PlanningRefEnter)
            .Permit(Trigger.Continue, ProjectState.PlanningPermissionStatus)
            .Permit(Trigger.Back, ProjectState.PlanningRef);

        Machine.Configure(ProjectState.PlanningPermissionStatus)
            .Permit(Trigger.Continue, ProjectState.Location)
            .Permit(Trigger.Back, ProjectState.PlanningRefEnter);

        Machine.Configure(ProjectState.Location)
            .Permit(Trigger.Continue, ProjectState.ProvideLocalAuthority)
            .PermitIf(Trigger.Back, ProjectState.PlanningPermissionStatus, () => Model.PlanningReferenceNumberExists == CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.PlanningRef, () => Model.PlanningReferenceNumberExists != CommonResponse.Yes);

        Machine.Configure(ProjectState.ProvideLocalAuthority)
            .Permit(Trigger.Continue, ProjectState.LocalAuthorityResult)
            .Permit(Trigger.Back, ProjectState.Location);

        Machine.Configure(ProjectState.LocalAuthorityResult)
            .Permit(Trigger.Continue, ProjectState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, ProjectState.ProvideLocalAuthority);

        Machine.Configure(ProjectState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, ProjectState.Ownership)
            .Permit(Trigger.Back, ProjectState.ProvideLocalAuthority);

        Machine.Configure(ProjectState.LocalAuthorityReset)
            .Permit(Trigger.Continue, ProjectState.Ownership)
            .Permit(Trigger.Back, ProjectState.ProvideLocalAuthority);

        Machine.Configure(ProjectState.Ownership)
            .PermitIf(Trigger.Continue, ProjectState.Additional, () => Model.Ownership == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, ProjectState.GrantFunding, () => Model.Ownership != CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.LocalAuthorityConfirm, () => Model.LocalAuthorityName.IsProvided())
            .PermitIf(Trigger.Back, ProjectState.ProvideLocalAuthority, () => Model.LocalAuthorityName.IsNotProvided());

        Machine.Configure(ProjectState.Additional)
            .Permit(Trigger.Continue, ProjectState.GrantFunding)
            .Permit(Trigger.Back, ProjectState.Ownership);

        Machine.Configure(ProjectState.GrantFunding)
           .PermitIf(Trigger.Continue, ProjectState.GrantFundingMore, () => Model.GrantFundingStatus == CommonResponse.Yes)
           .PermitIf(Trigger.Continue, ProjectState.ChargesDebt, () => Model.GrantFundingStatus != CommonResponse.Yes)
           .PermitIf(Trigger.Back, ProjectState.Additional, () => Model.Ownership == CommonResponse.Yes)
           .PermitIf(Trigger.Back, ProjectState.Ownership, () => Model.Ownership != CommonResponse.Yes);

        Machine.Configure(ProjectState.GrantFundingMore)
            .Permit(Trigger.Continue, ProjectState.ChargesDebt)
            .Permit(Trigger.Back, ProjectState.GrantFunding);

        Machine.Configure(ProjectState.ChargesDebt)
            .Permit(Trigger.Continue, ProjectState.AffordableHomes)
            .PermitIf(Trigger.Back, ProjectState.GrantFundingMore, () => Model.GrantFundingStatus == CommonResponse.Yes)
            .PermitIf(Trigger.Back, ProjectState.GrantFunding, () => Model.GrantFundingStatus != CommonResponse.Yes)
            .Permit(Trigger.Change, ProjectState.CheckAnswers);

        Machine.Configure(ProjectState.AffordableHomes)
           .Permit(Trigger.Continue, ProjectState.CheckAnswers)
           .Permit(Trigger.Back, ProjectState.ChargesDebt)
           .Permit(Trigger.Change, ProjectState.CheckAnswers);

        Machine.Configure(ProjectState.CheckAnswers)
            .Permit(Trigger.Continue, ProjectState.Complete)
            .PermitIf(Trigger.Back, ProjectState.AffordableHomes, () => Model.IsEditable())
            .PermitIf(Trigger.Back, ProjectState.Complete, () => Model.IsReadOnly());
    }
}
