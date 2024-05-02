using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Delivery.Factories;

public class DeliveryPhaseCheckAnswersViewModelFactory : IDeliveryPhaseCheckAnswersViewModelFactory
{
    private delegate string CreateAction(string actionName);

    public IList<SectionSummaryViewModel> CreateSummary(
        AhpApplicationId applicationId,
        DeliveryPhaseDetails deliveryPhase,
        DeliveryPhaseHomes deliveryPhaseHomes,
        IUrlHelper urlHelper,
        bool useWorkflowRedirection = true)
    {
        var encodedWorkflow = useWorkflowRedirection
            ? new DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState.Name, deliveryPhase).GetEncodedWorkflow()
            : null;

        string CreateAction(string actionName) =>
            CreateDeliveryPhaseActionUrl(
                urlHelper,
                applicationId,
                DeliveryPhaseId.From(deliveryPhase.Id),
                actionName,
                encodedWorkflow);

        return new List<SectionSummaryViewModel>
        {
            CreateDeliveryPhaseSummary(deliveryPhase, deliveryPhaseHomes, CreateAction, deliveryPhase.Application.IsEditable),
            CreateMilestonesSummary(deliveryPhase, CreateAction, deliveryPhase.Application.IsEditable),
            CreateMilestonesDatesSummary(deliveryPhase, CreateAction, deliveryPhase.Application.IsEditable),
        };
    }

    private static SectionSummaryViewModel CreateDeliveryPhaseSummary(
        DeliveryPhaseDetails deliveryPhase,
        DeliveryPhaseHomes deliveryPhaseHomes,
        CreateAction createAction,
        bool isEditable)
    {
        var items = new List<SectionSummaryItemModel>
        {
            new(
                "Phase name",
                deliveryPhase.Name.ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.Name))),
            new(
                "Type of homes",
                deliveryPhase.TypeOfHomes?.GetDescription().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.Details))),
            new(
                "Build activity type",
                deliveryPhase.BuildActivityType?.GetDescription().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(deliveryPhase.IsReconfiguringExistingNeeded
                    ? nameof(DeliveryPhaseController.RehabBuildActivityType)
                    : nameof(DeliveryPhaseController.NewBuildActivityType))),
        };

        items.AddWhen(
            new(
                "Reconfiguring existing residential properties",
                deliveryPhase.ReconfiguringExisting.MapToYesNo().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.ReconfiguringExisting))),
            deliveryPhase.IsReconfiguringExistingNeeded);

        foreach (var homeTypesToDeliver in deliveryPhaseHomes.HomeTypesToDeliver)
        {
            items.Add(new(
                $"Number of homes {homeTypesToDeliver.HomeTypeName}",
                homeTypesToDeliver.UsedHomes?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.AddHomes))));
        }

        return new SectionSummaryViewModel("Delivery phase", items);
    }

    private static SectionSummaryViewModel CreateMilestonesSummary(DeliveryPhaseDetails deliveryPhase, CreateAction createAction, bool isEditable)
    {
        var summaryItems = deliveryPhase.Tranches?.ShouldBeAmended == true
            ? CreateMilestone(deliveryPhase, deliveryPhase.Tranches?.SummaryOfDelivery, createAction, isEditable)
            : CreateMilestone(deliveryPhase, deliveryPhase.Tranches?.SummaryOfDelivery, createAction, false);

        return new SectionSummaryViewModel("Milestone summary", summaryItems.ToList());
    }

    private static IEnumerable<SectionSummaryItemModel> CreateMilestone(
        DeliveryPhaseDetails deliveryPhase,
        SummaryOfDelivery? summary,
        CreateAction createAction,
        bool isEditable)
    {
        yield return new("Grant apportioned to this phase", summary?.GrantApportioned.DisplayPoundsPences().ToOneElementList(), IsEditable: false);

        if (deliveryPhase is { IsOnlyCompletionMilestone: false })
        {
            yield return new(
                "Acquisition milestone",
                (summary?.AcquisitionMilestone.DisplayPoundsPences() ?? "-").ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.SummaryOfDelivery)));
        }

        if (deliveryPhase is { IsOnlyCompletionMilestone: false })
        {
            yield return new(
                "Start on site milestone",
                (summary?.StartOnSiteMilestone.DisplayPoundsPences() ?? "-").ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.SummaryOfDelivery)));
        }

        yield return new(
            "Completion milestone",
            (summary?.CompletionMilestone.DisplayPoundsPences() ?? "-").ToOneElementList(),
            IsEditable: isEditable,
            ActionUrl: createAction(nameof(DeliveryPhaseController.SummaryOfDelivery)));
    }

    private static SectionSummaryViewModel CreateMilestonesDatesSummary(DeliveryPhaseDetails deliveryPhase, CreateAction createAction, bool isEditable)
    {
        var items = new List<SectionSummaryItemModel>();
        items.AddWhen(
            new(
                "Acquisition date",
                FormatDate(deliveryPhase.AcquisitionDate),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.AcquisitionMilestone))),
            !deliveryPhase.IsOnlyCompletionMilestone);
        items.AddWhen(
            new(
                "Forecast acquisition claim date",
                FormatDate(deliveryPhase.AcquisitionPaymentDate),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.AcquisitionMilestone))),
            !deliveryPhase.IsOnlyCompletionMilestone);
        items.AddWhen(
            new(
                "Start on site date",
                FormatDate(deliveryPhase.StartOnSiteDate),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.StartOnSiteMilestone))),
            !deliveryPhase.IsOnlyCompletionMilestone);
        items.AddWhen(
            new(
                "Forecast start on site claim date",
                FormatDate(deliveryPhase.StartOnSitePaymentDate),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.StartOnSiteMilestone))),
            !deliveryPhase.IsOnlyCompletionMilestone);
        items.Add(
            new(
                "Completion date",
                FormatDate(deliveryPhase.PracticalCompletionDate),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))));
        items.Add(
            new(
                "Forecast completion claim date",
                FormatDate(deliveryPhase.PracticalCompletionPaymentDate),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))));
        items.AddWhen(
            new(
                "Request additional payments",
                deliveryPhase.IsAdditionalPaymentRequested?.ToString().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(DeliveryPhaseController.UnregisteredBodyFollowUp))),
            deliveryPhase.IsUnregisteredBody);

        return new SectionSummaryViewModel("Milestones", items);
    }

    private static string CreateDeliveryPhaseActionUrl(
        IUrlHelper urlHelper,
        AhpApplicationId applicationId,
        DeliveryPhaseId deliveryPhaseId,
        string actionName,
        EncodedWorkflow<DeliveryPhaseWorkflowState>? encodedWorkflow)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(DeliveryPhaseController)).WithoutPrefix(),
            new
            {
                applicationId = applicationId.Value,
                deliveryPhaseId = deliveryPhaseId.Value,
                workflow = encodedWorkflow?.Value,
            });

        return action ?? string.Empty;
    }

    private static IList<string>? FormatDate(DateDetails? date) => DateHelper.DisplayAsUkFormatDate(date)?.ToOneElementList();
}
