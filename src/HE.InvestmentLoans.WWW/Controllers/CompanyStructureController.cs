using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL = HE.InvestmentLoans.BusinessLogic;

namespace HE.InvestmentLoans.WWW.Controllers;

[SuppressMessage("Usage", "CA1801", Justification = "It should be fixed in the future")]
[Route("application/{id}/company")]
[Authorize]
public class CompanyStructureController : Controller
{
    private readonly IMediator _mediator;
    private readonly IValidator<CompanyStructureViewModel> _validator;

    public CompanyStructureController(IValidator<CompanyStructureViewModel> validator, IMediator mediator)
        : base()
    {
        this._mediator = mediator;
        this._validator = validator;
    }

    [Route("{ending?}")]
    public async Task<IActionResult> Workflow(Guid id, string ending)
    {
        var model = await this._mediator.Send(new BL.LoanApplication.Queries.GetSingle() { Id = id });
        var workflow = new CompanyStructureWorkflow(model, _mediator);
        if (workflow.IsCompleted())
        {
            workflow.NextState(Trigger.Back);
        }

        return View(workflow.GetName(), model);
    }

    [HttpPost]
    [Route("{ending?}")]
    public async Task<IActionResult> WorkflowPost(Guid id, string ending, string action, [FromForm(Name = "File")] IFormFile formFile)
    {
        var sessionModel = await this._mediator.Send(new BL.LoanApplication.Queries.GetSingle() { Id = id });
        if (formFile != null)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            sessionModel.Company.CompanyInfoFile = memoryStream.ToArray();
            sessionModel.Company.CompanyInfoFileName = formFile.FileName;
        }

        var workflow = new CompanyStructureWorkflow(sessionModel, _mediator);

        try
        {
            await TryUpdateModelAsync(sessionModel);
            var vresult = _validator.Validate(sessionModel.Company, opt => opt.IncludeRuleSets(workflow.GetName()));
            if (!vresult.IsValid)
            {
                // error messages in the View.
                vresult.AddToModelState(this.ModelState, "Company");

                // re-render the view when validation failed.
                return View(workflow.GetName(), sessionModel);
            }

            var result = await this._mediator.Send(new BL.LoanApplication.Commands.Update()
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

        var loanWorkflow = new LoanApplicationWorkflow(sessionModel, _mediator);
        if (loanWorkflow.IsBeingChecked() || workflow.IsCompleted() || (sessionModel.Company.CheckAnswers == "No" && action != "Change"))
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
        model = await this._mediator.Send(new BL.LoanApplication.Queries.GetSingle() { Id = id });
        var workflow = new CompanyStructureWorkflow(model, _mediator);
        workflow.NextState(Trigger.Back);
        return RedirectToAction("Workflow", new { id = model.ID, ending = workflow.GetName() });
    }

    [Route("Change")]
    public async Task<IActionResult> Change(Guid id, string state)
    {
        var sessionmodel = await this._mediator.Send(new BL.LoanApplication.Queries.GetSingle() { Id = id });
        var workflow = new CompanyStructureWorkflow(sessionmodel, _mediator);
        workflow.ChangeState(Enum.Parse<CompanyStructureWorkflow.State>(state));
        return RedirectToAction("Workflow", new { id = sessionmodel.ID, ending = workflow.GetName() });
    }
}
