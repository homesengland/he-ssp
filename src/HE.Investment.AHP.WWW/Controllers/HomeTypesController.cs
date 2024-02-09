using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Common;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investment.AHP.WWW.Models.HomeTypes.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/home-types")]
public class HomeTypesController : WorkflowController<HomeTypesWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IAhpDocumentSettings _documentSettings;

    private readonly IHomeTypeSummaryViewModelFactory _summaryViewModelFactory;

    private readonly IAccountAccessContext _accountAccessContext;

    public HomeTypesController(
        IMediator mediator,
        IAhpDocumentSettings documentSettings,
        IHomeTypeSummaryViewModelFactory summaryViewModelFactory,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _documentSettings = documentSettings;
        _summaryViewModelFactory = summaryViewModelFactory;
        _accountAccessContext = accountAccessContext;
    }

    [WorkflowState(HomeTypesWorkflowState.Index)]
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        return View(new HomeTypeModelBase(application.Name));
    }

    [HttpGet("back")]
    public Task<IActionResult> Back([FromRoute] string applicationId, string homeTypeId, HomeTypesWorkflowState currentPage)
    {
        return Back(currentPage, new { applicationId, homeTypeId });
    }

    [WorkflowState(HomeTypesWorkflowState.List)]
    [HttpGet("list")]
    public async Task<IActionResult> List([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        var isEditable = await _accountAccessContext.CanEditApplication() && !homeTypes.IsReadOnly;

        return View(new HomeTypeListModel(homeTypes.ApplicationName)
        {
            HomeTypes = homeTypes.HomeTypes.Select(x => new HomeTypeItemModel(x.Id.Value, x.Name, x.HousingType, x.NumberOfHomes)).ToList(),
            IsEditable = isEditable,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.List)]
    [HttpPost("list")]
    public async Task<IActionResult> List([FromRoute] string applicationId, HomeTypeListModel model, string action, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SaveFinishHomeTypesAnswerCommand(AhpApplicationId.From(applicationId), FinishHomeTypesAnswer.Yes, true), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await this.ReturnToTaskListOrContinue(
            async () => await Continue(new { applicationId }));
    }

    [WorkflowState(HomeTypesWorkflowState.FinishHomeTypes)]
    [HttpGet("finish")]
    public async Task<IActionResult> FinishHomeTypes([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var homeTypesAnswer = await _mediator.Send(new GetFinishHomesTypeAnswerQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View(new FinishHomeTypeModel(homeTypesAnswer.ApplicationName) { FinishAnswer = homeTypesAnswer.Answer });
    }

    [WorkflowState(HomeTypesWorkflowState.FinishHomeTypes)]
    [HttpPost("finish")]
    public async Task<IActionResult> FinishHomeTypes(
        [FromRoute] string applicationId,
        FinishHomeTypeModel model,
        string action,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SaveFinishHomeTypesAnswerCommand(AhpApplicationId.From(applicationId), model.FinishAnswer), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await this.ReturnToTaskListOrContinue(
            async () => model.FinishAnswer == FinishHomeTypesAnswer.Yes
                ? RedirectToAction("TaskList", "Application", new { applicationId })
                : await Back(new { applicationId }));
    }

    [HttpGet("{homeTypeId}/duplicate")]
    public async Task<IActionResult> Duplicate([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DuplicateHomeTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        return RedirectToAction("List", new { applicationId });
    }

    [WorkflowState(HomeTypesWorkflowState.RemoveHomeType)]
    [HttpGet("{homeTypeId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View("RemoveHomeTypeConfirmation", new RemoveHomeTypeModel(homeTypeDetails.ApplicationName, homeTypeDetails.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.RemoveHomeType)]
    [HttpPost("{homeTypeId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string homeTypeId, RemoveHomeTypeModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveHomeTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.RemoveHomeTypeAnswer), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("RemoveHomeTypeConfirmation", model);
        }

        return RedirectToAction("List", new { applicationId });
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpGet("details")]
    public async Task<IActionResult> NewHomeTypeDetails([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        return View("HomeTypeDetails", new HomeTypeDetailsModel(application.Name) { HousingType = GetDefaultHousingType(application.Tenure) });
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpPost("details")]
    public async Task<IActionResult> NewHomeTypeDetails(
        [FromRoute] string applicationId,
        HomeTypeDetailsModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeDetails(AhpApplicationId.From(applicationId), null, model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeTypeDetails)]
    [HttpGet("{homeTypeId}/details")]
    public async Task<IActionResult> HomeTypeDetails(
        [FromRoute] string applicationId,
        string homeTypeId,
        [FromQuery] bool redirect,
        CancellationToken cancellationToken)
    {
        var homeType = await _mediator.Send(new GetFullHomeTypeQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var isReadOnly = !await _accountAccessContext.CanEditApplication() || homeType.IsReadOnly;
        if (isReadOnly)
        {
            return RedirectToAction("CheckAnswers", new { applicationId, homeTypeId });
        }

        if (redirect)
        {
            var firstNotAnsweredQuestion = _summaryViewModelFactory
                .CreateSummaryModel(homeType, Url, isReadOnly)
                .Where(x => x.Items != null)
                .SelectMany(x => x.Items!)
                .FirstOrDefault(x => x is { HasAnswer: false, HasRedirectAction: true });
            return firstNotAnsweredQuestion != null
                ? Redirect(firstNotAnsweredQuestion.ActionUrl!)
                : RedirectToAction("CheckAnswers", new { applicationId, homeTypeId });
        }

        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        return View(new HomeTypeDetailsModel(homeTypeDetails.ApplicationName)
        {
            HomeTypeName = homeTypeDetails.Name,
            HousingType = homeTypeDetails.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeTypeDetails)]
    [HttpPost("{homeTypeId}/details")]
    public async Task<IActionResult> HomeTypeDetails(
        [FromRoute] string applicationId,
        [FromRoute] string homeTypeId,
        HomeTypeDetailsModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeDetails(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpGet("{homeTypeId}/home-information")]
    public async Task<IActionResult> HomeInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new HomeInformationModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            NumberOfHomes = homeInformation.NumberOfHomes?.ToString(CultureInfo.InvariantCulture),
            NumberOfBedrooms = homeInformation.NumberOfBedrooms?.ToString(CultureInfo.InvariantCulture),
            MaximumOccupancy = homeInformation.MaximumOccupancy?.ToString(CultureInfo.InvariantCulture),
            NumberOfStoreys = homeInformation.NumberOfStoreys?.ToString(CultureInfo.InvariantCulture),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpPost("{homeTypeId}/home-information")]
    public async Task<IActionResult> HomeInformation(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomeInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveHomeInformationCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.NumberOfHomes,
                model.NumberOfBedrooms,
                model.MaximumOccupancy,
                model.NumberOfStoreys),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpGet("{homeTypeId}/homes-for-disabled-people")]
    public async Task<IActionResult> HomesForDisabledPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var disabledPeopleHomeType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        return View(new HomesForDisabledPeopleModel(disabledPeopleHomeType.ApplicationName, disabledPeopleHomeType.HomeTypeName)
        {
            HousingType = disabledPeopleHomeType.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpPost("{homeTypeId}/homes-for-disabled-people")]
    public async Task<IActionResult> HomesForDisabledPeople(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomesForDisabledPeopleModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveDisabledPeopleHousingTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.HousingType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleClientGroup)]
    [HttpGet("{homeTypeId}/disabled-people-client-group")]
    public async Task<IActionResult> DisabledPeopleClientGroup([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var disabledPeopleHomeType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        return View(new DisabledPeopleClientGroupModel(disabledPeopleHomeType.ApplicationName, disabledPeopleHomeType.HomeTypeName)
        {
            DisabledPeopleClientGroup = disabledPeopleHomeType.ClientGroupType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleClientGroup)]
    [HttpPost("{homeTypeId}/disabled-people-client-group")]
    public async Task<IActionResult> DisabledPeopleClientGroup(
        [FromRoute] string applicationId,
        string homeTypeId,
        DisabledPeopleClientGroupModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveDisabledPeopleClientGroupTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.DisabledPeopleClientGroup),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpGet("{homeTypeId}/homes-for-older-people")]
    public async Task<IActionResult> HomesForOlderPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var olderPeopleHomeType = await _mediator.Send(new GetOlderPeopleHomeTypeDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        return View(new HomesForOlderPeopleModel(olderPeopleHomeType.ApplicationName, olderPeopleHomeType.HomeTypeName)
        {
            HousingType = olderPeopleHomeType.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpPost("{homeTypeId}/homes-for-older-people")]
    public async Task<IActionResult> HomesForOlderPeople(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomesForOlderPeopleModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(new SaveOlderPeopleHousingTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.HousingType), model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HappiDesignPrinciples)]
    [HttpGet("{homeTypeId}/happi-design-principles")]
    public async Task<IActionResult> HappiDesignPrinciples([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var designPlans = await _mediator.Send(new GetDesignPlansQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        return View(new HappiDesignPrinciplesModel(designPlans.ApplicationName, designPlans.HomeTypeName)
        {
            DesignPrinciples = designPlans.DesignPrinciples.ToList(),
            OtherPrinciples = designPlans.DesignPrinciples.ToList(),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HappiDesignPrinciples)]
    [HttpPost("{homeTypeId}/happi-design-principles")]
    public async Task<IActionResult> HappiDesignPrinciples(
        [FromRoute] string applicationId,
        string homeTypeId,
        HappiDesignPrinciplesModel model,
        CancellationToken cancellationToken)
    {
        var designPrinciples = model.DesignPrinciples ?? Array.Empty<HappiDesignPrincipleType>();
        var otherDesignPrinciples = model.OtherPrinciples ?? Array.Empty<HappiDesignPrincipleType>();

        return await SaveHomeTypeSegment(
            new SaveHappiDesignPrinciplesCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), designPrinciples.Concat(otherDesignPrinciples).ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpPost("{homeTypeId}/upload-design-plans-file")]
    public async Task<IActionResult> UploadDesignPlansFile(
        [FromRoute] string applicationId,
        string homeTypeId,
        [FromForm(Name = "File")] IFormFile file,
        CancellationToken cancellationToken)
    {
        var fileToUpload = new FileToUpload(file.FileName, file.Length, file.OpenReadStream());
        try
        {
            var result = await _mediator.Send(new UploadDesignPlansFileCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), fileToUpload), cancellationToken);
            return result.HasValidationErrors ? new BadRequestObjectResult(result.Errors) : Ok(UploadedFileModel.FromUploadedFile(result.ReturnedData!));
        }
        finally
        {
            await fileToUpload.Content.DisposeAsync();
        }
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpGet("{homeTypeId}/design-plans")]
    public async Task<IActionResult> DesignPlans([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var designPlans = await _mediator.Send(new GetDesignPlansQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        string GetRemoveAction(FileId fileId) => GetDesignFileAction("Remove", applicationId, homeTypeId, fileId);
        string GetDownloadAction(FileId fileId) => GetDesignFileAction("Download", applicationId, homeTypeId, fileId);

        return View(new DesignPlansModel(designPlans.ApplicationName, designPlans.HomeTypeName)
        {
            MoreInformation = designPlans.MoreInformation,
            UploadedFiles = designPlans.UploadedFiles
                .Select(x => new FileModel(
                    x.FileId.Value,
                    x.FileName,
                    x.UploadedOn,
                    x.UploadedBy,
                    x.CanBeRemoved,
                    GetRemoveAction(x.FileId),
                    GetDownloadAction(x.FileId)))
                .ToList(),
            MaxFileSizeInMegabytes = _documentSettings.MaxFileSize.Megabytes,
            AllowedExtensions = string.Join(", ", _documentSettings.AllowedExtensions.Select(x => x.Value.ToUpperInvariant())),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpPost("{homeTypeId}/design-plans")]
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
            return await SaveHomeTypeSegment(
                new SaveDesignPlansCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.MoreInformation, filesToUpload),
                model,
                cancellationToken);
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
    [HttpGet("{homeTypeId}/remove-design-plans-file")]
    public async Task<IActionResult> RemoveDesignPlansFile(
        [FromRoute] string applicationId,
        string homeTypeId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RemoveDesignPlansFileCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), FileId.From(fileId)),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            throw new DomainValidationException(result);
        }

        TryGetWorkflowQueryParameter(out var workflow);
        return RedirectToAction("DesignPlans", new { applicationId, homeTypeId, workflow });
    }

    [HttpGet("{homeTypeId}/download-design-plans-file")]
    public async Task<IActionResult> DownloadDesignPlansFile(
        [FromRoute] string applicationId,
        string homeTypeId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var file = await _mediator.Send(new DownloadDesignFileQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), FileId.From(fileId)), cancellationToken);
        return File(file.Content, "application/octet-stream", file.Name);
    }

    [WorkflowState(HomeTypesWorkflowState.SupportedHousingInformation)]
    [HttpGet("{homeTypeId}/supported-housing-information")]
    public async Task<IActionResult> SupportedHousingInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new SupportedHousingInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            LocalCommissioningBodiesConsulted = supportedHousingInformation.LocalCommissioningBodiesConsulted,
            ShortStayAccommodation = supportedHousingInformation.ShortStayAccommodation,
            RevenueFundingType = supportedHousingInformation.RevenueFundingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.SupportedHousingInformation)]
    [HttpPost("{homeTypeId}/supported-housing-information")]
    public async Task<IActionResult> SupportedHousingInformation(
        [FromRoute] string applicationId,
        string homeTypeId,
        SupportedHousingInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveSupportedHousingInformationCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.LocalCommissioningBodiesConsulted,
                model.ShortStayAccommodation,
                model.RevenueFundingType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.RevenueFunding)]
    [HttpGet("{homeTypeId}/revenue-funding")]
    public async Task<IActionResult> RevenueFunding([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new RevenueFundingModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            Sources = supportedHousingInformation.RevenueFundingSources,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.RevenueFunding)]
    [HttpPost("{homeTypeId}/revenue-funding")]
    public async Task<IActionResult> RevenueFunding(
        [FromRoute] string applicationId,
        string homeTypeId,
        RevenueFundingModel model,
        CancellationToken cancellationToken)
    {
        var revenueFundingSources = model.Sources ?? Array.Empty<RevenueFundingSourceType>();

        return await SaveHomeTypeSegment(
            new SaveRevenueFundingCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), revenueFundingSources.ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnAccommodation)]
    [HttpGet("{homeTypeId}/move-on-accommodation")]
    public async Task<IActionResult> MoveOnAccommodation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new MoveOnAccommodationModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            IntendedAsMoveOnAccommodation = homeInformation.IntendedAsMoveOnAccommodation,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnAccommodation)]
    [HttpPost("{homeTypeId}/move-on-accommodation")]
    public async Task<IActionResult> MoveOnAccommodation(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoveOnAccommodationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveMoveOnAccommodationCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.IntendedAsMoveOnAccommodation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)]
    [HttpGet("{homeTypeId}/people-group-for-specific-design-features")]
    public async Task<IActionResult> PeopleGroupForSpecificDesignFeatures(
        [FromRoute] string applicationId,
        string homeTypeId,
        CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new PeopleGroupForSpecificDesignFeaturesModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            PeopleGroupForSpecificDesignFeatures = homeInformation.PeopleGroupForSpecificDesignFeatures,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)]
    [HttpPost("{homeTypeId}/people-group-for-specific-design-features")]
    public async Task<IActionResult> PeopleGroupForSpecificDesignFeatures(
        [FromRoute] string applicationId,
        string homeTypeId,
        PeopleGroupForSpecificDesignFeaturesModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SavePeopleGroupForSpecificDesignFeaturesCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.PeopleGroupForSpecificDesignFeatures),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnArrangements)]
    [HttpGet("{homeTypeId}/move-on-arrangements")]
    public async Task<IActionResult> MoveOnArrangements([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new MoreInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.MoveOnArrangements,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnArrangements)]
    [HttpPost("{homeTypeId}/move-on-arrangements")]
    public async Task<IActionResult> MoveOnArrangements(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveMoveOnArrangementsCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.TypologyLocationAndDesign)]
    [HttpGet("{homeTypeId}/typology-location-and-design")]
    public async Task<IActionResult> TypologyLocationAndDesign([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new MoreInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.TypologyLocationAndDesign,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.TypologyLocationAndDesign)]
    [HttpPost("{homeTypeId}/typology-location-and-design")]
    public async Task<IActionResult> TypologyLocationAndDesign(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveTypologyLocationAndDesignCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ExitPlan)]
    [HttpGet("{homeTypeId}/exit-plan")]
    public async Task<IActionResult> ExitPlan([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new MoreInformationModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.ExitPlan,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.ExitPlan)]
    [HttpPost("{homeTypeId}/exit-plan")]
    public async Task<IActionResult> ExitPlan(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveExitPlanCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformation)]
    [HttpGet("{homeTypeId}/building-information")]
    public async Task<IActionResult> BuildingInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(
            new BuildingInformationModel(homeInformation.ApplicationName, homeInformation.HomeTypeName) { BuildingType = homeInformation.BuildingType, });
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformation)]
    [HttpPost("{homeTypeId}/building-information")]
    public async Task<IActionResult> BuildingInformation(
        [FromRoute] string applicationId,
        string homeTypeId,
        BuildingInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveBuildingInformationCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.BuildingType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformationIneligible)]
    [HttpGet("{homeTypeId}/building-information-ineligible")]
    public async Task<IActionResult> BuildingInformationIneligible([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new HomeTypeBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName));
    }

    [WorkflowState(HomeTypesWorkflowState.CustomBuildProperty)]
    [HttpGet("{homeTypeId}/custom-build-property")]
    public async Task<IActionResult> CustomBuildProperty([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new CustomBuildPropertyModel(homeInformation.ApplicationName, homeInformation.HomeTypeName) { CustomBuild = homeInformation.CustomBuild, });
    }

    [WorkflowState(HomeTypesWorkflowState.CustomBuildProperty)]
    [HttpPost("{homeTypeId}/custom-build-property")]
    public async Task<IActionResult> CustomBuildProperty(
        [FromRoute] string applicationId,
        string homeTypeId,
        CustomBuildPropertyModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveCustomBuildPropertyCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.CustomBuild),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.TypeOfFacilities)]
    [HttpGet("{homeTypeId}/type-of-facilities")]
    public async Task<IActionResult> TypeOfFacilities([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new TypeBasicOfFacilitiesModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            FacilityType = homeInformation.FacilityType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.TypeOfFacilities)]
    [HttpPost("{homeTypeId}/type-of-facilities")]
    public async Task<IActionResult> TypeOfFacilities(
        [FromRoute] string applicationId,
        string homeTypeId,
        TypeBasicOfFacilitiesModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveFacilityTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.FacilityType),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityStandards)]
    [HttpGet("{homeTypeId}/accessibility-standards")]
    public async Task<IActionResult> AccessibilityStandards([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new AccessibilityModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            AccessibilityStandards = homeInformation.AccessibilityStandards,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityStandards)]
    [HttpPost("{homeTypeId}/accessibility-standards")]
    public async Task<IActionResult> AccessibilityStandards(
        [FromRoute] string applicationId,
        string homeTypeId,
        AccessibilityModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveAccessibilityStandardsCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.AccessibilityStandards),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityCategory)]
    [HttpGet("{homeTypeId}/accessibility-category")]
    public async Task<IActionResult> AccessibilityCategory([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new AccessibilityModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            AccessibilityCategory = homeInformation.AccessibilityCategory,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.AccessibilityCategory)]
    [HttpPost("{homeTypeId}/accessibility-category")]
    public async Task<IActionResult> AccessibilityCategory(
        [FromRoute] string applicationId,
        string homeTypeId,
        AccessibilityModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveAccessibilityCategoryCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.AccessibilityCategory),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.FloorArea)]
    [HttpGet("{homeTypeId}/floor-area")]
    public async Task<IActionResult> FloorArea([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new FloorAreaModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            FloorArea = homeInformation.InternalFloorArea?.ToString("0.##", CultureInfo.InvariantCulture),
            MeetNationallyDescribedSpaceStandards = homeInformation.MeetNationallyDescribedSpaceStandards,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.FloorArea)]
    [HttpPost("{homeTypeId}/floor-area")]
    public async Task<IActionResult> FloorArea(
        [FromRoute] string applicationId,
        string homeTypeId,
        FloorAreaModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveFloorAreaCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.FloorArea, model.MeetNationallyDescribedSpaceStandards),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.FloorAreaStandards)]
    [HttpGet("{homeTypeId}/floor-area-standards")]
    public async Task<IActionResult> FloorAreaStandards([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new FloorAreaModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            NationallyDescribedSpaceStandards = homeInformation.NationallyDescribedSpaceStandards.ToList(),
            OtherNationallyDescribedSpaceStandards = homeInformation.NationallyDescribedSpaceStandards.ToList(),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.FloorAreaStandards)]
    [HttpPost("{homeTypeId}/floor-area-standards")]
    public async Task<IActionResult> FloorAreaStandards(
        [FromRoute] string applicationId,
        string homeTypeId,
        FloorAreaModel model,
        CancellationToken cancellationToken)
    {
        var nationallyDescribedSpaceStandards = model.NationallyDescribedSpaceStandards
                                                ?? Array.Empty<NationallyDescribedSpaceStandardType>();
        var otherNationallyDescribedSpaceStandards = model.OtherNationallyDescribedSpaceStandards
                                                     ?? Array.Empty<NationallyDescribedSpaceStandardType>();

        return await SaveHomeTypeSegment(
            new SaveFloorAreaStandardsCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                nationallyDescribedSpaceStandards.Concat(otherNationallyDescribedSpaceStandards).ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AffordableRent)]
    [HttpGet("{homeTypeId}/affordable-rent")]
    public async Task<IActionResult> AffordableRent([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new AffordableRentModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MarketValue = CurrencyHelper.InputPounds(tenureDetails.MarketValue),
            MarketRent = CurrencyHelper.InputPoundsPences(tenureDetails.MarketRent),
            ProspectiveRent = CurrencyHelper.InputPoundsPences(tenureDetails.ProspectiveRent),
            ProspectiveRentAsPercentageOfMarketRent = tenureDetails.ProspectiveRentAsPercentageOfMarketRent?.ToString("0", CultureInfo.InvariantCulture),
            TargetRentExceedMarketRent = tenureDetails.TargetRentExceedMarketRent,
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.AffordableRent)]
    [HttpPost("{homeTypeId}/affordable-rent")]
    public async Task<IActionResult> AffordableRent(
        [FromRoute] string applicationId,
        string homeTypeId,
        AffordableRentModel model,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateProspectiveRentQuery(
                    AhpApplicationId.From(applicationId),
                    HomeTypeId.From(homeTypeId),
                    model.MarketValue,
                    model.MarketRent,
                    model.ProspectiveRent,
                    model.TargetRentExceedMarketRent),
                cancellationToken);
            ModelState.AddValidationErrors(operationResult);
            model.ProspectiveRentAsPercentageOfMarketRent = calculationResult.ProspectiveRentPercentage?.ToString("0", CultureInfo.InvariantCulture);
            return View(model);
        }

        return await SaveHomeTypeSegment(
            new SaveProspectiveRentCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.MarketValue,
                model.MarketRent,
                model.ProspectiveRent,
                model.TargetRentExceedMarketRent),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.AffordableRentIneligible)]
    [HttpGet("{homeTypeId}/affordable-rent-ineligible")]
    public async Task<IActionResult> AffordableRentIneligible([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new HomeTypeBasicModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName));
    }

    [WorkflowState(HomeTypesWorkflowState.SocialRent)]
    [HttpGet("{homeTypeId}/social-rent")]
    public async Task<IActionResult> SocialRent([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new SocialRentModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MarketValue = CurrencyHelper.InputPounds(tenureDetails.MarketValue),
            ProspectiveRent = CurrencyHelper.InputPoundsPences(tenureDetails.ProspectiveRent),
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.SocialRent)]
    [HttpPost("{homeTypeId}/social-rent")]
    public async Task<IActionResult> SocialRent(
        [FromRoute] string applicationId,
        string homeTypeId,
        SocialRentModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveSocialRentCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.MarketValue,
                model.ProspectiveRent),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.SharedOwnership)]
    [HttpGet("{homeTypeId}/shared-ownership")]
    public async Task<IActionResult> SharedOwnership([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new SharedOwnershipModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MarketValue = CurrencyHelper.InputPounds(tenureDetails.MarketValue),
            InitialSale = tenureDetails.InitialSale?.ToString(CultureInfo.InvariantCulture),
            ExpectedFirstTranche = CurrencyHelper.DisplayPoundsPences(tenureDetails.ExpectedFirstTranche),
            ProspectiveRent = CurrencyHelper.InputPoundsPences(tenureDetails.ProspectiveRent),
            RentAsPercentageOfTheUnsoldShare =
                tenureDetails.RentAsPercentageOfTheUnsoldShare?.ToString("0.##", CultureInfo.InvariantCulture),
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.SharedOwnership)]
    [HttpPost("{homeTypeId}/shared-ownership")]
    public async Task<IActionResult> SharedOwnership(
        [FromRoute] string applicationId,
        string homeTypeId,
        SharedOwnershipModel model,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateSharedOwnershipQuery(
                    AhpApplicationId.From(applicationId),
                    HomeTypeId.From(homeTypeId),
                    model.MarketValue,
                    model.InitialSale,
                    model.ProspectiveRent),
                cancellationToken);

            model.ExpectedFirstTranche = CurrencyHelper.DisplayPoundsPences(calculationResult.ExpectedFirstTranche);
            model.RentAsPercentageOfTheUnsoldShare = calculationResult.ProspectiveRentPercentage?.ToString("0.##", CultureInfo.InvariantCulture);

            ModelState.AddValidationErrors(operationResult);

            return View(model);
        }

        return await SaveHomeTypeSegment(
            new SaveSharedOwnershipCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.MarketValue,
                model.InitialSale,
                model.ProspectiveRent),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ProspectiveRentIneligible)]
    [HttpGet("{homeTypeId}/prospective-rent-ineligible")]
    public async Task<IActionResult> ProspectiveRentIneligible([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new ProspectiveRentIneligibleModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName));
    }

    [WorkflowState(HomeTypesWorkflowState.RentToBuy)]
    [HttpGet("{homeTypeId}/rent-to-buy")]
    public async Task<IActionResult> RentToBuy([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new RentToBuyModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MarketValue = CurrencyHelper.InputPounds(tenureDetails.MarketValue),
            MarketRent = CurrencyHelper.InputPoundsPences(tenureDetails.MarketRent),
            ProspectiveRent = CurrencyHelper.InputPoundsPences(tenureDetails.ProspectiveRent),
            ProspectiveRentAsPercentageOfMarketRent = tenureDetails.ProspectiveRentAsPercentageOfMarketRent?.ToString("00.00", CultureInfo.InvariantCulture),
            TargetRentExceedMarketRent = tenureDetails.TargetRentExceedMarketRent,
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.RentToBuy)]
    [HttpPost("{homeTypeId}/rent-to-buy")]
    public async Task<IActionResult> RentToBuy(
        [FromRoute] string applicationId,
        string homeTypeId,
        RentToBuyModel model,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateProspectiveRentQuery(
                    AhpApplicationId.From(applicationId),
                    HomeTypeId.From(homeTypeId),
                    model.MarketValue,
                    model.MarketRent,
                    model.ProspectiveRent,
                    model.TargetRentExceedMarketRent),
                cancellationToken);
            ModelState.AddValidationErrors(operationResult);
            model.ProspectiveRentAsPercentageOfMarketRent = calculationResult.ProspectiveRentPercentage?.ToString("0", CultureInfo.InvariantCulture);

            return View(model);
        }

        return await SaveHomeTypeSegment(
            new SaveProspectiveRentCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.MarketValue,
                model.MarketRent,
                model.ProspectiveRent,
                model.TargetRentExceedMarketRent),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.RentToBuyIneligible)]
    [HttpGet("{homeTypeId}/rent-to-buy-ineligible")]
    public async Task<IActionResult> RentToBuyIneligible([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        return await AffordableRentIneligible(applicationId, homeTypeId, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    [HttpGet("{homeTypeId}/home-ownership-disabilities")]
    public async Task<IActionResult> HomeOwnershipDisabilities([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new HomeOwnershipDisabilitiesModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MarketValue = CurrencyHelper.InputPounds(tenureDetails.MarketValue),
            InitialSale = tenureDetails.InitialSale?.ToString(CultureInfo.InvariantCulture),
            ExpectedFirstTranche = CurrencyHelper.DisplayPoundsPences(tenureDetails.ExpectedFirstTranche),
            ProspectiveRent = CurrencyHelper.InputPoundsPences(tenureDetails.ProspectiveRent),
            RentAsPercentageOfTheUnsoldShare =
                tenureDetails.RentAsPercentageOfTheUnsoldShare?.ToString("0.##", CultureInfo.InvariantCulture),
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    [HttpPost("{homeTypeId}/home-ownership-disabilities")]
    public async Task<IActionResult> HomeOwnershipDisabilities(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomeOwnershipDisabilitiesModel model,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateHomeOwnershipDisabilitiesQuery(
                    AhpApplicationId.From(applicationId),
                    HomeTypeId.From(homeTypeId),
                    model.MarketValue,
                    model.InitialSale,
                    model.ProspectiveRent),
                cancellationToken);

            model.ExpectedFirstTranche = CurrencyHelper.DisplayPoundsPences(calculationResult.ExpectedFirstTranche);
            model.RentAsPercentageOfTheUnsoldShare = calculationResult.ProspectiveRentPercentage?.ToString("0.##", CultureInfo.InvariantCulture);

            ModelState.AddValidationErrors(operationResult);

            return View(model);
        }

        return await SaveHomeTypeSegment(
            new SaveHomeOwnershipDisabilitiesCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.MarketValue,
                model.InitialSale,
                model.ProspectiveRent),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.OlderPersonsSharedOwnership)]
    [HttpGet("{homeTypeId}/older-persons-shared-ownership")]
    public async Task<IActionResult> OlderPersonsSharedOwnership([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new OlderPersonsSharedOwnershipModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MarketValue = CurrencyHelper.InputPounds(tenureDetails.MarketValue),
            InitialSale = tenureDetails.InitialSale?.ToString(CultureInfo.InvariantCulture),
            ExpectedFirstTranche = CurrencyHelper.DisplayPoundsPences(tenureDetails.ExpectedFirstTranche),
            ProspectiveRent = CurrencyHelper.InputPoundsPences(tenureDetails.ProspectiveRent),
            RentAsPercentageOfTheUnsoldShare =
                tenureDetails.RentAsPercentageOfTheUnsoldShare?.ToString("0.##", CultureInfo.InvariantCulture),
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.OlderPersonsSharedOwnership)]
    [HttpPost("{homeTypeId}/older-persons-shared-ownership")]
    public async Task<IActionResult> OlderPersonsSharedOwnership(
        [FromRoute] string applicationId,
        string homeTypeId,
        OlderPersonsSharedOwnershipModel model,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateOlderPersonsSharedOwnershipQuery(
                    AhpApplicationId.From(applicationId),
                    HomeTypeId.From(homeTypeId),
                    model.MarketValue,
                    model.InitialSale,
                    model.ProspectiveRent),
                cancellationToken);

            model.ExpectedFirstTranche = CurrencyHelper.DisplayPoundsPences(calculationResult.ExpectedFirstTranche);
            model.RentAsPercentageOfTheUnsoldShare = calculationResult.ProspectiveRentPercentage?.ToString("0.##", CultureInfo.InvariantCulture);

            ModelState.AddValidationErrors(operationResult);

            return View(model);
        }

        return await SaveHomeTypeSegment(
            new SaveOlderPersonsSharedOwnershipCommand(
                AhpApplicationId.From(applicationId),
                HomeTypeId.From(homeTypeId),
                model.MarketValue,
                model.InitialSale,
                model.ProspectiveRent),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)]
    [HttpGet("{homeTypeId}/exempt-from-the-right-to-shared-ownership")]
    public async Task<IActionResult> ExemptFromTheRightToSharedOwnership(
        [FromRoute] string applicationId,
        string homeTypeId,
        CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var model = new ExemptFromTheRightToSharedOwnershipModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            ExemptFromTheRightToSharedOwnership = tenureDetails.ExemptFromTheRightToSharedOwnership,
        };

        return View(model);
    }

    [WorkflowState(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)]
    [HttpPost("{homeTypeId}/exempt-from-the-right-to-shared-ownership")]
    public async Task<IActionResult> ExemptFromTheRightToSharedOwnership(
        [FromRoute] string applicationId,
        string homeTypeId,
        ExemptFromTheRightToSharedOwnershipModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveExemptFromTheRightToSharedOwnershipCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.ExemptFromTheRightToSharedOwnership),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ExemptionJustification)]
    [HttpGet("{homeTypeId}/exemption-justification")]
    public async Task<IActionResult> ExemptionJustification([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var tenureDetails = await _mediator.Send(new GetTenureDetailsQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new MoreInformationModel(tenureDetails.ApplicationName, tenureDetails.HomeTypeName)
        {
            MoreInformation = tenureDetails.ExemptionJustification,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.ExemptionJustification)]
    [HttpPost("{homeTypeId}/exemption-justification")]
    public async Task<IActionResult> ExemptionJustification(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveExemptionJustificationCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.MoreInformation),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstruction)]
    [HttpGet("{homeTypeId}/modern-methods-construction")]
    public async Task<IActionResult> ModernMethodsConstruction([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var modernMethodsConstruction = await _mediator.Send(new GetModernMethodsConstructionQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);

        return View(new ModernMethodsConstructionModel(modernMethodsConstruction.ApplicationName, modernMethodsConstruction.HomeTypeName));
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstruction)]
    [HttpPost("{homeTypeId}/modern-methods-construction")]
    public async Task<IActionResult> ModernMethodsConstruction(
        [FromRoute] string applicationId,
        string homeTypeId,
        ModernMethodsConstructionModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveModernMethodsConstructionCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.ModernMethodsConstructionApplied),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstructionCategories)]
    [HttpGet("{homeTypeId}/modern-methods-construction-categories")]
    public async Task<IActionResult> ModernMethodsConstructionCategories([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        return await ModernMethodsConstruction(applicationId, homeTypeId, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstructionCategories)]
    [HttpPost("{homeTypeId}/modern-methods-construction-categories")]
    public async Task<IActionResult> ModernMethodsConstructionCategories(
        [FromRoute] string applicationId,
        string homeTypeId,
        ModernMethodsConstructionModel model,
        CancellationToken cancellationToken)
    {
        var modernMethodsConstructionCategories = model.ModernMethodsConstructionCategories ?? Array.Empty<ModernMethodsConstructionCategoriesType>();

        return await SaveHomeTypeSegment(
            new SaveModernMethodsConstructionCategoriesCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), modernMethodsConstructionCategories.ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories)]
    [HttpGet("{homeTypeId}/modern-methods-construction-2d-subcategories")]
    public async Task<IActionResult> ModernMethodsConstruction2DSubcategories([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        return await ModernMethodsConstruction(applicationId, homeTypeId, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories)]
    [HttpPost("{homeTypeId}/modern-methods-construction-2d-subcategories")]
    public async Task<IActionResult> ModernMethodsConstruction2DSubcategories(
        [FromRoute] string applicationId,
        string homeTypeId,
        ModernMethodsConstructionModel model,
        CancellationToken cancellationToken)
    {
        var modernMethodsConstruction2DSubcategories = model.ModernMethodsConstruction2DSubcategories ?? Array.Empty<ModernMethodsConstruction2DSubcategoriesType>();

        return await SaveHomeTypeSegment(
            new SaveModernMethodsConstruction2DSubcategoriesCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), modernMethodsConstruction2DSubcategories.ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories)]
    [HttpGet("{homeTypeId}/modern-methods-construction-3d-subcategories")]
    public async Task<IActionResult> ModernMethodsConstruction3DSubcategories([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        return await ModernMethodsConstruction(applicationId, homeTypeId, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories)]
    [HttpPost("{homeTypeId}/modern-methods-construction-3d-subcategories")]
    public async Task<IActionResult> ModernMethodsConstruction3DSubcategories(
        [FromRoute] string applicationId,
        string homeTypeId,
        ModernMethodsConstructionModel model,
        CancellationToken cancellationToken)
    {
        var modernMethodsConstruction3DSubcategories = model.ModernMethodsConstruction3DSubcategories ?? Array.Empty<ModernMethodsConstruction3DSubcategoriesType>();

        return await SaveHomeTypeSegment(
            new SaveModernMethodsConstruction3DSubcategoriesCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), modernMethodsConstruction3DSubcategories.ToList()),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.CheckAnswers)]
    [HttpGet("{homeTypeId}/check-answers")]
    public async Task<IActionResult> CheckAnswers([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        return View(await GetHomeTypeAndCreateSummary(Url, applicationId, homeTypeId, cancellationToken));
    }

    [WorkflowState(HomeTypesWorkflowState.CheckAnswers)]
    [HttpPost("{homeTypeId}/check-answers")]
    public async Task<IActionResult> CheckAnswers(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomeTypeSummaryModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteHomeTypeCommand(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId), model.IsSectionCompleted), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("CheckAnswers", await GetHomeTypeAndCreateSummary(Url, applicationId, homeTypeId, cancellationToken));
        }

        return RedirectToAction("List", new { applicationId });
    }

    protected override async Task<IStateRouting<HomeTypesWorkflowState>> Routing(HomeTypesWorkflowState currentState, object? routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                            ?? routeData?.GetPropertyValue<string>("applicationId")
                            ?? throw new NotFoundException($"{nameof(HomeTypesController)} required applicationId path parameter.");
        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        if (string.IsNullOrEmpty(applicationId))
        {
            return new HomeTypesWorkflow(isReadOnly);
        }

        var homeTypeId = Request.GetRouteValue("homeTypeId") ?? routeData?.GetPropertyValue<string>("homeTypeId");
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypesWorkflow(currentState, null, isReadOnly);
        }

        var homeType = await _mediator.Send(new GetHomeTypeQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)));
        isReadOnly = isReadOnly || homeType.IsReadOnly;
        var workflow = new HomeTypesWorkflow(currentState, homeType, isReadOnly);
        if (TryGetWorkflowQueryParameter(out var lastEncodedWorkflow))
        {
            var lastWorkflow = new EncodedWorkflow<HomeTypesWorkflowState>(lastEncodedWorkflow);
            var currentWorkflow = workflow.GetEncodedWorkflow();
            var changedState = currentWorkflow.GetNextChangedWorkflowState(currentState, lastWorkflow);

            return new HomeTypesWorkflow(changedState, homeType, isReadOnly, true);
        }

        return workflow;
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
        AhpApplicationId applicationId,
        HomeTypeId? homeTypeId,
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

        return await ProcessAction(applicationId, HomeTypeId.From(result.ReturnedData!.Value));
    }

    private async Task<IActionResult> SaveHomeTypeSegment<TModel, TSaveSegmentCommand>(
        TSaveSegmentCommand command,
        TModel model,
        CancellationToken cancellationToken)
        where TSaveSegmentCommand : ISaveHomeTypeSegmentCommand
    {
        return await this.ExecuteCommand<TModel>(
            _mediator,
            command,
            async () => await ProcessAction(command.ApplicationId, command.HomeTypeId),
            async () => await Task.FromResult<IActionResult>(View(model)),
            cancellationToken);
    }

    private string GetDesignFileAction(string actionName, string applicationId, string homeTypeId, FileId fileId)
    {
        TryGetWorkflowQueryParameter(out var workflow);
        return Url.Action($"{actionName}DesignPlansFile", "HomeTypes", new { applicationId, homeTypeId, fileId = fileId.Value, workflow }) ?? string.Empty;
    }

    private async Task<IActionResult> ProcessAction(AhpApplicationId applicationId, HomeTypeId homeTypeId)
    {
        return await this.ReturnToTaskListOrContinue(
            async () => TryGetWorkflowQueryParameter(out var workflow)
                ? await Continue(new { applicationId = applicationId.Value, homeTypeId = homeTypeId.Value, workflow })
                : await Continue(new { applicationId = applicationId.Value, homeTypeId = homeTypeId.Value }));
    }

    private bool TryGetWorkflowQueryParameter(out string workflow)
    {
        if (QueryHelpers.ParseQuery(Request.QueryString.Value).TryGetValue("workflow", out var lastEncodedWorkflow))
        {
            workflow = lastEncodedWorkflow.ToString();
            return true;
        }

        workflow = string.Empty;
        return false;
    }

    private async Task<HomeTypeSummaryModel> GetHomeTypeAndCreateSummary(
        IUrlHelper urlHelper,
        string applicationId,
        string homeTypeId,
        CancellationToken cancellationToken)
    {
        var homeType = await _mediator.Send(new GetFullHomeTypeQuery(AhpApplicationId.From(applicationId), HomeTypeId.From(homeTypeId)), cancellationToken);
        var isEditable = await _accountAccessContext.CanEditApplication() && !homeType.IsReadOnly;
        var sections = _summaryViewModelFactory.CreateSummaryModel(homeType, urlHelper, !isEditable, true);

        return new HomeTypeSummaryModel(homeType.ApplicationName, homeType.Name)
        {
            IsSectionCompleted = homeType.IsCompleted ? IsSectionCompleted.Yes : IsSectionCompleted.Undefied,
            Sections = sections.ToList(),
            IsEditable = isEditable,
        };
    }
}
