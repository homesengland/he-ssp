using HE.Investments.Common.WWW.Models.TaskList;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationModel(string SiteName, string Name, IList<TaskListSectionModel> Sections);
