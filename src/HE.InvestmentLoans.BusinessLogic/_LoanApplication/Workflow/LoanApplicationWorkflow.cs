using HE.InvestmentLoans.BusinessLogic.Enums;
using HE.InvestmentLoans.BusinessLogic.Routing;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Stateless;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow
{
    public class LoanApplicationWorkflow
    {
        public enum State : int
        {
            Index = 1,
            AboutLoan,
            CheckYourDetails,
            LoanPurpose,
            TaskList,
            CheckAnswers,
            ApplicationSubmitted,
            Ineligible
        }

        private StateMachine<State, Trigger> _machine;
        private IMediator _mediator;
        private LoanApplicationViewModel _model;

        public LoanApplicationWorkflow(LoanApplicationViewModel model, MediatR.IMediator mediator)
        {
            _model = model;
            _machine = new StateMachine<State, Trigger>(_model.State);
            _mediator = mediator;
            ConfigureTransitions();
        }

        private void ConfigureTransitions()
        {
            _machine.Configure(State.Index)
              .Permit(Trigger.Continue, State.AboutLoan);

            _machine.Configure(State.AboutLoan)
                .Permit(Trigger.Continue, State.CheckYourDetails)
                .Permit(Trigger.Back, State.Index);

            _machine.Configure(State.CheckYourDetails)
                .Permit(Trigger.Continue, State.LoanPurpose)
                .Permit(Trigger.Back, State.AboutLoan);

            _machine.Configure(State.LoanPurpose)
                .PermitIf(Trigger.Continue, State.TaskList, () => _model.Purpose == FundingPurpose.BuildingNewHomes)
                .PermitIf(Trigger.Continue, State.Ineligible, () => _model.Purpose != FundingPurpose.BuildingNewHomes)
                .Permit(Trigger.Back, State.CheckYourDetails);

            _machine.Configure(State.Ineligible)
                .Permit(Trigger.Back, State.LoanPurpose);

            _machine.Configure(State.TaskList)
                .Permit(Trigger.Continue, State.CheckAnswers)
                .Permit(Trigger.Back, State.LoanPurpose);

            _machine.Configure(State.CheckAnswers)
                .Permit(Trigger.Continue, State.ApplicationSubmitted)
                .Permit(Trigger.Back, State.TaskList);

            _machine.Configure(State.ApplicationSubmitted).OnEntry(x =>
            {
                _mediator.Send(new BusinessLogic._LoanApplication.Commands.SendToCrm() { Model = _model }).GetAwaiter().GetResult();
 
            });

            _machine.OnTransitionCompletedAsync(x =>
            {
                _model.State = x.Destination;
                return _mediator.Send(new BusinessLogic._LoanApplication.Commands.Update() { Model = _model });

            });
        }


        public async void NextState(Trigger trigger)
        {
            await _machine.FireAsync(trigger);
            _model.State = _machine.State;
        }

        public string GetName()
        {
            return System.Enum.GetName(typeof(State), _model.State);
        }

        public bool IsFilled()
        {
            return _model.Company.State == CompanyStructureWorkflow.State.Complete
                && _model.Security.State == SecurityWorkflow.State.Complete
                && _model.Funding.State == FundingWorkflow.State.Complete
                && _model.Sites.All(x => x.State == SiteWorkflow.State.Complete);

        }

        public bool IsBeingChecked()
        {
            return _model.State == State.CheckAnswers;
        }
    }
}
