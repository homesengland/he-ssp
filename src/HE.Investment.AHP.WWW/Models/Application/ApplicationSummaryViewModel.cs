namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationSummaryViewModel(string ApplicationId, string ApplicationName, IList<SectionSummaryViewModel> Sections);
