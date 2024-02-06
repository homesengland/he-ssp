using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Delivery.Factories;

public class DeliveryPhaseCheckAnswersViewModelFactory : IDeliveryPhaseCheckAnswersViewModelFactory
{
    public IList<SectionSummaryViewModel> CreateSummary(
        AhpApplicationId applicationId,
        DeliveryPhaseDetails deliveryPhase,
        DeliveryPhaseHomes deliveryPhaseHomes,
        IUrlHelper urlHelper,
        bool isEditable)
    {
        return new List<SectionSummaryViewModel>
        {
            CreateDeliveryPhaseSummary(applicationId, deliveryPhase, deliveryPhaseHomes, urlHelper, isEditable),
            CreateMilestonesSummary(deliveryPhase),
            CreateMilestonesDatesSummary(
                applicationId,
                deliveryPhase,
                urlHelper,
                isEditable),
        };
    }

    private static SectionSummaryViewModel CreateDeliveryPhaseSummary(
        AhpApplicationId applicationId,
        DeliveryPhaseDetails deliveryPhase,
        DeliveryPhaseHomes deliveryPhaseHomes,
        IUrlHelper urlHelper,
        bool isEditable)
    {
        string CreateAction(string actionName) =>
            CreateDeliveryPhaseActionUrl(
                urlHelper,
                applicationId,
                new DeliveryPhaseId(deliveryPhase.Id),
                actionName);

        IList<SectionSummaryItemModel> items = new List<SectionSummaryItemModel>
        {
            new(
                "Phase name",
                deliveryPhase.Name.ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.Name))),
            new(
                "Type of homes",
                deliveryPhase.TypeOfHomes?.GetDescription().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.Details))),
            new(
                "Build activity type",
                deliveryPhase.BuildActivityType?.GetDescription().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.BuildActivityType))),
        };

        items.AddWhen(
            new(
                "Reconfiguring existing residential properties",
                deliveryPhase.ReconfiguringExisting?.ToString().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.ReconfiguringExisting))),
            deliveryPhase.IsReconfiguringExistingNeeded);

        foreach (var homeTypesToDeliver in deliveryPhaseHomes.HomeTypesToDeliver)
        {
            items.Add(new(
                $"Number of homes {homeTypesToDeliver.HomeTypeName}",
                homeTypesToDeliver.UsedHomes?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.AddHomes))));
        }

        return new SectionSummaryViewModel("Delivery phase", items);
    }

    private static SectionSummaryViewModel CreateMilestonesSummary(DeliveryPhaseDetails deliveryPhase)
    {
        var summary = deliveryPhase.Tranches?.SummaryOfDelivery;
        var items = new List<SectionSummaryItemModel>
        {
            new(
                "Grant apportioned to this phase",
                ((summary?.GrantApportioned).DisplayPoundsPences() ?? "-").ToOneElementList(),
                IsEditable: false),
        };
        items.AddWhen(
            new(
                "Acquisition milestone",
                ((summary?.AcquisitionMilestone).DisplayPoundsPences() ?? "-").ToOneElementList(),
                IsEditable: false),
            deliveryPhase is { IsUnregisteredBody: false, IsOnlyCompletionMilestone: false });
        items.AddWhen(
            new(
                "Start on site milestone",
                ((summary?.StartOnSiteMilestone).DisplayPoundsPences() ?? "-").ToOneElementList(),
                IsEditable: false),
            deliveryPhase is { IsUnregisteredBody: false, IsOnlyCompletionMilestone: false });
        items.Add(new(
            "Completion milestone",
            ((summary?.CompletionMilestone).DisplayPoundsPences() ?? "-").ToOneElementList(),
            IsEditable: false));

        return new SectionSummaryViewModel("Milestone summary", items);
    }

    private static SectionSummaryViewModel CreateMilestonesDatesSummary(
        AhpApplicationId applicationId,
        DeliveryPhaseDetails deliveryPhase,
        IUrlHelper urlHelper,
        bool isEditable)
    {
        string CreateAction(string actionName) =>
            CreateDeliveryPhaseActionUrl(
                urlHelper,
                applicationId,
                new DeliveryPhaseId(deliveryPhase.Id),
                actionName);

        var showOnlyCompletionMilestone = deliveryPhase.IsUnregisteredBody || deliveryPhase.IsOnlyCompletionMilestone;

        var items = new List<SectionSummaryItemModel>();
        items.AddWhen(
            new(
                "Acquisition date",
                FormatDate(deliveryPhase.AcquisitionDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.AcquisitionMilestone))),
            !showOnlyCompletionMilestone);
        items.AddWhen(
            new(
                "Forecast acquisition claim date",
                FormatDate(deliveryPhase.AcquisitionPaymentDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.AcquisitionMilestone))),
            !showOnlyCompletionMilestone);
        items.AddWhen(
            new(
                "Start on site date",
                FormatDate(deliveryPhase.StartOnSiteDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.StartOnSiteMilestone))),
            !showOnlyCompletionMilestone);
        items.AddWhen(
            new(
                "Forecast start on site claim date",
                FormatDate(deliveryPhase.StartOnSitePaymentDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.StartOnSiteMilestone))),
            !showOnlyCompletionMilestone);
        items.Add(
            new(
                "Completion date",
                FormatDate(deliveryPhase.PracticalCompletionDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))));
        items.Add(
            new(
                "Forecast completion claim date",
                FormatDate(deliveryPhase.PracticalCompletionPaymentDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))));
        items.AddWhen(
            new(
                "Request additional payments",
                deliveryPhase.IsAdditionalPaymentRequested?.ToString().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))),
            deliveryPhase.IsUnregisteredBody);

        return new SectionSummaryViewModel("Milestones", items);
    }

    private static string CreateDeliveryPhaseActionUrl(IUrlHelper urlHelper, AhpApplicationId applicationId, DeliveryPhaseId deliveryPhaseId, string actionName)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(DeliveryPhaseController)).WithoutPrefix(),
            new { applicationId = applicationId.Value, deliveryPhaseId = deliveryPhaseId.Value, redirect = nameof(DeliveryPhaseController.CheckAnswers) });

        return action ?? string.Empty;
    }

    private static IList<string>? FormatDate(DateDetails? date)
    {
        if (date == null)
        {
            return Array.Empty<string>();
        }

        return $"{date.Day}/{date.Month}/{date.Year}".ToOneElementList();
    }
}
