using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Delivery.Factories;

public class DeliveryPhaseSummaryViewModelFactory : IDeliveryPhaseSummaryViewModelFactory
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
            CreateMilestonesSummary(applicationId, deliveryPhase),
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

        var items = new List<SectionSummaryItemModel>
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

        if (deliveryPhase.IsReconfiguringExistingNeeded)
        {
            items.Add(new(
                "Reconfiguring existing residential properties",
                deliveryPhase.ReconfiguringExisting?.ToString().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.ReconfiguringExisting))));
        }

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

    private static SectionSummaryViewModel CreateMilestonesSummary(
        AhpApplicationId applicationId,
        DeliveryPhaseDetails deliveryPhase)
    {
        var items = new List<SectionSummaryItemModel>
        {
            new(
                "Grant apportioned to this phase",
                "TODO".ToOneElementList(),
                IsEditable: false),
            new(
                "Completion milestone",
                "TODO".ToOneElementList(),
                IsEditable: false),
        };

        if (!deliveryPhase.IsUnregisteredBody && !deliveryPhase.IsOnlyCompletionMilestone)
        {
            items.Insert(
                1,
                new(
                    "Acquisition milestone",
                    "TODO".ToOneElementList(),
                    IsEditable: false));
            items.Insert(
                2,
                new(
                    "Start on site milestone",
                    "TODO".ToOneElementList(),
                    IsEditable: false));
        }

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

        var items = deliveryPhase.IsUnregisteredBody || deliveryPhase.IsOnlyCompletionMilestone
            ? new List<SectionSummaryItemModel>()
            : new List<SectionSummaryItemModel>
            {
                new(
                    "Acquisition date",
                    FormatDate(deliveryPhase.AcquisitionDate),
                    IsEditable: isEditable,
                    ActionUrl: CreateAction(nameof(DeliveryPhaseController.AcquisitionMilestone))),
                new(
                    "Forecast acquisition claim date",
                    FormatDate(deliveryPhase.AcquisitionPaymentDate),
                    IsEditable: isEditable,
                    ActionUrl: CreateAction(nameof(DeliveryPhaseController.AcquisitionMilestone))),
                new(
                    "Start on site date",
                    FormatDate(deliveryPhase.StartOnSiteDate),
                    IsEditable: isEditable,
                    ActionUrl: CreateAction(nameof(DeliveryPhaseController.StartOnSiteMilestone))),
                new(
                    "Forecast start on site claim date",
                    FormatDate(deliveryPhase.StartOnSitePaymentDate),
                    IsEditable: isEditable,
                    ActionUrl: CreateAction(nameof(DeliveryPhaseController.StartOnSiteMilestone))),
            };

        items.AddRange(new List<SectionSummaryItemModel>
        {
            new(
                "Completion date",
                FormatDate(deliveryPhase.PracticalCompletionDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))),
            new(
                "Forecast completion claim date",
                FormatDate(deliveryPhase.PracticalCompletionPaymentDate),
                IsEditable: isEditable,
                ActionUrl: CreateAction(nameof(DeliveryPhaseController.PracticalCompletionMilestone))),
        });

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
