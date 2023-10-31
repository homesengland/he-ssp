using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Models.TaskList;

namespace HE.Investment.AHP.WWW.Views.Application;

public static class ApplicationSections
{
    public static IList<TaskListSectionModel> CreateSections(
        SectionStatus schemeSectionStatus,
        SectionStatus homeTypesSectionStatus,
        SectionStatus financialSectionStatus,
        SectionStatus deliverySectionStatus)
    {
        return new List<TaskListSectionModel>
        {
            new(
                "Scheme Information",
                "Complete information about the tenure of your scheme, and other details about the funding you require and discussions you’ve had with local stakeholders.",
                "Enter scheme information",
                "#",
                schemeSectionStatus),
            new(
                "Home types",
                "Complete information about the type of homes, information about the homes, home design and rent details.",
                "Add homes types",
                "homeTypes",
                homeTypesSectionStatus),
            new(
                "Financial details",
                "Complete information about your finances, scheme costs and expected contributions.",
                "Enter financial details",
                "#",
                financialSectionStatus),
            new(
                "Delivery phases",
                "Complete information about your delivery phases and milestone dates. ",
                "Add delivery phases",
                "#",
                deliverySectionStatus),
        };
    }
}
