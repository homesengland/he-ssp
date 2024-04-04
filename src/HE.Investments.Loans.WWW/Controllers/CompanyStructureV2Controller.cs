using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using HE.Investments.Loans.Contract.Documents;
using HE.Investments.Loans.WWW.Workflows;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

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
    public async Task<IActionResult> PurposePost(Guid id, CompanyStructureViewModel viewModel, [FromQuery] string redirect)
    {
        await _mediator.Send(new ProvideCompanyPurposeCommand(LoanApplicationId.From(id), viewModel.Purpose));
        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("more-information-about-organization")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganization(Guid id)
    {
        var loanApplicationId = LoanApplicationId.From(id);
        var response = await _mediator.Send(new GetCompanyStructureQuery(loanApplicationId, CompanyStructureFieldsSet.MoreInformationAboutOrganization));
        var files = await _mediator.Send(new GetCompanyStructureFilesQuery(loanApplicationId));
        response.ViewModel.OrganisationMoreInformationFiles = files.Select(
                x => x with
                {
                    RemoveAction = Url.Action("MoreInformationAboutOrganizationRemoveFile", "CompanyStructureV2", new { id, fileId = x.FileId }) ?? string.Empty,
                    DownloadAction = Url.Action("MoreInformationAboutOrganizationDownloadFile", "CompanyStructureV2", new { id, fileId = x.FileId }) ?? string.Empty,
                })
            .ToList();

        return View("MoreInformationAboutOrganization", response.ViewModel);
    }

    [HttpGet("more-information-about-organization-remove-file")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationRemoveFile(Guid id, string fileId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RemoveMoreInformationAboutOrganizationFileCommand(LoanApplicationId.From(id), FileId.From(fileId)),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
        }

        return RedirectToAction("MoreInformationAboutOrganization", new { Id = id });
    }

    [HttpGet("more-information-about-organization-download-file")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationDownloadFile(Guid id, string fileId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCompanyStructureFileQuery(LoanApplicationId.From(id), FileId.From(fileId)), cancellationToken);

        return File(response.Content, "application/octet-stream", response.Name);
    }

    [HttpPost("more-information-about-organization-upload-file")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationUploadFile(Guid id, [FromForm(Name = "file")] IFormFile file, CancellationToken cancellationToken)
    {
        var documentToUpload = new FileToUpload(file.FileName, file.Length, file.OpenReadStream());
        try
        {
            var result = await _mediator.Send(new UploadOrganisationMoreInformationFileCommand(new LoanApplicationId(id), documentToUpload), cancellationToken);
            return result.HasValidationErrors
                ? new BadRequestObjectResult(result.Errors)
                : Ok(new
                {
                    fileId = result.ReturnedData!.Id?.Value,
                    uploadDetails = $"uploaded {DateHelper.DisplayAsUkFormatDateTime(result.ReturnedData.UploadedOn)} by {result.ReturnedData.UploadedBy}",
                    canBeRemoved = result.ReturnedData.Id.IsProvided(),
                });
        }
        finally
        {
            await documentToUpload.Content.DisposeAsync();
        }
    }

    [HttpPost("more-information-about-organization")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationPost(Guid id, CompanyStructureViewModel viewModel, [FromQuery] string redirect, [FromForm(Name = "OrganisationMoreInformationFile")] List<IFormFile> formFiles, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideMoreInformationAboutOrganizationCommand(LoanApplicationId.From(id), viewModel.OrganisationMoreInformation, formFiles),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("MoreInformationAboutOrganization", viewModel);
        }

        return await Continue(redirect, new { Id = id });
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
    public async Task<IActionResult> HowManyHomesBuiltPost(Guid id, CompanyStructureViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
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

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("check-answers")]
    [WorkflowState(CompanyStructureState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, CancellationToken cancellationToken)
    {
        var loanApplicationId = LoanApplicationId.From(id);
        var response = await _mediator.Send(new GetCompanyStructureQuery(loanApplicationId, CompanyStructureFieldsSet.GetAllFields), cancellationToken);
        response.ViewModel.OrganisationMoreInformationFiles = await _mediator.Send(new GetCompanyStructureFilesQuery(loanApplicationId), cancellationToken);
        return View("CheckAnswers", response.ViewModel);
    }

    [HttpPost("check-answers")]
    [WorkflowState(CompanyStructureState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswersPost(Guid id, CompanyStructureViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CheckAnswersCompanyStructureSectionCommand(LoanApplicationId.From(id), viewModel.CheckAnswers), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.Clear();
            ModelState.AddValidationErrors(result);
            var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id), CompanyStructureFieldsSet.GetAllFields), cancellationToken);
            return View("CheckAnswers", response.ViewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(Guid id, CompanyStructureState currentPage)
    {
        return await Back(currentPage, new { Id = id });
    }

    protected override async Task<IStateRouting<CompanyStructureState>> Routing(CompanyStructureState currentState, object routeData = null)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id!), CompanyStructureFieldsSet.GetAllFields));

        return new CompanyStructureWorkflow(currentState, response.ViewModel);
    }
}
