using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Project;

public record ProjectDetailsModel(
    AhpProjectId ProjectId,
    string ProjectName,
    string ProgrammeName,
    string OrganisationName,
    PaginationResult<ApplicationProjectModel> Applications,
    bool IsReadOnly);
