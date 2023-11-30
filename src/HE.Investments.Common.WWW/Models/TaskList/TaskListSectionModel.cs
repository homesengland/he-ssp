using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Gds;

namespace HE.Investments.Common.WWW.Models.TaskList;

public record TaskListSectionModel(string Header, string Description, string Action, string ActionUrl, SectionStatus Status, bool IsAvailable = true);
