using HE.Investments.Common.WWW.Components;

namespace HE.Investments.Common.WWW.Models.TaskList;

public record TaskListSectionModel(string Header, string Description, string Action, string ActionUrl, SectionStatus Status);
