using HE.Investments.Common.WWW.Models;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record UserOrganisationListModel(
    IList<OrganisationDetails>? Organisations,
    IList<ActionModel> Actions);
