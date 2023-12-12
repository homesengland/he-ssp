using System.Globalization;
using System.Reflection;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Routing;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.HomeTypes.Factories;

internal class HomeTypeQuestionFactory
{
    private readonly FullHomeType _homeType;

    private readonly IUrlHelper _urlHelper;

    private readonly HomeTypesWorkflow _workflow;

    private readonly EncodedWorkflow<HomeTypesWorkflowState>? _encodedWorkflow;

    public HomeTypeQuestionFactory(FullHomeType homeType, IUrlHelper urlHelper, bool useWorkflowRedirection)
    {
        _homeType = homeType;
        _urlHelper = urlHelper;
        _workflow = new HomeTypesWorkflow(homeType);
        _encodedWorkflow = useWorkflowRedirection ? _workflow.GetEncodedWorkflow() : null;
    }

    public SectionSummaryItemModel? Question(
        string questionName,
        string controllerActionName,
        params string?[]? answers)
    {
        var workflowState = GetWorkflowState(controllerActionName);
        if (!_workflow.CanBeAccessed(workflowState))
        {
            return null;
        }

        var validAnswers = answers?.Where(x => x != null).Select(x => x!).ToList() ?? new List<string>();
        return new SectionSummaryItemModel(questionName, validAnswers, CreateActionUrl(controllerActionName));
    }

    public SectionSummaryItemModel? Question(
        string questionName,
        string controllerActionName,
        int? answer)
    {
        return Question(questionName, controllerActionName, answer?.ToString(CultureInfo.InvariantCulture));
    }

    public SectionSummaryItemModel? Question<TEnum>(
        string questionName,
        string controllerActionName,
        params TEnum[]? answers)
        where TEnum : struct, Enum
    {
        return Question(
            questionName,
            controllerActionName,
            answers?.Where(x => Convert.ToInt32(x, CultureInfo.InvariantCulture) != 0).Select(x => x.GetDescription()).ToArray());
    }

    public SectionSummaryItemModel? FileQuestion(
        string questionName,
        string controllerActionName,
        string? answer,
        Dictionary<string, string> files)
    {
        var workflowState = GetWorkflowState(controllerActionName);
        if (!_workflow.CanBeAccessed(workflowState))
        {
            return null;
        }

        return new SectionSummaryItemModel(
            questionName,
            string.IsNullOrWhiteSpace(answer) ? null : new[] { answer },
            CreateActionUrl(controllerActionName),
            Files: files);
    }

    public SectionSummaryItemModel? DeadEnd(string controllerActionName)
    {
        var workflowState = GetWorkflowState(controllerActionName);
        if (!_workflow.CanBeAccessed(workflowState))
        {
            return null;
        }

        return new SectionSummaryItemModel("Dead End", null, CreateActionUrl(controllerActionName), IsVisible: false);
    }

    private static HomeTypesWorkflowState GetWorkflowState(string controllerActionName)
    {
        var method = typeof(HomeTypesController).GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(x => x.Name == controllerActionName);
        var workflowState = method?.GetCustomAttribute<WorkflowStateAttribute>();

        return workflowState?.StateAs<HomeTypesWorkflowState>() ??
               throw new InvalidOperationException($"Home type action {controllerActionName} with Workflow Attribute does not exist.");
    }

    private string CreateActionUrl(string controllerActionName)
    {
        if (_encodedWorkflow != null)
        {
            return _urlHelper.RouteUrl(
                "subSection",
                new
                {
                    controller = "HomeTypes",
                    action = controllerActionName,
                    applicationId = _homeType.ApplicationId,
                    id = _homeType.Id,
                    workflow = _encodedWorkflow.Value,
                }) ?? string.Empty;
        }

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
