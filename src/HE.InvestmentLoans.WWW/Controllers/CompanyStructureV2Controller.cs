using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application/{id}/company")]
[AuthorizeWithCompletedProfile]
public class CompanyStructureV2Controller : WorkflowController<CompanyStructureState>
{
    private readonly IMediator _mediator;

    public CompanyStructureV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start-company-structure")]
    [WorkflowState(CompanyStructureState.StartCompanyStructure)]
    public async Task<IActionResult> StartCompanyStructure(Guid id)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id), CompanyStructureFieldsSet.GetEmpty));
        if (response.ViewModel.IsReadOnly())
        {
            return RedirectToAction("CheckAnswers", new { Id = id });
        }

        return View("StartCompanyStructure", LoanApplicationId.From(id));
    }

    [HttpPost("start-company-structure")]
    [WorkflowState(CompanyStructureState.StartCompanyStructure)]
    public async Task<IActionResult> StartCompanyStructurePost(Guid id)
    {
        return await Continue(new { Id = id });
    }

    [HttpGet("purpose")]
    [WorkflowState(CompanyStructureState.Purpose)]
    public async Task<IActionResult> Purpose(Guid id)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id), CompanyStructureFieldsSet.CompanyPurpose));
        return View("Purpose", response.ViewModel);
    }

    [HttpPost("purpose")]
    [WorkflowState(CompanyStructureState.Purpose)]
    public async Task<IActionResult> PurposePost(Guid id, CompanyStructureViewModel viewModel)
    {
        await _mediator.Send(new ProvideCompanyPurposeCommand(LoanApplicationId.From(id), viewModel.Purpose));
        return await Continue(new { Id = id });
    }

    [HttpGet("more-information-about-organization")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganization(Guid id)
    {
        var loanApplicationId = LoanApplicationId.From(id);
        var response = await _mediator.Send(new GetCompanyStructureQuery(loanApplicationId, CompanyStructureFieldsSet.MoreInformationAboutOrganization));
        response.ViewModel.OrganisationMoreInformationFiles = (await _mediator.Send(new GetCompanyStructureFilesQuery(loanApplicationId))).Items;

        return View("MoreInformationAboutOrganization", response.ViewModel);
    }

    [HttpGet("more-information-about-organization-remove-file")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationRemoveFile(Guid id, string folderPath, string fileName, CancellationToken cancellationToken)
    {
        var loanApplicationId = LoanApplicationId.From(id);

        var result = await _mediator.Send(
            new ProvideMoreInformationAboutOrganizationRemoveFileCommand(loanApplicationId, folderPath, fileName),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
        }

        return RedirectToAction("MoreInformationAboutOrganization", new { Id = id });
    }

    [HttpGet("more-information-about-organization-download-file")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationDownloadFile(Guid id, string folderPath, string fileName, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCompanyStructureFileQuery(id, folderPath, fileName), cancellationToken);

        return File(response.Data, "application/octet-stream", response.Name);
    }

    [HttpPost("more-information-about-organization")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationPost(Guid id, CompanyStructureViewModel viewModel, [FromForm(Name = "File")] List<IFormFile> formFiles, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId.From(id),
                viewModel.OrganisationMoreInformation,
                viewModel.OrganisationMoreInformationFiles,
                formFiles),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("MoreInformationAboutOrganization", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("how-many-homes-built")]
    [WorkflowState(CompanyStructureState.HomesBuilt)]
    public async Task<IActionResult> HowManyHomesBuilt(Guid id)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id), CompanyStructureFieldsSet.HomesBuilt));
        return View("HomesBuilt", response.ViewModel);
    }

    [HttpPost("how-many-homes-built")]
    [WorkflowState(CompanyStructureState.HomesBuilt)]
    public async Task<IActionResult> HowManyHomesBuiltPost(Guid id, CompanyStructureViewModel viewModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideHowManyHomesBuiltCommand(
                LoanApplicationId.From(id),
                viewModel.HomesBuilt),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("HomesBuilt", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("check-answers")]
    [WorkflowState(CompanyStructureState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id), CompanyStructureFieldsSet.GetAllFields), cancellationToken);
        return View("CheckAnswers", response.ViewModel);
    }

    [HttpPost("check-answers")]
    [WorkflowState(CompanyStructureState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswersPost(Guid id, CompanyStructureViewModel viewModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CheckAnswersCompanyStructureSectionCommand(LoanApplicationId.From(id), viewModel.CheckAnswers), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id), CompanyStructureFieldsSet.GetAllFields), cancellationToken);
            return View("CheckAnswers", response.ViewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(Guid id, CompanyStructureState currentPage)
    {
        return await Back(currentPage, new { Id = id });
    }

    protected override async Task<IStateRouting<CompanyStructureState>> Routing(CompanyStructureState currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id!), CompanyStructureFieldsSet.GetAllFields));

        return new CompanyStructureWorkflow(currentState, response.ViewModel);
    }
}
