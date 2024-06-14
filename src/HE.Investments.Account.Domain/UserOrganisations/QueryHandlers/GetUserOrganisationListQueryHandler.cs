using HE.Investments.Account.Contract.UserOrganisations.Queries;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.Contract;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisations.QueryHandlers;

public class GetUserOrganisationListQueryHandler : IRequestHandler<GetUserOrganisationListQuery, IList<OrganisationDetails>>
{
    private readonly IAccountUserContext _accountUserContext;

    public GetUserOrganisationListQueryHandler(IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
    }

    public async Task<IList<OrganisationDetails>> Handle(GetUserOrganisationListQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _accountUserContext.GetAccounts();

        return accounts?.Where(account => account.Organisation != null)
            .Select(account => new OrganisationDetails(
                account.Organisation!.RegisteredCompanyName,
                account.Organisation.AddressLine1,
                account.Organisation.City,
                account.Organisation.PostalCode,
                account.Organisation.CompanyRegistrationNumber,
                account.Organisation.OrganisationId.ToString()))
            .ToList() ?? [];
    }
}
