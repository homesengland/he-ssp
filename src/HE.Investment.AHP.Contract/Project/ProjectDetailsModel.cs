namespace HE.Investment.AHP.Contract.Project;

public record ProjectDetailsModel(
    AhpProjectId ProjectId,
    string ProjectName,
    string ProgrammeName,
    string OrganisationName,
    IList<ApplicationProjectModel> Applications,
    bool IsReadOnly);
