using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application/{id}/company")]
[Authorize]
public class CompanyStructureV2Controller : Controller
{
    private readonly IMediator _mediator;

    private readonly IValidator<CompanyStructureViewModel> _validator;

    public CompanyStructureV2Controller(IMediator mediator, IValidator<CompanyStructureViewModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("start-company-structure")]
    public IActionResult StartCompanyStructure(Guid id)
    {
        return View("StartCompanyStructure", LoanApplicationId.From(id));
    }

    [HttpPost("start-company-structure")]
    public IActionResult StartCompanyStructurePost(Guid id)
    {
        return RedirectToAction("Purpose", new { id });
    }

    [HttpGet("purpose")]
    public async Task<IActionResult> Purpose(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));
        return View("Purpose", response.ViewModel.Company);
    }

    [HttpPost("purpose")]
    public async Task<IActionResult> PurposePost(Guid id)
    {
        await _mediator.Send(new ProvideCompanyPurposeCommand(LoanApplicationId.From(id), new CompanyPurpose(true)));
        return RedirectToAction("MoreInformationAboutOrganization", new { id });
    }

    [HttpGet("more-information-about-organization")]
    public async Task<IActionResult> MoreInformationAboutOrganization(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));
        return View("MoreInformationAboutOrganization", response.ViewModel.Company);
    }

    [HttpPost("more-information-about-organization")]
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
            validationResult.AddToModelState(ModelState, "Company");
            return View("MoreInformationAboutOrganization", viewModel);
        }

        await _mediator.Send(
            new ProvideMoreInformationAboutOrganizationCommand(
                LoanApplicationId.From(id),
                new OrganisationMoreInformation(),
                new OrganisationMoreInformationFile()),
            cancellationToken);

        return RedirectToAction("HowManyHomesBuilt", new { id });
    }

    [HttpGet("how-many-homes-built")]
    public async Task<IActionResult> HowManyHomesBuilt(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));
        return View("HomesBuilt", response.ViewModel.Company);
    }

    [HttpPost("how-many-homes-built")]
    public async Task<IActionResult> HowManyHomesBuiltPost(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));
        return View("HomesBuilt", response.ViewModel.Company);
    }
}
