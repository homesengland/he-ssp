using HE.Investments.Common.Gds;

namespace HE.Investments.Common.WWW.Models.TaskList;

public record TaskListSectionModel(string Header, string Description, string Action, string ActionUrl, string Status, TagColour TagColour, bool IsAvailable = true);
