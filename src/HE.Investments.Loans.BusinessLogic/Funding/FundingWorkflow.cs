using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.Funding;
using HE.Investments.Loans.Contract.Funding.Enums;
using Stateless;

namespace HE.Investments.Loans.BusinessLogic.Funding;

public class FundingWorkflow : IStateRouting<FundingState>
{
    private readonly FundingViewModel _model;
    private readonly StateMachine<FundingState, Trigger> _machine;

    public FundingWorkflow(FundingState currentState, FundingViewModel fundingViewModel)
    {
        _model = fundingViewModel;
        _machine = new StateMachine<FundingState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public async Task<FundingState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);

        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(FundingState nextState)
    {
        return Task.FromResult(true);
    }

    public FundingState CurrentState(FundingState targetState)
    {
        if (_model.IsReadOnly())
        {
            return FundingState.CheckAnswers;
        }

        if (targetState != FundingState.Index || _model.State == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _model switch
        {
            { GrossDevelopmentValue: var x } when x.IsNotProvided() => FundingState.GDV,
            { TotalCosts: var x } when x.IsNotProvided() => FundingState.TotalCosts,
            { AbnormalCosts: var x } when x.IsNotProvided() => FundingState.AbnormalCosts,
            { PrivateSectorFunding: var x } when x.IsNotProvided() => FundingState.PrivateSectorFunding,
            { Refinance: var x } when x.IsNotProvided() => FundingState.Refinance,
            { AdditionalProjects: var x } when x.IsNotProvided() => FundingState.AdditionalProjects,
            _ => FundingState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(FundingState.Index)
          .Permit(Trigger.Continue, FundingState.GDV);

        _machine.Configure(FundingState.GDV)
            .Permit(Trigger.Continue, FundingState.TotalCosts)
            .Permit(Trigger.Back, FundingState.Index)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.TotalCosts)
            .Permit(Trigger.Continue, FundingState.AbnormalCosts)
            .Permit(Trigger.Back, FundingState.GDV)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.AbnormalCosts)
            .Permit(Trigger.Continue, FundingState.PrivateSectorFunding)
            .Permit(Trigger.Back, FundingState.TotalCosts)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.PrivateSectorFunding)
            .Permit(Trigger.Continue, FundingState.Refinance)
            .Permit(Trigger.Back, FundingState.AbnormalCosts)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.Refinance)
            .Permit(Trigger.Continue, FundingState.AdditionalProjects)
            .Permit(Trigger.Back, FundingState.PrivateSectorFunding)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.AdditionalProjects)
           .Permit(Trigger.Continue, FundingState.CheckAnswers)
            .Permit(Trigger.Back, FundingState.Refinance)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.CheckAnswers)
           .Permit(Trigger.Continue, FundingState.Complete)
           .PermitIf(Trigger.Back, FundingState.AdditionalProjects, () => _model.IsEditable())
           .PermitIf(Trigger.Back, FundingState.Complete, () => _model.IsReadOnly());

        _machine.Configure(FundingState.Complete)
            .Permit(Trigger.Back, FundingState.CheckAnswers);
    }
}
