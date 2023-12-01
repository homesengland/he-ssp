using HE.Investment.AHP.WWW.Models.Application;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public record SchemeSummaryViewModel(string ApplicationId, string ApplicationName, bool? IsCompleted, SectionSummaryViewModel Section);
