using HE.Investments.Common.Contract;

namespace HE.Investments.Common.WWW.Models.TaskList;

public record TaskListSectionModel(string Header, string Description, string Action, string ActionUrl, SectionStatus Status, bool IsAvailable = true);
