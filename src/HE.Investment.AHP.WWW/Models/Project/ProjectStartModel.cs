using HE.Investments.Programme.Contract;

namespace HE.Investment.AHP.WWW.Models.Project;

public record ProjectStartModel(string Id, Programme SelectedProgramme, bool DisplayConsortiumInfo, bool ShouldCreateNewProject);
