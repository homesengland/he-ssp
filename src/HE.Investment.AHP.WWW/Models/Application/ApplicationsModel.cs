using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationsModel(string SiteName, IList<ApplicationBasicDetails> Applications);
