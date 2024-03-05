using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.CompanyStructure;
using Stateless;

namespace HE.Investments.Loans.WWW.Workflows;

public class CompanyStructureWorkflow : WorkflowBase<CompanyStructureState, CompanyStructureViewModel>
{
    public CompanyStructureWorkflow(CompanyStructureState state, CompanyStructureViewModel model)
    {
        Model = model;
        Machine = new StateMachine<CompanyStructureState, Trigger>(state);
        ConfigureTransitions();
    }

    public override CompanyStructureState CurrentState(CompanyStructureState targetState)
    {
        if (Model.IsReadOnly())
        {
            return CompanyStructureState.CheckAnswers;
        }

        if (targetState != CompanyStructureState.StartCompanyStructure || !IsSectionStarted())
        {
            return targetState;
        }

        return Model switch
        {
            { Purpose: var x } when x.IsNotProvided() => CompanyStructureState.Purpose,
            { OrganisationMoreInformation: var x } when x.IsNotProvided() => CompanyStructureState.ExistingCompany,
            { HomesBuilt: var x } when x.IsNotProvided() => CompanyStructureState.HomesBuilt,
            _ => CompanyStructureState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(CompanyStructureState.StartCompanyStructure)
            .Permit(Trigger.Continue, CompanyStructureState.Purpose);

        Machine.Configure(CompanyStructureState.Purpose)
            .Permit(Trigger.Continue, CompanyStructureState.ExistingCompany)
            .Permit(Trigger.Change, CompanyStructureState.CheckAnswers)
            .PermitIf(Trigger.Back, CompanyStructureState.TaskList, IsSectionStarted)
            .PermitIf(Trigger.Back, CompanyStructureState.StartCompanyStructure, () => !IsSectionStarted());

        Machine.Configure(CompanyStructureState.ExistingCompany)
            .Permit(Trigger.Continue, CompanyStructureState.HomesBuilt)
            .Permit(Trigger.Back, CompanyStructureState.Purpose)
            .Permit(Trigger.Change, CompanyStructureState.CheckAnswers);

        Machine.Configure(CompanyStructureState.HomesBuilt)
            .Permit(Trigger.Continue, CompanyStructureState.CheckAnswers)
            .Permit(Trigger.Back, CompanyStructureState.ExistingCompany)
            .Permit(Trigger.Change, CompanyStructureState.CheckAnswers);

        Machine.Configure(CompanyStructureState.CheckAnswers)
            .PermitIf(Trigger.Continue, CompanyStructureState.Complete)
            .PermitIf(Trigger.Back, CompanyStructureState.Complete, () => Model.IsReadOnly())
            .PermitIf(Trigger.Back, CompanyStructureState.HomesBuilt, () => Model.IsEditable());

        Machine.Configure(CompanyStructureState.Complete)
           .Permit(Trigger.Back, CompanyStructureState.CheckAnswers);
    }
}
