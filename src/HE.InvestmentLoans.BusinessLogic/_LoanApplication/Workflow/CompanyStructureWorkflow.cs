using HE.InvestmentLoans.BusinessLogic.Routing;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow
{
    public class CompanyStructureWorkflow
    {

        public enum State : int
        {
            Index = 1,
            Purpose,
            ExistingCompany,
            HomesBuilt,
            CheckAnswers,
            Complete
        }

        private LoanApplicationViewModel _model;
        private StateMachine<State, Trigger> _machine;
        private IMediator _mediator;

        public CompanyStructureWorkflow(LoanApplicationViewModel model, IMediator mediator)
        {
            _model = model;
            _machine = new StateMachine<State, Trigger>(_model.Company.State);
            _mediator = mediator;

            ConfigureTransitions();
        }


        public async void NextState(Trigger trigger)
        {
            await _machine.FireAsync(trigger);

        }

        public bool IsCompleted()
        {
            return _model.Company.State == State.Complete;
        }

        public bool IsStarted()
        {
            return _model.Company.Purpose != null
                || _model.Company.ExistingCompany != null
                || _model.Company.HomesBuilt != null;
        }

        public string GetName()
        {
            return System.Enum.GetName(typeof(State), _model.Company.State);
        }

        public async void ChangeState(State state)
        {
            _model.Company.State = state;
            _model.Company.StateChanged = true;
            await _mediator.Send(new Commands.Update() { Model = _model });
        }

        private void ConfigureTransitions()
        {
            _machine.Configure(State.Index)
                .Permit(Trigger.Continue, State.Purpose);


            _machine.Configure(State.Purpose)
              .Permit(Trigger.Continue, State.ExistingCompany)
              .Permit(Trigger.Change, State.CheckAnswers)
              .Permit(Trigger.Back, State.Index);

            _machine.Configure(State.ExistingCompany)
                .Permit(Trigger.Continue, State.HomesBuilt)
                .Permit(Trigger.Back, State.Purpose)
                .Permit(Trigger.Change, State.CheckAnswers);

            _machine.Configure(State.HomesBuilt)
                .Permit(Trigger.Continue, State.CheckAnswers)
                .Permit(Trigger.Back, State.ExistingCompany)
                .Permit(Trigger.Change, State.CheckAnswers);

            _machine.Configure(State.CheckAnswers)
               .PermitIf(Trigger.Continue, State.Complete, () => _model.Company.CheckAnswers == "Yes")
               .IgnoreIf(Trigger.Continue, () => _model.Company.CheckAnswers != "Yes")
               .PermitIf(Trigger.Back, State.HomesBuilt, () => _model.Company.Purpose != "Yes");

            _machine.Configure(State.Complete)
             .Permit(Trigger.Back, State.CheckAnswers);

            _machine.OnTransitionCompletedAsync(x =>
            {
                _model.Company.State = x.Destination;
                return _mediator.Send(new BusinessLogic._LoanApplication.Commands.Update() { Model = _model });

            });
        }
    }
}
