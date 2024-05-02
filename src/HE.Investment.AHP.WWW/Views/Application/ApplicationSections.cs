using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models.TaskList;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Application;

public static class ApplicationSections
{
    public static IList<TaskListSectionModel> CreateSections(string applicationId, IUrlHelper url, IList<ApplicationSection> sections)
    {
        return new List<TaskListSectionModel?>
            {
                AddSection(
                    SectionType.Scheme,
                    sections,
                    "Scheme information",
                    "Complete information about the funding you require and discussions you've had with local stakeholders.",
                    "Enter scheme information",
                    _ => GetAction(url, applicationId, typeof(SchemeController), nameof(SchemeController.Start))),
                AddSection(
                    SectionType.HomeTypes,
                    sections,
                    "Home types",
                    "Complete information about the type of homes, information about the homes, home design and rent details.",
                    "Add home type",
                    section => GetAction(url, applicationId, typeof(HomeTypesController), section.SectionStatus == SectionStatus.NotStarted ? nameof(HomeTypesController.Index) : nameof(HomeTypesController.List))),
                AddSection(
                    SectionType.FinancialDetails,
                    sections,
                    "Financial details",
                    "Complete information about your finances, scheme costs and expected contributions.",
                    "Enter financial details",
                    _ => GetAction(url, applicationId, typeof(FinancialDetailsController), nameof(FinancialDetailsController.Start))),
                AddSection(
                    SectionType.DeliveryPhases,
                    sections,
                    "Delivery phases",
                    "Complete information about your delivery phases and milestone dates.",
                    "Add delivery phases",
                    section => GetAction(url, applicationId, typeof(DeliveryController), section.SectionStatus == SectionStatus.NotStarted ? nameof(DeliveryController.Start) : nameof(DeliveryController.List)),
                    isAvailable: sections.FirstOrDefault(s => s.SectionType == SectionType.HomeTypes)?.SectionStatus > SectionStatus.NotStarted),
            }
            .Where(i => i != null)
            .Cast<TaskListSectionModel>()
            .ToList();
    }

    private static TaskListSectionModel? AddSection(
        SectionType sectionType,
        IList<ApplicationSection> sections,
        string header,
        string description,
        string action,
        Func<ApplicationSection, string> getActionUrl,
        bool isAvailable = true)
    {
        var section = sections.FirstOrDefault(s => s.SectionType == sectionType);
        if (section == null)
        {
            return null;
        }

        return new(
            header,
            description,
            action,
            getActionUrl(section),
            section.SectionStatus,
            isAvailable);
    }

    private static string GetAction(IUrlHelper url, string applicationId, Type controller, string actionName)
    {
        ArgumentNullException.ThrowIfNull(controller);
        return url.Action(actionName, new ControllerName(controller.Name).WithoutPrefix(), new { applicationId }) ?? string.Empty;
    }
}
