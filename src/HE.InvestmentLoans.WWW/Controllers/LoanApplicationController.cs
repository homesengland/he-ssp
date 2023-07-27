using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL = HE.InvestmentLoans.BusinessLogic;

namespace HE.InvestmentLoans.WWW.Controllers;

[SuppressMessage("Usage", "CA1801", Justification = "It should be fixed in the future")]
[Route("application")]
[Authorize]
public class LoanApplicationController : Controller
{
    private readonly IMediator _mediator;
    private readonly IValidator<LoanApplicationViewModel> _validator;
    private readonly ICacheService _cacheService;

    public LoanApplicationController(IMediator mediator, IValidator<LoanApplicationViewModel> validator, ICacheService cacheService)
    {
        this._mediator = mediator;
        this._validator = validator;
        this._cacheService = cacheService;
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> IndexPost(string action)
    {
        var model = await this._mediator.Send(new BL.LoanApplicationLegacy.Commands.Create());
        var workflow = new LoanApplicationWorkflow(model, _mediator);
        return RedirectToAction("Workflow", new { id = model.ID, ending = workflow.GetName() });
    }

    [Route("{id}/{ending?}")]
    public async Task<IActionResult> Workflow(Guid id, string ending)
    {
        var model = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var workflow = new LoanApplicationWorkflow(model, _mediator);
        var (isDeletedProjectInCache, deletedProjectFromCache) = model.ToggleDeleteProjectName(_cacheService);

        if (isDeletedProjectInCache)
        {
            ViewBag.AdditionalData = deletedProjectFromCache;
        }

        return View(workflow.GetName(), model);
    }

    [HttpPost]
    [Route("{id}/{ending?}")]
    public async Task<IActionResult> WorkflowPost(Guid id, LoanApplicationViewModel model, string ending, string action)
    {
        var sessionModel = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var workflow = new LoanApplicationWorkflow(sessionModel, _mediator);

        try
        {
            var validationResult = _validator.Validate(model, opt => opt.IncludeRuleSets(workflow.GetName()));
            if (!validationResult.IsValid)
            {
                // error messages in the View.
                validationResult.AddToModelState(ModelState);

                // re-render the view when validation failed.
                return View(workflow.GetName(), model);
            }

            var result = await this._mediator.Send(new BL.LoanApplicationLegacy.Commands.Update()
            {
                Model = sessionModel,
                TryUpdateModelAction = x => this.TryUpdateModelAsync(x),
            }).ConfigureAwait(false);

            workflow.NextState(Enum.Parse<Trigger>(action));
        }
        catch (Common.Exceptions.ValidationException ex)
        {
            ex.Results.ForEach(item => item.AddToModelState(ModelState, null));
        }

        return RedirectToAction("Workflow", new { id = sessionModel.ID, ending = workflow.GetName() });
    }

    [HttpGet("report-problem")]
    public IActionResult ReportProblem()
    {
        throw new NotFoundException("Site", nameof(ReportProblem));
    }

    [Route("GoBack")]
    public async Task<IActionResult> GoBack(Guid id, LoanApplicationViewModel model, string ending, string action)
    {
        model = await this._mediator.Send(new BL.LoanApplicationLegacy.Queries.GetSingle() { Id = id });
        var workflow = new LoanApplicationWorkflow(model, _mediator);
        workflow.NextState(Trigger.Back);

        if (workflow.GetName() == LoanApplicationWorkflow.State.AboutLoan.ToString())
        {
            return RedirectToAction("AboutLoan", "LoanApplicationV2");
        }

        return RedirectToAction("Workflow", new { id = model.ID, ending = workflow.GetName() });
    }
}
