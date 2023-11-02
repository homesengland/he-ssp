using System.Globalization;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
internal class LocalAuthorityMockedRepository : ILocalAuthorityRepository
{
    public LocalAuthorityMockedRepository()
    {
    }

    public Task AssignLocalAuthority(LoanApplicationId loanApplicationId, ProjectId projectId, LocalAuthorityId localAuthorityId, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<(IEnumerable<LocalAuthorityTemporaryDto> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken)
    {
        var foundItems = LocalAuthoritiesData.LocalAuthorities
                .Where(c => c.Name.ToLower(CultureInfo.InvariantCulture).Contains(phrase.ToLower(CultureInfo.InvariantCulture)));

        var totalItems = foundItems.Count();

        var itemsAtPage = foundItems
                .OrderBy(c => c.Name)
                .Skip(startPage * pageSize)
                .Take(pageSize);

        return Task.FromResult((itemsAtPage, totalItems));
    }
}
