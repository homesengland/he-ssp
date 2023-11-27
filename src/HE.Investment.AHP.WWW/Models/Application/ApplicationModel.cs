using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationModel(string ApplicationId, string SiteName, string Name, IList<ApplicationSection> Sections);
