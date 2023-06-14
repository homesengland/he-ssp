using HE.InvestmentLoans.BusinessLogic.Routing;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow
{

    public class SecurityWorkflow 
    {
        public enum State : int
        {
            Index=1,
            ChargesDebtCompany=2,
            DirLoans=3,
            DirLoansSub=4,
            CheckAnswers=5,
            Complete=6
        }
        private LoanApplicationViewModel _model;
        private StateMachine<State, Trigger> _machine;
        private IMediator _mediator;

        public SecurityWorkflow(LoanApplicationViewModel model, IMediator mediator)
        {
            _model = model;
            _machine = new StateMachine<State, Trigger>(_model.Security.State);
            _mediator = mediator;

            ConfigureTransitions();
        }


        public async void NextState(Trigger trigger)
        {
            await _machine.FireAsync(trigger);

        }

        public bool IsCompleted()
        {
            return _model.Security.State == State.Complete;
        }

        public bool IsNotStarted()
        {
            return _model.Security.State == State.Index;
        }

        public string GetName()
        {
            return System.Enum.GetName(typeof(State), _model.Security.State);
        }

        public async void ChangeState(State state)
        {
            _model.Security.State = state;
            _model.Security.StateChanged = true;
            await _mediator.Send(new BusinessLogic._LoanApplication.Commands.Update() { Model = _model });
        }

        private void ConfigureTransitions()
        {

            _machine.Configure(State.Index)
              .Permit(Trigger.Continue, State.ChargesDebtCompany);


            _machine.Configure(State.ChargesDebtCompany)
                .Permit(Trigger.Continue, State.DirLoans)
                .Permit(Trigger.Back, State.Index)
                .Permit(Trigger.Change, State.CheckAnswers);

            _machine.Configure(State.DirLoans)
                .PermitIf(Trigger.Continue, State.DirLoansSub, () => _model.Security.DirLoans == "Yes")
                .PermitIf(Trigger.Continue, State.CheckAnswers, () => _model.Security.DirLoans != "Yes")
                .PermitIf(Trigger.Change, State.DirLoansSub, () => _model.Security.DirLoans == "Yes")
                .PermitIf(Trigger.Change, State.CheckAnswers, () => _model.Security.DirLoans != "Yes")
                .Permit(Trigger.Back, State.ChargesDebtCompany);

            _machine.Configure(State.DirLoansSub)
                .Permit(Trigger.Continue, State.CheckAnswers)
                .Permit(Trigger.Back, State.DirLoans)
                .Permit(Trigger.Change, State.CheckAnswers);

            _machine.Configure(State.CheckAnswers)
               .PermitIf(Trigger.Continue, State.Complete, () => _model.Security.CheckAnswers == "Yes")
               .IgnoreIf(Trigger.Continue, () => _model.Security.CheckAnswers != "Yes")
               .PermitIf(Trigger.Back, State.DirLoansSub, () => _model.Security.DirLoans == "Yes")
               .PermitIf(Trigger.Back, State.DirLoans, () => _model.Security.DirLoans != "Yes");

            _machine.Configure(State.Complete)
              .Permit(Trigger.Back, State.CheckAnswers);

            _machine.OnTransitionCompletedAsync(x =>
            {
                _model.Security.State = x.Destination;

                _model.Security.RemoveAlternativeRoutesData();

                return _mediator.Send(new BusinessLogic._LoanApplication.Commands.Update() { Model = _model });

            });
        }

     
    }
}
