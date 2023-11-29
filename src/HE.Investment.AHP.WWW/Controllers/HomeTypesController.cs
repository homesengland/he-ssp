using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
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

    public HomeTypesController(IMediator mediator)
    {
        _mediator = mediator;
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

        return View("RemoveHomeTypeConfirmation", new RemoveHomeTypeBasicModel(homeTypeDetails.ApplicationName, homeTypeDetails.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.RemoveHomeType)]
    [HttpPost("{homeTypeId}/Remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string homeTypeId, RemoveHomeTypeBasicModel basicModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveHomeTypeCommand(applicationId, homeTypeId), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("RemoveHomeTypeConfirmation", basicModel);
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

        return View(new HomeInformationBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            NumberOfHomes = homeInformation.NumberOfHomes?.ToString(CultureInfo.InvariantCulture),
            NumberOfBedrooms = homeInformation.NumberOfBedrooms?.ToString(CultureInfo.InvariantCulture),
            MaximumOccupancy = homeInformation.MaximumOccupancy?.ToString(CultureInfo.InvariantCulture),
            NumberOfStoreys = homeInformation.NumberOfStoreys?.ToString(CultureInfo.InvariantCulture),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpPost("{homeTypeId}/HomeInformation")]
    public async Task<IActionResult> HomeInformation([FromRoute] string applicationId, string homeTypeId, HomeInformationBasicModel basicModel, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveHomeInformationCommand(
                applicationId,
                homeTypeId,
                basicModel.NumberOfHomes,
                basicModel.NumberOfBedrooms,
                basicModel.MaximumOccupancy,
                basicModel.NumberOfStoreys),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpGet("{homeTypeId}/HomesForDisabledPeople")]
    public async Task<IActionResult> HomesForDisabledPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var disabledPeopleHomeType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomesForDisabledPeopleBasicModel(disabledPeopleHomeType.ApplicationName, disabledPeopleHomeType.HomeTypeName)
        {
            HousingType = disabledPeopleHomeType.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpPost("{homeTypeId}/HomesForDisabledPeople")]
    public async Task<IActionResult> HomesForDisabledPeople(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomesForDisabledPeopleBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(new SaveDisabledPeopleHousingTypeCommand(applicationId, homeTypeId, basicModel.HousingType), basicModel, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleClientGroup)]
    [HttpGet("{homeTypeId}/DisabledPeopleClientGroup")]
    public async Task<IActionResult> DisabledPeopleClientGroup([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var disabledPeopleHomeType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new DisabledPeopleClientGroupBasicModel(disabledPeopleHomeType.ApplicationName, disabledPeopleHomeType.HomeTypeName)
        {
            DisabledPeopleClientGroup = disabledPeopleHomeType.ClientGroupType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleClientGroup)]
    [HttpPost("{homeTypeId}/DisabledPeopleClientGroup")]
    public async Task<IActionResult> DisabledPeopleClientGroup(
        [FromRoute] string applicationId,
        string homeTypeId,
        DisabledPeopleClientGroupBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveDisabledPeopleClientGroupTypeCommand(applicationId, homeTypeId, basicModel.DisabledPeopleClientGroup),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpGet("{homeTypeId}/HomesForOlderPeople")]
    public async Task<IActionResult> HomesForOlderPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var olderPeopleHomeType = await _mediator.Send(new GetOlderPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomesForOlderPeopleBasicModel(olderPeopleHomeType.ApplicationName, olderPeopleHomeType.HomeTypeName) { HousingType = olderPeopleHomeType.HousingType, });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpPost("{homeTypeId}/HomesForOlderPeople")]
    public async Task<IActionResult> HomesForOlderPeople([FromRoute] string applicationId, string homeTypeId, HomesForOlderPeopleBasicModel basicModel, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(new SaveOlderPeopleHousingTypeCommand(applicationId, homeTypeId, basicModel.HousingType), basicModel, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HappiDesignPrinciples)]
    [HttpGet("{homeTypeId}/HappiDesignPrinciples")]
    public async Task<IActionResult> HappiDesignPrinciples([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var designPlans = await _mediator.Send(new GetDesignPlansQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HappiDesignPrinciplesBasicModel(designPlans.ApplicationName, designPlans.HomeTypeName)
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
        HappiDesignPrinciplesBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        var designPrinciples = basicModel.DesignPrinciples ?? Array.Empty<HappiDesignPrincipleType>();
        var otherDesignPrinciples = basicModel.OtherPrinciples ?? Array.Empty<HappiDesignPrincipleType>();

        return await SaveHomeTypeSegment(
            new SaveHappiDesignPrinciplesCommand(applicationId, homeTypeId, designPrinciples.Concat(otherDesignPrinciples).ToList()),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpGet("{homeTypeId}/DesignPlans")]
    public async Task<IActionResult> DesignPlans([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var designPlans = await _mediator.Send(new GetDesignPlansQuery(applicationId, homeTypeId), cancellationToken);

        string GetRemoveAction(string fileId) => Url.RouteUrl("subSection", new { controller = "HomeTypes", action = "RemoveDesignPlansFile", applicationId, id = homeTypeId, fileId }) ?? throw new InvalidOperationException();
        return View(new DesignPlansBasicModel(designPlans.ApplicationName, designPlans.HomeTypeName)
        {
            MoreInformation = designPlans.MoreInformation,
            UploadedFiles = designPlans.UploadedFiles.Select(x => new FileModel(x.FileId, x.FileName, x.UploadedOn, x.UploadedBy, x.CanBeRemoved, GetRemoveAction(x.FileId))).ToList(),
        });
    }

    [DisableRequestSizeLimit]
    [WorkflowState(HomeTypesWorkflowState.DesignPlans)]
    [HttpPost("{homeTypeId}/DesignPlans")]
    public async Task<IActionResult> DesignPlans(
        [FromRoute] string applicationId,
        string homeTypeId,
        DesignPlansBasicModel basicModel,
        [FromForm(Name = "File")] List<IFormFile> files,
        CancellationToken cancellationToken)
    {
        var filesToUpload = files.Select(x => new FileToUpload(x.FileName, x.Length, x.OpenReadStream())).ToList();

        try
        {
            return await SaveHomeTypeSegment(new SaveDesignPlansCommand(applicationId, homeTypeId, basicModel.MoreInformation, filesToUpload), basicModel, cancellationToken);
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

    [WorkflowState(HomeTypesWorkflowState.SupportedHousingInformation)]
    [HttpGet("{homeTypeId}/SupportedHousingInformation")]
    public async Task<IActionResult> SupportedHousingInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new SupportedHousingInformationBasicModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
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
        SupportedHousingInformationBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveSupportedHousingInformationCommand(
                applicationId,
                homeTypeId,
                basicModel.LocalCommissioningBodiesConsulted,
                basicModel.ShortStayAccommodation,
                basicModel.RevenueFundingType),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.RevenueFunding)]
    [HttpGet("{homeTypeId}/RevenueFunding")]
    public async Task<IActionResult> RevenueFunding([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new RevenueFundingBasicModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            Sources = supportedHousingInformation.RevenueFundingSources,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.RevenueFunding)]
    [HttpPost("{homeTypeId}/RevenueFunding")]
    public async Task<IActionResult> RevenueFunding(
        [FromRoute] string applicationId,
        string homeTypeId,
        RevenueFundingBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        var revenueFundingSources = basicModel.Sources ?? Array.Empty<RevenueFundingSourceType>();

        return await SaveHomeTypeSegment(
            new SaveRevenueFundingCommand(applicationId, homeTypeId, revenueFundingSources.ToList()),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnAccommodation)]
    [HttpGet("{homeTypeId}/MoveOnAccommodation")]
    public async Task<IActionResult> MoveOnAccommodation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoveOnAccommodationBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            IntendedAsMoveOnAccommodation = homeInformation.IntendedAsMoveOnAccommodation,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnAccommodation)]
    [HttpPost("{homeTypeId}/MoveOnAccommodation")]
    public async Task<IActionResult> MoveOnAccommodation([FromRoute] string applicationId, string homeTypeId, MoveOnAccommodationBasicModel basicModel, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveMoveOnAccommodationCommand(applicationId, homeTypeId, basicModel.IntendedAsMoveOnAccommodation),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)]
    [HttpGet("{homeTypeId}/PeopleGroupForSpecificDesignFeatures")]
    public async Task<IActionResult> PeopleGroupForSpecificDesignFeatures([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new PeopleGroupForSpecificDesignFeaturesBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            PeopleGroupForSpecificDesignFeatures = homeInformation.PeopleGroupForSpecificDesignFeatures,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures)]
    [HttpPost("{homeTypeId}/PeopleGroupForSpecificDesignFeatures")]
    public async Task<IActionResult> PeopleGroupForSpecificDesignFeatures([FromRoute] string applicationId, string homeTypeId, PeopleGroupForSpecificDesignFeaturesBasicModel basicModel, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SavePeopleGroupForSpecificDesignFeaturesCommand(applicationId, homeTypeId, basicModel.PeopleGroupForSpecificDesignFeatures),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnArrangements)]
    [HttpGet("{homeTypeId}/MoveOnArrangements")]
    public async Task<IActionResult> MoveOnArrangements([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoreInformationBasicModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.MoveOnArrangements,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.MoveOnArrangements)]
    [HttpPost("{homeTypeId}/MoveOnArrangements")]
    public async Task<IActionResult> MoveOnArrangements(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveMoveOnArrangementsCommand(applicationId, homeTypeId, basicModel.MoreInformation),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.TypologyLocationAndDesign)]
    [HttpGet("{homeTypeId}/TypologyLocationAndDesign")]
    public async Task<IActionResult> TypologyLocationAndDesign([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoreInformationBasicModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.TypologyLocationAndDesign,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.TypologyLocationAndDesign)]
    [HttpPost("{homeTypeId}/TypologyLocationAndDesign")]
    public async Task<IActionResult> TypologyLocationAndDesign(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveTypologyLocationAndDesignCommand(applicationId, homeTypeId, basicModel.MoreInformation),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.ExitPlan)]
    [HttpGet("{homeTypeId}/ExitPlan")]
    public async Task<IActionResult> ExitPlan([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var supportedHousingInformation = await _mediator.Send(new GetSupportedHousingInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new MoreInformationBasicModel(supportedHousingInformation.ApplicationName, supportedHousingInformation.HomeTypeName)
        {
            MoreInformation = supportedHousingInformation.ExitPlan,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.ExitPlan)]
    [HttpPost("{homeTypeId}/ExitPlan")]
    public async Task<IActionResult> ExitPlan(
        [FromRoute] string applicationId,
        string homeTypeId,
        MoreInformationBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveExitPlanCommand(applicationId, homeTypeId, basicModel.MoreInformation),
            basicModel,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformation)]
    [HttpGet("{homeTypeId}/BuildingInformation")]
    public async Task<IActionResult> BuildingInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new BuildingInformationBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            BuildingType = homeInformation.BuildingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.BuildingInformation)]
    [HttpPost("{homeTypeId}/BuildingInformation")]
    public async Task<IActionResult> BuildingInformation(
        [FromRoute] string applicationId,
        string homeTypeId,
        BuildingInformationBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveBuildingInformationCommand(applicationId, homeTypeId, basicModel.BuildingType),
            basicModel,
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

        return View(new CustomBuildPropertyBasicModel(homeInformation.ApplicationName, homeInformation.HomeTypeName)
        {
            CustomBuild = homeInformation.CustomBuild,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.CustomBuildProperty)]
    [HttpPost("{homeTypeId}/CustomBuildProperty")]
    public async Task<IActionResult> CustomBuildProperty(
        [FromRoute] string applicationId,
        string homeTypeId,
        CustomBuildPropertyBasicModel basicModel,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveCustomBuildPropertyCommand(applicationId, homeTypeId, basicModel.CustomBuild),
            basicModel,
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
}
