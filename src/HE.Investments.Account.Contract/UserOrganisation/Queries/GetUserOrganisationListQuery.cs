using HE.Investments.Organisation.Contract;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Queries;

public class GetUserOrganisationListQuery : IRequest<IList<OrganisationDetails>>
{
}
