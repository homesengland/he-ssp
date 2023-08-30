using FluentValidation;
using FluentValidation.AspNetCore;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
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

    private readonly IValidator<CompanyStructureViewModel> _validator;

    public CompanyStructureV2Controller(IMediator mediator, IValidator<CompanyStructureViewModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("start-company-structure")]
    [WorkflowState(CompanyStructureState.Index)]
    public IActionResult StartCompanyStructure(Guid id)
    {
        return View("StartCompanyStructure", LoanApplicationId.From(id));
    }

    [HttpPost("start-company-structure")]
    [WorkflowState(CompanyStructureState.Index)]
    public async Task<IActionResult> StartCompanyStructurePost(Guid id)
    {
        return await Continue(new { Id = id });
    }

    [HttpGet("purpose")]
    [WorkflowState(CompanyStructureState.Purpose)]
    public async Task<IActionResult> Purpose(Guid id)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id)));
        return View("Purpose", response.ViewModel);
    }

    [HttpPost("purpose")]
    [WorkflowState(CompanyStructureState.Purpose)]
    public async Task<IActionResult> PurposePost(Guid id, CompanyStructureViewModel viewModel)
    {
        var companyPurpose = CompanyStructureViewModelMapper.MapCompanyPurpose(viewModel.Purpose);
        await _mediator.Send(new ProvideCompanyPurposeCommand(LoanApplicationId.From(id), companyPurpose));
        return await Continue(new { Id = id });
    }

    [HttpGet("more-information-about-organization")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganization(Guid id)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id)));
        return View("MoreInformationAboutOrganization", response.ViewModel);
    }

    [HttpPost("more-information-about-organization")]
    [WorkflowState(CompanyStructureState.ExistingCompany)]
    public async Task<IActionResult> MoreInformationAboutOrganizationPost(Guid id, CompanyStructureViewModel viewModel, [FromForm(Name = "File")] IFormFile formFile, CancellationToken cancellationToken)
    {
        if (formFile != null)
        {
            viewModel.CompanyInfoFileName = formFile.FileName;
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream, cancellationToken);
            viewModel.CompanyInfoFile = memoryStream.ToArray();
        }

        var validationResult = await _validator.ValidateAsync(viewModel, opt => opt.IncludeRuleSets(CompanyStructureView.MoreInformationAboutOrganization), cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View("MoreInformationAboutOrganization", viewModel);
        }

        await _mediator.Send(
            new ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId.From(id),
                CompanyStructureViewModelMapper.MapOrganisationMoreInformation(viewModel.ExistingCompany),
                CompanyStructureViewModelMapper.MapOrganisationMoreInformationFile(viewModel.CompanyInfoFileName, viewModel.CompanyInfoFile)),
            cancellationToken);

        return await Continue(new { Id = id });
    }

    [HttpGet("how-many-homes-built")]
    [WorkflowState(CompanyStructureState.HomesBuilt)]
    public async Task<IActionResult> HowManyHomesBuilt(Guid id)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id)));
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

        if (result.AreValidationErrors)
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
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id)), cancellationToken);
        return View("CheckAnswers", response.ViewModel);
    }

    [HttpPost("check-answers")]
    [WorkflowState(CompanyStructureState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswersPost(Guid id, CompanyStructureViewModel viewModel, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id)), cancellationToken);
        response.ViewModel.CheckAnswers = viewModel.CheckAnswers;
        var validationResult = await _validator.ValidateAsync(response.ViewModel, opt => opt.IncludeRuleSets(CompanyStructureView.CheckAnswers), cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View("CheckAnswers", response.ViewModel);
        }

        await (viewModel.CheckAnswers switch
        {
            CommonResponse.Yes => _mediator.Send(new CompanyStructureSectionCommand(LoanApplicationId.From(id)), cancellationToken),
            CommonResponse.No => _mediator.Send(new UnCompleteCompanyStructureSectionCommand(LoanApplicationId.From(id)), cancellationToken),
            _ => Task.CompletedTask,
        });

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(Guid id, CompanyStructureState currentPage)
    {
        return await Back(currentPage, new { Id = id });
    }

    protected override IStateRouting<CompanyStructureState> Routing(CompanyStructureState currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;
        var response = _mediator.Send(new GetCompanyStructureQuery(LoanApplicationId.From(id!))).Result;

        return new CompanyStructureWorkflow(response.ViewModel, currentState);
    }
}
