using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BL = HE.InvestmentLoans.BusinessLogic;

namespace HE.InvestmentLoans.WWW.Controllers
{
    [Route("application/{id}/site")]
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
            var site = new SiteViewModel() { Id = Guid.NewGuid() };
            model.Sites.Add(site);
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
            if (workflow.IsCompleted())
            {
                workflow.NextState(BL.Routing.Trigger.Back);
            }

            return View(workflow.GetName(), sitemodel);
        }

        [HttpPost]
        [Route("{site}/{ending?}")]
        public async Task<IActionResult> WorkflowPost(Guid id, Guid site, SiteViewModel model, string ending, string action)
        {
            var sessionModel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });

            SiteWorkflow workflow = new SiteWorkflow(sessionModel, mediator, site);
            var sitemodel = sessionModel.Sites.Where(item => item.Id == site).First();
            if (Request.Form.Keys.Contains("EstimatedStartDate.Value.Day")
                && Request.Form.Keys.Contains("EstimatedStartDate.Value.Month")
                && Request.Form.Keys.Contains("EstimatedStartDate.Value.Year"))
            {
                try
                {
                    sitemodel.EstimatedStartDate = new DateTime(
                        int.Parse(Request.Form["EstimatedStartDate.Value.Year"], System.Globalization.CultureInfo.CurrentCulture.NumberFormat),
                        int.Parse(Request.Form["EstimatedStartDate.Value.Month"], System.Globalization.CultureInfo.CurrentCulture.NumberFormat),
                        int.Parse(Request.Form["EstimatedStartDate.Value.Day"], System.Globalization.CultureInfo.CurrentCulture.NumberFormat));
                    model.EstimatedStartDate = sitemodel.EstimatedStartDate;
                }
                catch
                {
                }

            }

            try
            {
                var vresult = validator.Validate(model, opt => opt.IncludeRuleSets(workflow.GetName()));
                if (!vresult.IsValid)
                {
                    // error messages in the View.
                    vresult.AddToModelState(this.ModelState);
                    // re-render the view when validation failed.
                    model.Id = site;

                    await TryUpdateModelAsync(sitemodel);
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

            if (workflow.IsCompleted() || sitemodel.CheckAnswers == "No")
            {
                var loanWorkflow = new LoanApplicationWorkflow(sessionModel, mediator);
                return RedirectToAction("Workflow", "LoanApplication", new { id = sessionModel.ID, ending = loanWorkflow.GetName() });
            }
            else
            {
                return RedirectToAction("Workflow", new { id = sessionModel.ID, site = sitemodel.Id, ending = workflow.GetName() });
            }
        }

        [Route("GoBack")]
        public async Task<IActionResult> GoBack(Guid id, Guid site, SiteViewModel model, string action)
        {
            var sessionmodel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SiteWorkflow workflow = new SiteWorkflow(sessionmodel, mediator, site);
            workflow.NextState(BL.Routing.Trigger.Back);
            return RedirectToAction("Workflow", new { id = sessionmodel.ID, site = site, ending = workflow.GetName() });
        }

        [Route("{site}/Change")]
        public async Task<IActionResult> Change(Guid id, Guid site, SiteViewModel model, string state)
        {
            var sessionmodel = await this.mediator.Send(new BL._LoanApplication.Queries.GetSingle() { Id = id });
            SiteWorkflow workflow = new SiteWorkflow(sessionmodel, mediator, site);
            workflow.ChangeState(Enum.Parse<HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow.SiteWorkflow.State>(state));
            return RedirectToAction("Workflow", new { id = sessionmodel.ID, site = site, ending = workflow.GetName() });
        }
    }
}
