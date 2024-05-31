using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Extensions;

public static class ControllerExtensions
{
    public static AhpApplicationId GetApplicationIdFromRoute(this Controller controller)
    {
        var applicationId = controller.Request.GetRouteValue("applicationId") ?? throw new NotFoundException("Missing required applicationId path parameter.");
        return AhpApplicationId.From(applicationId);
    }

    public static AhpApplicationId? TryGetApplicationIdFromRoute(this Controller controller)
    {
        var id = controller.Request.GetRouteValue("applicationId");
        var applicationId = !string.IsNullOrEmpty(id) ? AhpApplicationId.From(id) : null;

        return applicationId;
    }

    public static DeliveryPhaseId GetDeliveryPhaseIdFromRoute(this Controller controller)
    {
        var deliveryPhase = controller.Request.GetRouteValue("deliveryPhaseId") ?? throw new NotFoundException("Missing required deliveryPhaseId path parameter.");
        return DeliveryPhaseId.From(deliveryPhase);
    }

    public static SiteId GetSiteIdFromRoute(this Controller controller)
    {
        var siteId = controller.Request.GetRouteValue("siteId") ?? throw new NotFoundException("Missing required siteId path parameter.");
        return SiteId.From(siteId);
    }

    public static async Task<IActionResult> ReturnToTaskListOrContinue(
        this Controller controller,
        Func<Task<IActionResult>> onContinue)
    {
        var applicationId = controller.GetApplicationIdFromRoute();

        return await ReturnToListOrContinue(
            controller,
            async () => await Task.FromResult(controller.Url.RedirectToTaskList(applicationId.Value)),
            async () => await onContinue());
    }

    public static async Task<IActionResult> ReturnToSitesListOrContinue(
        this Controller controller,
        string projectId,
        Func<Task<IActionResult>> onContinue)
    {
        return await ReturnToListOrContinue(
            controller,
            async () => await Task.FromResult(controller.Url.RedirectToSitesList(projectId)),
            async () => await onContinue());
    }

    public static IActionResult ContinueSectionAnswering(
        this Controller controller,
        ISummaryViewModel summaryViewModel,
        Func<RedirectToActionResult> checkAnswersRedirectFactory)
    {
        if (summaryViewModel.IsReadOnly)
        {
            return checkAnswersRedirectFactory();
        }

        var firstNotAnsweredQuestion = summaryViewModel.Sections
            .Where(x => x.Items != null)
            .SelectMany(x => x.Items!)
            .FirstOrDefault(x => x is { HasAnswer: false, HasRedirectAction: true });

        return firstNotAnsweredQuestion != null
            ? controller.Redirect(firstNotAnsweredQuestion.ActionUrl!)
            : checkAnswersRedirectFactory();
    }

    private static async Task<IActionResult> ReturnToListOrContinue(
        this Controller controller,
        Func<Task<IActionResult>> onRedirectToList,
        Func<Task<IActionResult>> onContinue)
    {
        if (controller.Request.IsSaveAndReturnAction())
        {
            return await onRedirectToList();
        }

        return await onContinue();
    }
}
