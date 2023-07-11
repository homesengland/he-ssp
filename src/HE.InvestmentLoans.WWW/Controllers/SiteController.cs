using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.Application.Project.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL = HE.InvestmentLoans.BusinessLogic;

namespace HE.InvestmentLoans.WWW.Controllers
{
    [Route("application/{id}/site")]
    [Authorize]
    public class SiteController : Controller
    {
        private readonly ILogger<SecurityController> logger;
        private readonly IMediator mediator;
        private readonly IValidator<SiteViewModel> validator;

        public SiteController(IValidator<SiteViewModel> validator, ILogger<SecurityController> logger, IMediator mediator)
            : base()
        {
            this.logger = logger;
            this.mediator = mediator;
            this.validator = validator;
        }

        [Route("Create")]
        public async Task<IActionResult> CreateSite(Guid id)
        {
            var model = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            var site = model.AddNewSite();
            SiteWorkflow workflow = new SiteWorkflow(model, mediator, site.Id);
            var result = await this.mediator.Send(new BL._LoanApplication.Commands.Update()
            {
                Model = model,
                TryUpdateModelAction = null
            }).ConfigureAwait(false);

            return RedirectToAction("Workflow", new { id = model.ID, site = site.Id, ending = workflow.GetName() });
        }

        [Route("{site}/{ending?}")]
        public async Task<IActionResult> Workflow(Guid id, Guid site, string ending)
        {
            var model = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            var sitemodel = model.Sites.Where(item => item.Id == site).First();
            SiteWorkflow workflow = new SiteWorkflow(model, mediator, site);
            if (ending == "DeleteProject")
            {
                sitemodel.PreviousState = sitemodel.State;
                workflow.ChangeState(SiteWorkflow.State.DeleteProject, false);
            }

            if (workflow.IsCompleted())
            {
                workflow.NextState(BL.Routing.Trigger.Back);
            }

            return View(workflow.GetName(), sitemodel);
        }

        [HttpPost]
        [Route("{site}/{ending?}")]
        public async Task<IActionResult> WorkflowPost(Guid id, Guid site,  string ending, string action)
        {
            var sessionModel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });

            SiteWorkflow workflow = new SiteWorkflow(sessionModel, mediator, site);
            var sitemodel = sessionModel.Sites.Where(item => item.Id == site).First();

            try
            {
                await TryUpdateModelAsync(sitemodel);
                var vresult = validator.Validate(sitemodel, opt => opt.IncludeRuleSets(workflow.GetName()));
                if (!vresult.IsValid)
                {
                    // error messages in the View.
                    vresult.AddToModelState(this.ModelState);
                    // re-render the view when validation failed.
                    sitemodel.Id = site;

                    return View(workflow.GetName(), sitemodel);
                }

                await TryUpdateModelAsync(sitemodel);
                sitemodel.Id = site;
                var result = await this.mediator.Send(new BL._LoanApplication.Commands.Update()
                {
                    Model = sessionModel,
                    TryUpdateModelAction = null
                }).ConfigureAwait(false);

                workflow.NextState(Enum.Parse<BL.Routing.Trigger>(action));
            }
            catch (BL.Exceptions.ValidationException ex)
            {
                ex.Results.ForEach(item => item.AddToModelState(ModelState, null));
            }

            var loanWorkflow = new LoanApplicationWorkflow(sessionModel, mediator);
            if (loanWorkflow.IsBeingChecked() || workflow.IsCompleted() || (sitemodel.CheckAnswers == "No" && action != "Change"))
            {
                return RedirectToAction("Workflow", "LoanApplication", new { id = sessionModel.ID, ending = loanWorkflow.GetName() });
            }
            else
            {
                return RedirectToAction("Workflow", new { id = sessionModel.ID, site = sitemodel.Id, ending = workflow.GetName() });
            }
        }

        [Route("GoBack")]
        public async Task<IActionResult> GoBack(Guid id, Guid site, string action)
        {
            var sessionmodel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SiteWorkflow workflow = new SiteWorkflow(sessionmodel, mediator, site);
            workflow.NextState(BL.Routing.Trigger.Back);
            return RedirectToAction("Workflow", new { id = sessionmodel.ID, site = site, ending = workflow.GetName() });
        }

        [Route("{site}/Change")]
        public async Task<IActionResult> Change(Guid id, Guid site, string state)
        {
            var sessionmodel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SiteWorkflow workflow = new SiteWorkflow(sessionmodel, mediator, site);
            workflow.ChangeState(Enum.Parse<HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow.SiteWorkflow.State>(state), true);
            return RedirectToAction("Workflow", new { id = sessionmodel.ID, site = site, ending = workflow.GetName() });
        }

        [HttpPost]
        [Route("{site}/DeleteProject")]
        public async Task<IActionResult> Delete(Guid id, Guid site, string state)
        {
            var model = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            var sitemodel = model.Sites.FirstOrDefault(item => item.Id == site);

            if (Request.Form["DeleteProject"] == "Yes")
            {
                await this.mediator.Send(new DeleteProjectCommand(id, site));
            }
            else
            {
                SiteWorkflow workflow = new SiteWorkflow(model, mediator, site);
                workflow.ChangeState(sitemodel.PreviousState, false);
            }

            return RedirectToAction("Workflow", "LoanApplication", new { id, ending = "TaskList" });
        }
    }
}
