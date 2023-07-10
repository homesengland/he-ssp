namespace HE.InvestmentLoans.WWW.Controllers
{
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
    using HE.InvestmentLoans.BusinessLogic.ViewModel;
    using HE.InvestmentLoans.Common.Routing;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Crm.Sdk.Messages;
    using BL = BusinessLogic;

    [Route("application/{id}/security")]
    [Authorize]
    public class SecurityController : Controller
    {
        private readonly ILogger<SecurityController> logger;
        private readonly IMediator mediator;
        private readonly IValidator<SecurityViewModel> validator;

        public SecurityController(IValidator<SecurityViewModel> validator, ILogger<SecurityController> logger, IMediator mediator)
            : base()
        {
            this.logger = logger;
            this.mediator = mediator;
            this.validator = validator;
        }

        [Route("{ending?}")]
        public async Task<IActionResult> Workflow(Guid id, string ending)
        {
            var model = await mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });

            var workflow = new SecurityWorkflow(model, mediator);

            if (workflow.IsCompleted())
            {
                workflow.NextState(Trigger.Back);
            }

            return View(workflow.GetName(), model);
        }

        [HttpPost]
        [Route("{ending?}")]
        public async Task<IActionResult> WorkflowPost(Guid id, string ending, string action)
        {
            var sessionModel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SecurityWorkflow workflow = new SecurityWorkflow(sessionModel, mediator);

            try
            {
                await TryUpdateModelAsync(sessionModel);
                var vresult = validator.Validate(sessionModel.Security, opt => opt.IncludeRuleSets(workflow.GetName()));
                if (!vresult.IsValid)
                {
                    // error messages in the View.
                    vresult.AddToModelState(this.ModelState, "Security");
                    // re-render the view when validation failed.
                    return View(workflow.GetName(), sessionModel);
                }

                var result = await this.mediator.Send(new BL._LoanApplication.Commands.Update()
                {
                    Model = sessionModel,
                    TryUpdateModelAction = x => this.TryUpdateModelAsync(x)
                }).ConfigureAwait(false);

                workflow.NextState(Enum.Parse<Trigger>(action));
            }
            catch (Common.Exceptions.ValidationException ex)
            {
                ex.Results.ForEach(item => item.AddToModelState(ModelState, null));
            }

            var loanWorkflow = new LoanApplicationWorkflow(sessionModel, mediator);
            if (loanWorkflow.IsBeingChecked() || workflow.IsCompleted() || (sessionModel.Security.CheckAnswers == "No" && action != "Change"))
            {
                return RedirectToAction("Workflow", "LoanApplication", new { id = sessionModel.ID, ending = loanWorkflow.GetName() });
            }
            else
            {
                return RedirectToAction("Workflow", new { id = sessionModel.ID, ending = workflow.GetName() });
            }
        }

        [Route("GoBack")]
        public async Task<IActionResult> GoBack(Guid id, LoanApplicationViewModel model, string action)
        {
            model = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SecurityWorkflow workflow = new SecurityWorkflow(model, mediator);
            workflow.NextState(Trigger.Back);
            return RedirectToAction("Workflow", new { id = model.ID, ending = workflow.GetName() });
        }

        [Route("Change")]
        public async Task<IActionResult> Change(Guid id, string state)
        {
            var sessionmodel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SecurityWorkflow workflow = new SecurityWorkflow(sessionmodel, mediator);
            workflow.ChangeState(Enum.Parse<HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow.SecurityWorkflow.State>(state));
            return RedirectToAction("Workflow", new { id = sessionmodel.ID, ending = workflow.GetName() });
        }
    }
}
