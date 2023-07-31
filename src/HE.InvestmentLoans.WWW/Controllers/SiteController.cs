using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow.SiteWorkflow;
using BL = HE.InvestmentLoans.BusinessLogic;

namespace HE.InvestmentLoans.WWW.Controllers;

[SuppressMessage("Usage", "CA1801", Justification = "It should be fixed in the future")]
[Route("application/{id}/site")]
[Authorize]
public class SiteController : Controller
{
    private readonly IMediator _mediator;
    private readonly IValidator<SiteViewModel> _validator;
    private readonly ICacheService _cacheService;

    public SiteController(IValidator<SiteViewModel> validator, IMediator mediator, ICacheService cacheService)
        : base()
    {
        this._mediator = mediator;
        this._validator = validator;
        this._cacheService = cacheService;
    }

    [Route("Create")]
    public async Task<IActionResult> CreateSite(Guid id)
    {
        var model = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var site = model.AddNewSite();
        var workflow = new SiteWorkflow(model, _mediator, site.Id.Value);
        await this._mediator.Send(new BL.LoanApplicationLegacy.Commands.Update()
        {
            Model = model,
            TryUpdateModelAction = null,
        }).ConfigureAwait(false);

        return RedirectToAction("Workflow", new { id = model.ID, site = site.Id, ending = workflow.GetName() });
    }

    [Route("{site}/{ending?}")]
    public async Task<IActionResult> Workflow(Guid id, Guid site, string ending)
    {
        var model = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var sitemodel = model.Sites.First(item => item.Id == site);
        var workflow = new SiteWorkflow(model, _mediator, site);
        if (ending == "DeleteProject")
        {
            sitemodel.PreviousState = sitemodel.State;
            workflow.ChangeState(SiteWorkflow.State.DeleteProject, false);
        }

        if (workflow.IsStateComplete())
        {
            workflow.NextState(Trigger.Back);
        }

        return View(workflow.GetName(), sitemodel);
    }

    [HttpPost]
    [Route("{site}/{ending?}")]
    public async Task<IActionResult> WorkflowPost(Guid id, Guid site, string ending, string action)
    {
        var sessionModel = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });

        var workflow = new SiteWorkflow(sessionModel, _mediator, site);
        var sitemodel = sessionModel.Sites.First(item => item.Id == site);

        try
        {
            var originalSessionModel = ObjectUtilities.DeepCopy(sessionModel);
            await TryUpdateModelAsync(sitemodel);
            var vresult = _validator.Validate(sitemodel, opt => opt.IncludeRuleSets(workflow.GetName()));
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

            if (!sessionModel.Equals(originalSessionModel))
            {
                sitemodel.SetFlowCompletion(false);
            }

            var result = await this._mediator.Send(new BL.LoanApplicationLegacy.Commands.Update()
            {
                Model = sessionModel,
                TryUpdateModelAction = null,
            }).ConfigureAwait(false);

            workflow.NextState(Enum.Parse<Trigger>(action));
        }
        catch (Common.Exceptions.ValidationException ex)
        {
            ex.Results.ForEach(item => item.AddToModelState(ModelState, null));
        }

        var loanWorkflow = new LoanApplicationWorkflow(sessionModel, _mediator);
        if (loanWorkflow.IsBeingChecked() || workflow.IsStateComplete() || (sitemodel.CheckAnswers == "No" && action != "Change"))
        {
            return RedirectToAction("TaskList", "LoanApplicationV2", new { id = sessionModel.ID });
        }
        else
        {
            return RedirectToAction("Workflow", new { id = sessionModel.ID, site = sitemodel.Id, ending = workflow.GetName() });
        }
    }

    [Route("GoBack")]
    public async Task<IActionResult> GoBack(Guid id, Guid site, string action)
    {
        var sessionmodel = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var sitemodel = sessionmodel.Sites.FirstOrDefault(item => item.Id == site);

        var workflow = new SiteWorkflow(sessionmodel, _mediator, site);
        workflow.NextState(Trigger.Back);

        if (workflow.GetName() == State.TaskList.ToString())
        {
            workflow.ChangeState(sitemodel.PreviousState, false);
            return RedirectToAction("Workflow", "LoanApplication", new { id, ending = "TaskList" });
        }
        else
        {
            return RedirectToAction("Workflow", new { id = sessionmodel.ID, site, ending = workflow.GetName() });
        }
    }

    [Route("{site}/Change")]
    public async Task<IActionResult> Change(Guid id, Guid site, string state)
    {
        var sessionmodel = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var workflow = new SiteWorkflow(sessionmodel, _mediator, site);
        workflow.ChangeState(Enum.Parse<State>(state), true);
        return RedirectToAction("Workflow", new { id = sessionmodel.ID, site, ending = workflow.GetName() });
    }

    [HttpPost]
    [Route("{site}/DeleteProject")]
    public async Task<IActionResult> Delete(Guid id, Guid site, string state)
    {
        var projectName = string.Empty;
        var model = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var sitemodel = model.Sites.FirstOrDefault(item => item.Id == site);

        if (Request.Form["DeleteProject"] == "Yes")
        {
            await this._mediator.Send(new LegacyDeleteProjectCommand(id, site));
            projectName = sitemodel.Name ?? sitemodel.DefaultName;
            model.ToggleDeleteProjectName(_cacheService, projectName);
        }
        else
        {
            var workflow = new SiteWorkflow(model, _mediator, site);
            workflow.ChangeState(sitemodel.PreviousState, false);
        }

        return RedirectToAction("Workflow", "LoanApplication", new { id, ending = "TaskList" });
    }
}
