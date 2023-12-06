using System.Globalization;
using HE.Investment.AHP.Common.Utils;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.WWW.Models.Common;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("Application/{applicationId}/HomeTypes")]
public class HomeTypesController : WorkflowController<HomeTypesWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IAhpDocumentSettings _documentSettings;

    public HomeTypesController(IMediator mediator, IAhpDocumentSettings documentSettings)
    {
        _mediator = mediator;
        _documentSettings = documentSettings;
    }

    [WorkflowState(HomeTypesWorkflowState.Index)]
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        return View(new HomeTypeModelBase(application.Name));
    }

    [HttpGet("Back")]
    public Task<IActionResult> Back([FromRoute] string applicationId, string homeTypeId, HomeTypesWorkflowState currentPage)
    {
        return Back(currentPage, new { applicationId, homeTypeId });
    }

    [WorkflowState(HomeTypesWorkflowState.List)]
    [HttpGet("List")]
    public async Task<IActionResult> List([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(applicationId), cancellationToken);

        return View(new HomeTypeListModel(homeTypes.ApplicationName)
        {
            HomeTypes = homeTypes.HomeTypes.Select(x => new HomeTypeItemModel(x.Id, x.Name, x.HousingType, x.NumberOfHomes)).ToList(),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.List)]
    [HttpPost("List")]
    public async Task<IActionResult> List([FromRoute] string applicationId, HomeTypeListModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SaveFinishHomeTypesAnswerCommand(applicationId, FinishHomeTypesAnswer.Yes, true), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationId });
    }

    [WorkflowState(HomeTypesWorkflowState.FinishHomeTypes)]
    [HttpGet("FinishHomeTypes")]
    public async Task<IActionResult> FinishHomeTypes([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var homeTypesAnswer = await _mediator.Send(new GetFinishHomesTypeAnswerQuery(applicationId), cancellationToken);

        return View(new FinishHomeTypeModel(homeTypesAnswer.ApplicationName) { FinishAnswer = homeTypesAnswer.Answer });
    }

    [WorkflowState(HomeTypesWorkflowState.FinishHomeTypes)]
    [HttpPost("FinishHomeTypes")]
    public async Task<IActionResult> FinishHomeTypes([FromRoute] string applicationId, FinishHomeTypeModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SaveFinishHomeTypesAnswerCommand(applicationId, model.FinishAnswer), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return model.FinishAnswer == FinishHomeTypesAnswer.Yes
            ? RedirectToAction("TaskList", "Application", new { applicationId })
            : await Back(new { applicationId });
    }

    [HttpGet("{homeTypeId}/Duplicate")]
    public async Task<IActionResult> Duplicate([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DuplicateHomeTypeCommand(applicationId, homeTypeId), cancellationToken);
        return RedirectToAction("List", new { applicationId });
    }

    [WorkflowState(HomeTypesWorkflowState.RemoveHomeType)]
    [HttpGet("{homeTypeId}/Remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);

        return View("RemoveHomeTypeConfirmation", new RemoveHomeTypeModel(homeTypeDetails.ApplicationName, homeTypeDetails.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.RemoveHomeType)]
    [HttpPost("{homeTypeId}/Remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string homeTypeId, RemoveHomeTypeModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveHomeTypeCommand(applicationId, homeTypeId), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("RemoveHomeTypeConfirmation", model);
        }

        return RedirectToAction("List", new { applicationId });
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpGet("HomeTypeDetails")]
    public async Task<IActionResult> NewHomeTypeDetails([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        return View("HomeTypeDetails", new HomeTypeDetailsModel(application.Name) { HousingType = GetDefaultHousingType(application.Tenure) });
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpPost("HomeTypeDetails")]
    public async Task<IActionResult> NewHomeTypeDetails([FromRoute] string applicationId, HomeTypeDetailsModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeDetails(applicationId, null, model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeTypeDetails)]
    [HttpGet("{homeTypeId}/HomeTypeDetails")]
    public async Task<IActionResult> HomeTypeDetails([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomeTypeDetailsModel(homeTypeDetails.ApplicationName)
        {
            HomeTypeName = homeTypeDetails.Name,
            HousingType = homeTypeDetails.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeTypeDetails)]
    [HttpPost("{homeTypeId}/HomeTypeDetails")]
    public async Task<IActionResult> HomeTypeDetails([FromRoute] string applicationId, [FromRoute] string homeTypeId, HomeTypeDetailsModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeDetails(applicationId, homeTypeId, model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpGet("{homeTypeId}/HomeInformation")]
    public async Task<IActionResult> HomeInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new HomeInformationModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            NumberOfHomes = homeInformation.NumberOfHomes?.ToString(CultureInfo.InvariantCulture),
            NumberOfBedrooms = homeInformation.NumberOfBedrooms?.ToString(CultureInfo.InvariantCulture),
            MaximumOccupancy = homeInformation.MaximumOccupancy?.ToString(CultureInfo.InvariantCulture),
            NumberOfStoreys = homeInformation.NumberOfStoreys?.ToString(CultureInfo.InvariantCulture),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpPost("{homeTypeId}/HomeInformation")]
    public async Task<IActionResult> HomeInformation([FromRoute] string applicationId, string homeTypeId, HomeInformationModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveHomeInformationCommand(
                applicationId,
                homeTypeId,
                model.NumberOfHomes,
                model.NumberOfBedrooms,
                model.MaximumOccupancy,
                model.NumberOfStoreys),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpGet("{homeTypeId}/HomesForDisabledPeople")]
    public async Task<IActionResult> HomesForDisabledPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var disabledPeopleHomeType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomesForDisabledPeopleModel(disabledPeopleHomeType.ApplicationName, disabledPeopleHomeType.HomeTypeName)
        {
            HousingType = disabledPeopleHomeType.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpPost("{homeTypeId}/HomesForDisabledPeople")]
    public async Task<IActionResult> HomesForDisabledPeople(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomesForDisabledPeopleModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(new SaveDisabledPeopleHousingTypeCommand(applicationId, homeTypeId, model.HousingType), model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleClientGroup)]
    [HttpGet("{homeTypeId}/DisabledPeopleClientGroup")]
    public async Task<IActionResult> DisabledPeopleClientGroup([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var disabledPeopleHomeType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new DisabledPeopleClientGroupModel(disabledPeopleHomeType.ApplicationName, disabledPeopleHomeType.HomeTypeName)
        {
            DisabledPeopleClientGroup = disabledPeopleHomeType.ClientGroupType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleClientGroup)]
    [HttpPost("{homeTypeId}/DisabledPeopleClientGroup")]
    public async Task<IActionResult> DisabledPeopleClientGroup(
        [FromRoute] string applicationId,
        string homeTypeId,
        DisabledPeopleClientGroupModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveDisabledPeopleClientGroupTypeCommand(applicationId, homeTypeId, model.DisabledPeopleClientGroup),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpGet("{homeTypeId}/HomesForOlderPeople")]
    public async Task<IActionResult> HomesForOlderPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var olderPeopleHomeType = await _mediator.Send(new GetOlderPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomesForOlderPeopleModel(olderPeopleHomeType.ApplicationName, olderPeopleHomeType.HomeTypeName) { HousingType = olderPeopleHomeType.HousingType, });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpPost("{homeTypeId}/HomesForOlderPeople")]
    public async Task<IActionResult> HomesForOlderPeople([FromRoute] string applicationId, string homeTypeId, HomesForOlderPeopleModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(new SaveOlderPeopleHousingTypeCommand(applicationId, homeTypeId, model.HousingType), model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HappiDesignPrinciples)]
    [HttpGet("{homeTypeId}/HappiDesignPrinciples")]
    public async Task<IActionResult> HappiDesignPrinciples([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var designPlans = await _mediator.Send(new GetDesignPlansQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HappiDesignPrinciplesModel(designPlans.ApplicationName, designPlans.HomeTypeName)
        {
            DesignPrinciples = designPlans.DesignPrinciples.ToList(),
            OtherPrinciples = designPlans.DesignPrinciples.ToList(),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HappiDesignPrinciples)]
    [HttpPost("{homeTypeId}/HappiDesignPrinciples")]
    public async Task<IActionResult> HappiDesignPrinciples(
        [FromRoute] string applicationId,
        string homeTypeId,
        HappiDesignPrinciplesModel model,
        CancellationToken cancellationToken)
    {
        var designPrinciples = model.DesignPrinciples ?? Array.Empty<HappiDesignPrincipleType>();
        var otherDesignPrinciples = model.OtherPrinciples ?? Array.Empty<HappiDesignPrincipleType>();

        return await SaveHomeTypeSegment(
            new SaveHappiDesignPrinciplesCommand(applicationId, homeTypeId, designPrinciples.Concat(otherDesignPrinciples).ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpGet("{homeTypeId}/DesignPlans")]
    public async Task<IActionResult> DesignPlans([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var designPlans = await _mediator.Send(new GetDesignPlansQuery(applicationId, homeTypeId), cancellationToken);

        string GetRemoveAction(string fileId) => GetDesignFileAction("Remove", applicationId, homeTypeId, fileId);
        string GetDownloadAction(string fileId) => GetDesignFileAction("Download", applicationId, homeTypeId, fileId);

        return View(new DesignPlansModel(designPlans.ApplicationName, designPlans.HomeTypeName)
        {
            MoreInformation = designPlans.MoreInformation,
            UploadedFiles = designPlans.UploadedFiles
                .Select(x => new FileModel(x.FileId, x.FileName, x.UploadedOn, x.UploadedBy, x.CanBeRemoved, GetRemoveAction(x.FileId), GetDownloadAction(x.FileId)))
                .ToList(),
            MaxFileSizeInMegabytes = _documentSettings.MaxFileSize.Megabytes,
            AllowedExtensions = string.Join(", ", _documentSettings.AllowedExtensions.Select(x => x.Value.ToUpperInvariant())),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpPost("{homeTypeId}/UploadDesignPlansFile")]
    public async Task<IActionResult> UploadDesignPlansFile([FromRoute] string applicationId, string homeTypeId, [FromForm(Name = "File")] IFormFile file, CancellationToken cancellationToken)
    {
        var fileToUpload = new FileToUpload(file.FileName, file.Length, file.OpenReadStream());
        try
        {
            var result = await _mediator.Send(new UploadDesignPlansFileCommand(applicationId, homeTypeId, fileToUpload), cancellationToken);
            return result.HasValidationErrors ? new BadRequestObjectResult(result.Errors) : Ok(UploadedFileModel.FromUploadedFile(result.ReturnedData!));
        }
        finally
        {
            await fileToUpload.Content.DisposeAsync();
        }
    }

    [DisableRequestSizeLimit]
    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpPost("{homeTypeId}/DesignPlans")]
    public async Task<IActionResult> DesignPlans(
        [FromRoute] string applicationId,
        string homeTypeId,
        DesignPlansModel model,
        [FromForm(Name = "File")] List<IFormFile> files,
        CancellationToken cancellationToken)
    {
        var filesToUpload = files.Select(x => new FileToUpload(x.FileName, x.Length, x.OpenReadStream())).ToList();

        try
        {
            return await SaveHomeTypeSegment(new SaveDesignPlansCommand(applicationId, homeTypeId, model.MoreInformation, filesToUpload), model, cancellationToken);
        }
        finally
        {
            foreach (var file in filesToUpload)
            {
                await file.Content.DisposeAsync();
            }
        }
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpGet("{homeTypeId}/RemoveDesignPlansFile")]
    public async Task<IActionResult> RemoveDesignPlansFile(
        [FromRoute] string applicationId,
        string homeTypeId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveDesignPlansFileCommand(applicationId, homeTypeId, fileId), cancellationToken);
        if (result.HasValidationErrors)
        {
            throw new DomainValidationException(result);
        }

        return RedirectToAction("DesignPlans", new { applicationId, homeTypeId });
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpGet("{homeTypeId}/DownloadDesignPlansFile")]
    public async Task<IActionResult> DownloadDesignPlansFile(
        [FromRoute] string applicationId,
        string homeTypeId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var file = await _mediator.Send(new DownloadDesignFileQuery(applicationId, homeTypeId, fileId), cancellationToken);
        return File(file.Content, "application/octet-stream", file.Name);
    }

    [WorkflowState(HomeTypesWorkflowState.SupportedHousingInformation)]
    [HttpGet("{homeTypeId}/SupportedHousingInformation")]
    public async Task<IActionResult> SupportedHousingInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new SupportedHousingInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            LocalCommissioningBodiesConsulted = supportedHousingInformation.LocalCommissioningBodiesConsulted,
            ShortStayAccommodation = supportedHousingInformation.ShortStayAccommodation,
            RevenueFundingType = supportedHousingInformation.RevenueFundingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.SupportedHousingInformation)]
    [HttpPost("{homeTypeId}/SupportedHousingInformation")]
    public async Task<IActionResult> SupportedHousingInformation(
        [FromRoute] string applicationId,
        string homeTypeId,
        SupportedHousingInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveSupportedHousingInformationCommand(
                applicationId,
                homeTypeId,
                model.LocalCommissioningBodiesConsulted,
                model.ShortStayAccommodation,
                model.RevenueFundingType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.RevenueFunding)]
    [HttpGet("{homeTypeId}/RevenueFunding")]
    public async Task<IActionResult> RevenueFunding([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new RevenueFundingModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            Sources = supportedHousingInformation.RevenueFundingSources,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.RevenueFunding)]
    [HttpPost("{homeTypeId}/RevenueFunding")]
    public async Task<IActionResult> RevenueFunding(
        [FromRoute] string applicationId,
        string homeTypeId,
        RevenueFundingModel model,
        CancellationToken cancellationToken)
    {
        var revenueFundingSources = model.Sources ?? Array.Empty<RevenueFundingSourceType>();

        return await SaveHomeTypeSegment(
            new SaveRevenueFundingCommand(applicationId, homeTypeId, revenueFundingSources.ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnAccommodation)]
    [HttpGet("{homeTypeId}/MoveOnAccommodation")]
    public async Task<IActionResult> MoveOnAccommodation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoveOnAccommodationModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            IntendedAsMoveOnAccommodation = homeInformation.IntendedAsMoveOnAccommodation,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnAccommodation)]
    [HttpPost("{homeTypeId}/MoveOnAccommodation")]
    public async Task<IActionResult> MoveOnAccommodation([FromRoute] string applicationId, string homeTypeId, MoveOnAccommodationModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveMoveOnAccommodationCommand(applicationId, homeTypeId, model.IntendedAsMoveOnAccommodation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)]
    [HttpGet("{homeTypeId}/PeopleGroupForSpecificDesignFeatures")]
    public async Task<IActionResult> PeopleGroupForSpecificDesignFeatures([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new PeopleGroupForSpecificDesignFeaturesModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            PeopleGroupForSpecificDesignFeatures = homeInformation.PeopleGroupForSpecificDesignFeatures,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)]
    [HttpPost("{homeTypeId}/PeopleGroupForSpecificDesignFeatures")]
    public async Task<IActionResult> PeopleGroupForSpecificDesignFeatures([FromRoute] string applicationId, string homeTypeId, PeopleGroupForSpecificDesignFeaturesModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SavePeopleGroupForSpecificDesignFeaturesCommand(applicationId, homeTypeId, model.PeopleGroupForSpecificDesignFeatures),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnArrangements)]
    [HttpGet("{homeTypeId}/MoveOnArrangements")]
    public async Task<IActionResult> MoveOnArrangements([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoreInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.MoveOnArrangements,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnArrangements)]
    [HttpPost("{homeTypeId}/MoveOnArrangements")]
    public async Task<IActionResult> MoveOnArrangements(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveMoveOnArrangementsCommand(applicationId, homeTypeId, model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.TypologyLocationAndDesign)]
    [HttpGet("{homeTypeId}/TypologyLocationAndDesign")]
    public async Task<IActionResult> TypologyLocationAndDesign([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoreInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.TypologyLocationAndDesign,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.TypologyLocationAndDesign)]
    [HttpPost("{homeTypeId}/TypologyLocationAndDesign")]
    public async Task<IActionResult> TypologyLocationAndDesign(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveTypologyLocationAndDesignCommand(applicationId, homeTypeId, model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ExitPlan)]
    [HttpGet("{homeTypeId}/ExitPlan")]
    public async Task<IActionResult> ExitPlan([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoreInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.ExitPlan,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.ExitPlan)]
    [HttpPost("{homeTypeId}/ExitPlan")]
    public async Task<IActionResult> ExitPlan(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveExitPlanCommand(applicationId, homeTypeId, model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformation)]
    [HttpGet("{homeTypeId}/BuildingInformation")]
    public async Task<IActionResult> BuildingInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new BuildingInformationModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            BuildingType = homeInformation.BuildingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformation)]
    [HttpPost("{homeTypeId}/BuildingInformation")]
    public async Task<IActionResult> BuildingInformation(
        [FromRoute] string applicationId,
        string homeTypeId,
        BuildingInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveBuildingInformationCommand(applicationId, homeTypeId, model.BuildingType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformationIneligible)]
    [HttpGet("{homeTypeId}/BuildingInformationIneligible")]
    public async Task<IActionResult> BuildingInformationIneligible([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new HomeTypeBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName));
    }

    [WorkflowState(HomeTypesWorkflowState.CustomBuildProperty)]
    [HttpGet("{homeTypeId}/CustomBuildProperty")]
    public async Task<IActionResult> CustomBuildProperty([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new CustomBuildPropertyModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            CustomBuild = homeInformation.CustomBuild,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.CustomBuildProperty)]
    [HttpPost("{homeTypeId}/CustomBuildProperty")]
    public async Task<IActionResult> CustomBuildProperty(
        [FromRoute] string applicationId,
        string homeTypeId,
        CustomBuildPropertyModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveCustomBuildPropertyCommand(applicationId, homeTypeId, model.CustomBuild),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.TypeOfFacilities)]
    [HttpGet("{homeTypeId}/TypeOfFacilities")]
    public async Task<IActionResult> TypeOfFacilities([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new TypeBasicOfFacilitiesModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            FacilityType = homeInformation.FacilityType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.TypeOfFacilities)]
    [HttpPost("{homeTypeId}/TypeOfFacilities")]
    public async Task<IActionResult> TypeOfFacilities(
        [FromRoute] string applicationId,
        string homeTypeId,
        TypeBasicOfFacilitiesModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveFacilityTypeCommand(applicationId, homeTypeId, model.FacilityType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityStandards)]
    [HttpGet("{homeTypeId}/AccessibilityStandards")]
    public async Task<IActionResult> AccessibilityStandards([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new AccessibilityModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            AccessibilityStandards = homeInformation.AccessibilityStandards,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityStandards)]
    [HttpPost("{homeTypeId}/AccessibilityStandards")]
    public async Task<IActionResult> AccessibilityStandards(
        [FromRoute] string applicationId,
        string homeTypeId,
        AccessibilityModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveAccessibilityStandardsCommand(applicationId, homeTypeId, model.AccessibilityStandards),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityCategory)]
    [HttpGet("{homeTypeId}/AccessibilityCategory")]
    public async Task<IActionResult> AccessibilityCategory([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new AccessibilityModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            AccessibilityCategory = homeInformation.AccessibilityCategory,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityCategory)]
    [HttpPost("{homeTypeId}/AccessibilityCategory")]
    public async Task<IActionResult> AccessibilityCategory(
        [FromRoute] string applicationId,
        string homeTypeId,
        AccessibilityModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveAccessibilityCategoryCommand(applicationId, homeTypeId, model.AccessibilityCategory),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AffordableRent)]
    [HttpGet("{homeTypeId}/AffordableRent")]
    public async Task<IActionResult> AffordableRent([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(applicationId, homeTypeId), cancellationToken);
        var model = new AffordableRentModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            HomeMarketValue = tenureDetails.HomeMarketValue?.ToString(CultureInfo.InvariantCulture),
            HomeWeeklyRent = tenureDetails.HomeWeeklyRent?.ToString("0.##", CultureInfo.InvariantCulture),
            AffordableWeeklyRent = tenureDetails.AffordableWeeklyRent?.ToString("0.##", CultureInfo.InvariantCulture),
            AffordableRentAsPercentageOfMarketRent = tenureDetails.CalculatedPercentage?.ToString("00.00", CultureInfo.InvariantCulture),
            TargetRentExceedMarketRent = tenureDetails.TargetRentExceedMarketRent,
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.AffordableRent)]
    [HttpPost("{homeTypeId}/AffordableRent")]
    public async Task<IActionResult> AffordableRent(
        [FromRoute] string applicationId,
        string homeTypeId,
        AffordableRentModel model,
        string action,
        CancellationToken cancellationToken)
    {
        model.AffordableRentAsPercentageOfMarketRent =
            CalculationUtilities.CalculateAffordableRent(model.HomeWeeklyRent, model.AffordableWeeklyRent).ToString(CultureInfo.InvariantCulture);

        if (action == CommonStrings.Calculate)
        {
            var operationResult = await _mediator.Send(
                new CalculateAffordableRentQuery(
                    applicationId,
                    homeTypeId,
                    model.HomeMarketValue,
                    model.HomeWeeklyRent,
                    model.AffordableWeeklyRent,
                    model.TargetRentExceedMarketRent),
                cancellationToken);
            ModelState.AddValidationErrors(operationResult);

            return View(model);
        }

        return await SaveHomeTypeSegment(
            new SaveAffordableRentCommand(
                applicationId,
                homeTypeId,
                model.HomeMarketValue,
                model.HomeWeeklyRent,
                model.AffordableWeeklyRent,
                model.TargetRentExceedMarketRent),
            model,
            cancellationToken);
    }

    protected override async Task<IStateRouting<HomeTypesWorkflowState>> Routing(HomeTypesWorkflowState currentState, object? routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                                ?? routeData?.GetPropertyValue<string>("applicationId")
                                ?? throw new NotFoundException($"{nameof(HomeTypesController)} required applicationId path parameter.");
        if (string.IsNullOrEmpty(applicationId))
        {
            return new HomeTypesWorkflow();
        }

        var homeTypeId = Request.GetRouteValue("homeTypeId") ?? routeData?.GetPropertyValue<string>("homeTypeId");
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypesWorkflow(currentState, null);
        }

        var homeType = await _mediator.Send(new GetHomeTypeQuery(applicationId, homeTypeId));
        return new HomeTypesWorkflow(currentState, homeType);
    }

    private static HousingType GetDefaultHousingType(Tenure applicationTenure)
    {
        return applicationTenure switch
        {
            Tenure.OlderPersonsSharedOwnership => HousingType.HomesForOlderPeople,
            Tenure.HomeOwnershipLongTermDisabilities => HousingType.HomesForDisabledAndVulnerablePeople,
            _ => HousingType.Undefined,
        };
    }

    private async Task<IActionResult> SaveHomeTypeDetails(
        string applicationId,
        string? homeTypeId,
        HomeTypeDetailsModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SaveHomeTypeDetailsCommand(applicationId, homeTypeId, model.HomeTypeName, model.HousingType),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("HomeTypeDetails", model);
        }

        return await Continue(new { applicationId, homeTypeId = result.ReturnedData!.Value });
    }

    private async Task<IActionResult> SaveHomeTypeSegment<TModel, TSaveSegmentCommand>(
        TSaveSegmentCommand command,
        TModel model,
        CancellationToken cancellationToken)
        where TSaveSegmentCommand : SaveHomeTypeSegmentCommandBase
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationId = command.ApplicationId, homeTypeId = command.HomeTypeId });
    }

    private string GetDesignFileAction(string actionName, string applicationId, string homeTypeId, string fileId)
    {
        return Url.RouteUrl(
            "subSection",
            new
            {
                controller = "HomeTypes",
                action = $"{actionName}DesignPlansFile",
                applicationId,
                id = homeTypeId,
                fileId,
            }) ?? string.Empty;
    }
}
