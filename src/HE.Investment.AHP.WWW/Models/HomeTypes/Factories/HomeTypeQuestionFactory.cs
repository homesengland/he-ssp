using System.Globalization;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.HomeTypes.Factories;

internal class HomeTypeQuestionFactory
{
    private readonly FullHomeType _homeType;

    private readonly IUrlHelper _urlHelper;

    public HomeTypeQuestionFactory(FullHomeType homeType, IUrlHelper urlHelper)
    {
        _homeType = homeType;
        _urlHelper = urlHelper;
    }

    public SectionSummaryItemModel? Question(
        string questionName,
        string controllerActionName,
        HomeTypesWorkflowState workflowState,
        params string?[]? answers)
    {
        if (!new HomeTypesWorkflow(_homeType).CanBeAccessed(workflowState))
        {
            return null;
        }

        var validAnswers = answers?.Where(x => x != null).Select(x => x!).ToList() ?? new List<string>();
        return new SectionSummaryItemModel(questionName, validAnswers.Any() ? validAnswers : new[] { "Not Provided" }, CreateActionUrl(controllerActionName));
    }

    public SectionSummaryItemModel? Question(
        string questionName,
        string controllerActionName,
        HomeTypesWorkflowState workflowState,
        int? answer)
    {
        return Question(questionName, controllerActionName, workflowState, answer?.ToString(CultureInfo.InvariantCulture));
    }

    public SectionSummaryItemModel? Question<TEnum>(
        string questionName,
        string controllerActionName,
        HomeTypesWorkflowState workflowState,
        params TEnum[]? answers)
        where TEnum : struct, Enum
    {
        return Question(
            questionName,
            controllerActionName,
            workflowState,
            answers?.Where(x => Convert.ToInt32(x, CultureInfo.InvariantCulture) != 0).Select(x => x.GetDescription()).ToArray());
    }

    private string CreateActionUrl(string controllerActionName)
    {
        return _urlHelper.RouteUrl(
            "subSection",
            new
            {
                controller = "HomeTypes",
                action = controllerActionName,
                applicationId = _homeType.ApplicationId,
                id = _homeType.Id,
            }) ?? string.Empty;
    }
}
