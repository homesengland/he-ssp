using HE.Investments.AHP.Consortium.Contract.Queries;

namespace HE.Investment.AHP.WWW.Models.Consortium;

public record SelectProgramme(string SelectedProgrammeId, AvailableProgramme[] AvailableProgrammes);
