using HE.Investments.Organisation.Contract;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisations.Queries;

public class GetUserOrganisationListQuery : IRequest<IList<OrganisationDetails>>
{
}
