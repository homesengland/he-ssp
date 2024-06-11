using HE.Investments.Common.WWW.Models;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Account.WWW.Models.UserOrganisations;

public record UserOrganisationsListModel(
    IList<OrganisationDetails>? Organisations,
    IList<ActionModel> Actions);
