using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record ProjectsListModel(string OrganisationName, string ProgrammeName, PaginationResult<ProjectModel> Result);
