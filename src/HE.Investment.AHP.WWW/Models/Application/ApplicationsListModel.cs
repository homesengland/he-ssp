using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationsListModel(string OrganisationName, IList<ApplicationBasicDetails> Applications);
