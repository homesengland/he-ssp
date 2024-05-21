using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Project;

public record ProjectsListModel(string OrganisationName, string ProgrammeName, PaginationResult<ProjectModel> Result);
