using System.Diagnostics.Metrics;
using HE.InvestmentLoans.BusinessLogic.Projects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Models;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        if (projectId != Guid.Empty)
        {
            var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.ProjectName));

            if (result.IsReadOnly())
            {
                return RedirectToAction("CheckAnswers", new { Id = id, ProjectId = projectId });
            }
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
        var result = await _mediator.Send(new ChangeProjectNameCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.ProjectName), token);

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
        var result = await _mediator.Send(
            new ProvideHomesTypesCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.HomeTypes, model.OtherHomeTypes),
            token);

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
    public async Task<IActionResult> PlanningReferenceNumberExists(
        Guid id,
        Guid projectId,
        [FromQuery] string redirect,
        ProjectViewModel model,
        CancellationToken token)
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
    public async Task<IActionResult> PlanningReferenceNumber(
        Guid id,
        Guid projectId,
        [FromQuery] string redirect,
        ProjectViewModel model,
        CancellationToken token)
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
    public async Task<IActionResult> PlanningPermissionStatus(
        Guid id,
        Guid projectId,
        [FromQuery] string redirect,
        ProjectViewModel model,
        CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvidePlanningPermissionStatusCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.PlanningPermissionStatus),
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

    [HttpGet("{projectId}/local-authority/search")]
    [WorkflowState(ProjectState.ProvideLocalAuthority)]
    public IActionResult LocalAuthoritySearch(Guid id, Guid projectId)
    {
        return View(new LocalAuthoritiesViewModel
        {
            ApplicationId = id,
            ProjectId = projectId,
        });
    }

    [HttpPost("{projectId}/local-authority/search")]
    public IActionResult LocalAuthoritySearch(Guid id, Guid projectId, LocalAuthoritiesViewModel organization)
    {
        return RedirectToAction(nameof(LocalAuthorityResult), new { id, projectId, phrase = organization.Phrase });
    }

    [HttpGet("{projectId}/local-authority/search/result")]
    public async Task<IActionResult> LocalAuthorityResult(Guid id, Guid projectId, [FromQuery] string phrase, CancellationToken token, [FromQuery] int page = 0)
    {
        var result = await _mediator.Send(new SearchLocalAuthoritiesQuery(phrase, page - 1, 10), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View(nameof(LocalAuthoritySearch));
        }

        if (result.ReturnedData.TotalItems == 0)
        {
            return RedirectToAction(nameof(LocalAuthorityNotFound), new { id, projectId });
        }

        var viewModel = result.ReturnedData;

        viewModel.ApplicationId = id;
        viewModel.ProjectId = projectId;
        viewModel.Page = page;

        return View(viewModel);
    }

    [HttpGet("{projectId}/local-authority/not-found")]
    public IActionResult LocalAuthorityNotFound(Guid id, Guid projectId)
    {
        return View(new LocalAuthoritiesViewModel
        {
            ApplicationId = id,
            ProjectId = projectId,
        });
    }

    [HttpGet("{projectId}/local-authority/{localAuthorityId}/confirm")]
    public IActionResult LocalAuthorityConfirm(Guid id, Guid projectId, string localAuthorityId)
    {
        var viewModel = new LocalAuthoritiesViewModel
        {
            ApplicationId = id,
            ProjectId = projectId,
            LocalAuthorityId = localAuthorityId,
        };

        return View(new ConfirmModel<LocalAuthoritiesViewModel>(viewModel));
    }

    [HttpPost("{projectId}/local-authority/{localAuthorityId}/confirm")]
    [WorkflowState(ProjectState.ProvideLocalAuthority)]
    public async Task<IActionResult> LocalAuthorityConfirm(Guid id, Guid projectId, string localAuthorityId, ConfirmModel<LocalAuthoritiesViewModel> model, CancellationToken token)
    {
        if (model.Response == CommonResponse.Yes)
        {
            await _mediator.Send(new ConfirmLocalAuthorityCommand(LoanApplicationId.From(id), ProjectId.From(projectId), LocalAuthorityId.From(localAuthorityId)), token);

            return await Continue(new { id, projectId });
        }

        return RedirectToAction(nameof(LocalAuthoritySearch), new { id, projectId });
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
        var result = await _mediator.Send(
            new ProvideChargesDebtCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.ChargesDebt, model.ChargesDebtInfo),
            token);

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
        var result = await _mediator.Send(
            new ProvideAffordableHomesCommand(
                LoanApplicationId.From(id),
                ProjectId.From(projectId),
                model.AffordableHomes),
            token);

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

            var response = await _mediator.Send(
                new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId), ProjectFieldsSet.GetAllFields),
                token);
            return View("CheckAnswers", response);
        }

        return await Continue(new { id });
    }

    [HttpGet("{projectId}/back")]
    public Task<IActionResult> Back(ProjectState currentPage, Guid id, Guid projectId)
    {
        return Back(currentPage, new { id, projectId });
    }

    protected override async Task<IStateRouting<ProjectState>> Routing(ProjectState currentState, object routeData = null)
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
