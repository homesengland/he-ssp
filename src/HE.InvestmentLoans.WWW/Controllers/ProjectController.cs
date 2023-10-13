using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Projects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.Contract.Security.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis;
using NuGet.Common;
using ProjectId = HE.InvestmentLoans.Contract.Application.ValueObjects.ProjectId;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application/{id}/project")]
[AuthorizeWithCompletedProfile]
public class ProjectController : WorkflowController<ProjectState>
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start")]
    [WorkflowState(ProjectState.Index)]
    public async Task<IActionResult> StartProject(Guid id, Guid projectId)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));

        if (response.LoanApplication.IsReadOnly())
        {
            return RedirectToAction("CheckAnswers", new { Id = id, ProjectId = projectId });
        }

        return View("Index", LoanApplicationId.From(id));
    }

    [HttpPost("start")]
    [WorkflowState(ProjectState.Index)]
    public async Task<IActionResult> StartProjectPost(Guid id, [FromQuery] Guid? existingProjectId)
    {
        if (existingProjectId.IsProvided())
        {
            return RedirectToAction(nameof(ProjectName), new { id, projectId = existingProjectId });
        }

        var result = await _mediator.Send(new CreateProjectCommand(LoanApplicationId.From(id)));

        return await Continue(new { id, projectId = result.ReturnedData.Value });
    }

    [HttpGet("{projectId}/delete")]
    public IActionResult Delete(Guid id, Guid projectId)
    {
        return View(new ProjectViewModel { ApplicationId = id, ProjectId = projectId });
    }

    [HttpPost("{projectId}/delete")]
    public async Task<IActionResult> Delete(Guid id, Guid projectId, ProjectViewModel project)
    {
        if (project.DeleteProject == CommonResponse.Yes)
        {
            await _mediator.Send(new DeleteProjectCommand(LoanApplicationId.From(id), ProjectId.From(projectId)));
        }

        return RedirectToAction(
            nameof(LoanApplicationV2Controller.TaskList),
            new ControllerName(nameof(LoanApplicationV2Controller)).WithoutPrefix(),
            new { id });
    }

    [HttpGet("{projectId}/name")]
    [WorkflowState(ProjectState.Name)]
    public async Task<IActionResult> ProjectName(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.ProjectName));

        return View(result);
    }

    [HttpPost("{projectId}/name")]
    [WorkflowState(ProjectState.Name)]
    public async Task<IActionResult> ProjectName(Guid id, Guid projectId, ProjectViewModel model, [FromQuery] string redirect, CancellationToken token)
    {
        var result = await _mediator.Send(new ChangeProjectNameCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.Name), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("ProjectName", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/start-date")]
    [WorkflowState(ProjectState.StartDate)]
    public async Task<IActionResult> StartDate(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.StartDate));

        return View(result);
    }

    [HttpPost("{projectId}/start-date")]
    [WorkflowState(ProjectState.StartDate)]
    public async Task<IActionResult> StartDate(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideStartDateCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.HasEstimatedStartDate,
                model.EstimatedStartYear,
                model.EstimatedStartMonth,
                model.EstimatedStartDay),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("StartDate", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/many-homes")]
    [WorkflowState(ProjectState.ManyHomes)]
    public async Task<IActionResult> ManyHomes(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.ManyHomes));

        return View(result);
    }

    [HttpPost("{projectId}/many-homes")]
    [WorkflowState(ProjectState.ManyHomes)]
    public async Task<IActionResult> ManyHomes(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideHomesCountCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.HomesCount), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("ManyHomes", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/type-homes")]
    [WorkflowState(ProjectState.TypeHomes)]
    public async Task<IActionResult> TypeHomes(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.TypeOfHomes));

        return View(result);
    }

    [HttpPost("{projectId}/type-homes")]
    [WorkflowState(ProjectState.TypeHomes)]
    public async Task<IActionResult> TypeHomes(Guid id, Guid projectId, ProjectViewModel model, [FromQuery] string redirect, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideHomesTypesCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.HomeTypes, model.OtherHomeTypes), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("TypeHomes", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/type")]
    [WorkflowState(ProjectState.Type)]
    public async Task<IActionResult> Type(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.TypeOfProject));

        return View(result);
    }

    [HttpPost("{projectId}/type")]
    [WorkflowState(ProjectState.Type)]
    public async Task<IActionResult> Type(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideProjectTypeCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.ProjectType), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Type", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/planning-ref-number-exists")]
    [WorkflowState(ProjectState.PlanningRef)]
    public async Task<IActionResult> PlanningReferenceNumberExists(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(
            LoanApplicationId.From(id),
            ProjectId.From(projectId),
            ProjectFieldsSet.PlanningReferenceNumberExists));

        return View(result);
    }

    [HttpPost("{projectId}/planning-ref-number-exists")]
    [WorkflowState(ProjectState.PlanningRef)]
    public async Task<IActionResult> PlanningReferenceNumberExists(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvidePlanningReferenceNumberCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.PlanningReferenceNumberExists,
                null),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("PlanningReferenceNumberExists", model);
        }

        return await Continue(new { id, projectId, redirect });
    }

    [HttpGet("{projectId}/planning-ref-number")]
    [WorkflowState(ProjectState.PlanningRefEnter)]
    public async Task<IActionResult> PlanningReferenceNumber(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.PlanningReferenceNumber));

        return View(result);
    }

    [HttpPost("{projectId}/planning-ref-number")]
    [WorkflowState(ProjectState.PlanningRefEnter)]
    public async Task<IActionResult> PlanningReferenceNumber(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvidePlanningReferenceNumberCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                CommonResponse.Yes,
                model.PlanningReferenceNumber),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("PlanningReferenceNumber", model);
        }

        return await Continue(new { id, projectId, redirect });
    }

    [HttpGet("{projectId}/planning-permission-status")]
    [WorkflowState(ProjectState.PlanningPermissionStatus)]
    public async Task<IActionResult> PlanningPermissionStatus(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(
            LoanApplicationId.From(id),
            ProjectId.From(projectId),
            ProjectFieldsSet.PlanningPermissionStatus));

        return View(result);
    }

    [HttpPost("{projectId}/planning-permission-status")]
    [WorkflowState(ProjectState.PlanningPermissionStatus)]
    public async Task<IActionResult> PlanningPermissionStatus(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvidePlanningPermissionStatusCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.PlanningPermissionStatus),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/location")]
    [WorkflowState(ProjectState.Location)]
    public async Task<IActionResult> Location(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.Location));

        return View(result);
    }

    [HttpPost("{projectId}/location")]
    [WorkflowState(ProjectState.Location)]
    public async Task<IActionResult> Location(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideLocationCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.LocationOption,
                model.LocationCoordinates,
                model.LocationLandRegistry),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/ownership")]
    [WorkflowState(ProjectState.Ownership)]
    public async Task<IActionResult> Ownership(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.Ownership));

        return View(result);
    }

    [HttpPost("{projectId}/ownership")]
    [WorkflowState(ProjectState.Ownership)]
    public async Task<IActionResult> Ownership(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideLandOwnershipCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.Ownership), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(model);
        }

        if (model.Ownership == CommonResponse.Yes)
        {
            return await Continue(new { id, projectId, redirect });
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/additional-details")]
    [WorkflowState(ProjectState.Additional)]
    public async Task<IActionResult> AdditionalDetails(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.AdditionalDetails));

        return View(result);
    }

    [HttpPost("{projectId}/additional-details")]
    [WorkflowState(ProjectState.Additional)]
    public async Task<IActionResult> AdditionalDetails(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideAdditionalDetailsCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.PurchaseYear,
                model.PurchaseMonth,
                model.PurchaseDay,
                model.Cost,
                model.Value,
                model.Source),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/grant-funding-exists")]
    [WorkflowState(ProjectState.GrantFunding)]
    public async Task<IActionResult> GrantFundingExists(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.GrantFundingExists));

        return View(result);
    }

    [HttpPost("{projectId}/grant-funding-exists")]
    [WorkflowState(ProjectState.GrantFunding)]
    public async Task<IActionResult> GrantFundingExists(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideGrantFundingStatusCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.GrantFundingStatus),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(model);
        }

        if (model.GrantFundingStatus == CommonResponse.Yes)
        {
            return await Continue(new { id, projectId, redirect });
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/grant-funding")]
    [WorkflowState(ProjectState.GrantFundingMore)]
    public async Task<IActionResult> GrantFunding(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.GrantFunding));

        return View(result);
    }

    [HttpPost("{projectId}/grant-funding")]
    [WorkflowState(ProjectState.GrantFundingMore)]
    public async Task<IActionResult> GrantFunding(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideGrantFundingInformationCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.GrantFundingProviderName,
                model.GrantFundingAmount,
                model.GrantFundingName,
                model.GrantFundingPurpose),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/charges-debt")]
    [WorkflowState(ProjectState.ChargesDebt)]
    public async Task<IActionResult> ChargesDebt(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.ChargesDebt));

        return View(result);
    }

    [HttpPost("{projectId}/charges-debt")]
    [WorkflowState(ProjectState.ChargesDebt)]
    public async Task<IActionResult> ChargesDebt(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideChargesDebtCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.ChargesDebt, model.ChargesDebtInfo), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("ChargesDebt", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/affordable-homes")]
    [WorkflowState(ProjectState.AffordableHomes)]
    public async Task<IActionResult> AffordableHomes(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.AffordableHomes));

        return View(result);
    }

    [HttpPost("{projectId}/affordable-homes")]
    [WorkflowState(ProjectState.AffordableHomes)]
    public async Task<IActionResult> AffordableHomes(Guid id, Guid projectId, [FromQuery] string redirect, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideAffordableHomesCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.AffordableHomes), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("AffordableHomes", model);
        }

        return await Continue(redirect, new { id, projectId });
    }

    [HttpGet("{projectId}/check-answers")]
    [WorkflowState(ProjectState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.GetAllFields));

        return View(result);
    }

    [HttpPost("{projectId}/check-answers")]
    [WorkflowState(ProjectState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new CheckProjectAnswersCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.CheckAnswers), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            var response = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.GetAllFields), token);
            return View("CheckAnswers", response);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/back")]
    public Task<IActionResult> Back(ProjectState currentPage, Guid id, Guid projectId)
    {
        return Back(currentPage, new { id, projectId });
    }

    protected override async Task<IStateRouting<ProjectState>> Routing(ProjectState currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;
        var projectId = Request.RouteValues.FirstOrDefault(x => x.Key == "projectId").Value as string;

        if (projectId.IsNotProvided())
        {
            return ProjectWorkflow.ForStartPage();
        }

        var response = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id!), ProjectId.From(projectId!), ProjectFieldsSet.GetAllFields));

        return new ProjectWorkflow(currentState, response);
    }
}
