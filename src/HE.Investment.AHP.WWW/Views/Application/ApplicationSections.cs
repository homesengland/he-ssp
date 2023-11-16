using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Gds;
using HE.Investments.Common.WWW.Models.TaskList;

namespace HE.Investment.AHP.WWW.Views.Application;

public static class ApplicationSections
{
    public static IList<TaskListSectionModel> CreateSections(IList<ApplicationSection> sections)
    {
        return new List<TaskListSectionModel>
            {
                AddSection(
                    SectionType.Scheme,
                    sections,
                    "Scheme Information",
                    "Complete information about the tenure of your scheme, and other details about the funding you require and discussions you’ve had with local stakeholders.",
                    "Enter scheme information",
                    "scheme"),
                AddSection(
                    SectionType.HomeTypes,
                    sections,
                    "Home types",
                    "Complete information about the type of homes, information about the homes, home design and rent details.",
                    "Add homes types",
                    "homeTypes"),
                AddSection(
                    SectionType.FinancialDetails,
                    sections,
                    "Financial details",
                    "Complete information about your finances, scheme costs and expected contributions.",
                    "Enter financial details",
                    "#"),
                AddSection(
                    SectionType.DeliveryPhases,
                    sections,
                    "Delivery phases",
                    "Complete information about your delivery phases and milestone dates.",
                    "Add delivery phases",
                    "#",
                    false),
            }
            .Where(i => i != null)
            .ToList();
    }

    private static TaskListSectionModel AddSection(
        SectionType sectionType,
        IList<ApplicationSection> sections,
        string header,
        string description,
        string action,
        string actionUrl,
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
            actionUrl,
            section.SectionStatus,
            isAvailable);
    }
}
