using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BL = HE.InvestmentLoans.BusinessLogic;

namespace HE.InvestmentLoans.WWW.Controllers
{
    [Route("application")]
    public class LoanApplicationController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMediator mediator;
        private readonly IValidator<LoanApplicationViewModel> validator;

        public LoanApplicationController(ILogger<HomeController> logger, IMediator mediator, IValidator<LoanApplicationViewModel> validator)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.validator = validator;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> IndexPost(string action)
        {
            var model = await this.mediator.Send(new BL._LoanApplication.Commands.Create());
            LoanApplicationWorkflow workflow = new LoanApplicationWorkflow(model, mediator);
            return RedirectToAction("Workflow", new { id = model.ID, ending = workflow.GetName() });
        }

        [Route("{id}/{ending?}")]
        public async Task<IActionResult> Workflow(Guid id, string ending)
        {
            var model = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            LoanApplicationWorkflow workflow = new LoanApplicationWorkflow(model, mediator);
            return View(workflow.GetName(), model);
        }

        [HttpPost]
        [Route("{id}/{ending?}")]
        public async Task<IActionResult> WorkflowPost(Guid id, LoanApplicationViewModel model, string ending, string action)
        {
            var sessionModel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            var workflow = new LoanApplicationWorkflow(sessionModel, mediator);

            try
            {
                var validationResult = validator.Validate(model, opt => opt.IncludeRuleSets(workflow.GetName()));
                if (!validationResult.IsValid)
                {
                    // error messages in the View.
                    validationResult.AddToModelState(ModelState);
                    // re-render the view when validation failed.
                    return View(workflow.GetName(), model);
                }

                var result = await this.mediator.Send(new BL._LoanApplication.Commands.Update()
                {
                    Model = sessionModel,
                    TryUpdateModelAction = x => this.TryUpdateModelAsync(x)
                }).ConfigureAwait(false);

                workflow.NextState(Enum.Parse<BL.Routing.Trigger>(action));
            }
            catch (BL.Exceptions.ValidationException ex)
            {
                ex.Results.ForEach(item => item.AddToModelState(ModelState, null));
            }

            return RedirectToAction("Workflow", new { id = sessionModel.ID, ending = workflow.GetName() });
        }

        [Route("GoBack")]
        public async Task<IActionResult> GoBack(Guid id, LoanApplicationViewModel model, string ending, string action)
        {
            model = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            LoanApplicationWorkflow workflow = new LoanApplicationWorkflow(model, mediator);
            workflow.NextState(BL.Routing.Trigger.Back);
            return RedirectToAction("Workflow", new { id = model.ID, ending = workflow.GetName() });
        }
    }
}
