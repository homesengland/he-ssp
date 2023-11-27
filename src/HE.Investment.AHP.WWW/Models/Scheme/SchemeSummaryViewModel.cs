using HE.Investments.Common.WWW.Components.SectionSummary;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public record SchemeSummaryViewModel(string ApplicationId, string ApplicationName, bool? IsCompleted, IList<SectionSummaryItemModel>? Items);
