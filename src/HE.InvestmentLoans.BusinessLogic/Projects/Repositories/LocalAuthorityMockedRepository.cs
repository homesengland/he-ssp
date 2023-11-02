using System.Globalization;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
internal class LocalAuthorityMockedRepository : ILocalAuthorityRepository
{
    private readonly List<LocalAuthorityTemporaryDto> _mockedData;

    public LocalAuthorityMockedRepository()
    {
        _mockedData = new List<LocalAuthorityTemporaryDto>();

        for (var i = 1; i <= 50; i++)
        {
            _mockedData.Add(new LocalAuthorityTemporaryDto(Guid.NewGuid(), $"Local authority- {i}"));
        }
    }

    public Task AssignLocalAuthority(LoanApplicationId loanApplicationId, ProjectId projectId, Guid localAuthorityId, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<(IEnumerable<LocalAuthorityTemporaryDto> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken)
    {
        var foundItems = _mockedData
                .Where(c => c.Name.ToLower(CultureInfo.InvariantCulture).Contains(phrase.ToLower(CultureInfo.InvariantCulture)));

        var totalItems = foundItems.Count();

        var itemsAtPage = foundItems
                .OrderBy(c => c.Name)
                .Skip(startPage * pageSize)
                .Take(pageSize);

        return Task.FromResult((itemsAtPage, totalItems));
    }
}
