using HE.Investments.Programme.Contract;

namespace HE.Investment.AHP.WWW.Models.Consortium;

public record SelectProgramme(string SelectedProgrammeId, IList<Programme> AvailableProgrammes);
