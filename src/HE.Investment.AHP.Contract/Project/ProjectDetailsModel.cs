namespace HE.Investment.AHP.Contract.Project;

public record ProjectDetailsModel(
    string ProjectId,
    string ProjectName,
    string ProgrammeName,
    string OrganisationName,
    IList<ApplicationProjectModel> Applications,
    bool IsReadOnly);
