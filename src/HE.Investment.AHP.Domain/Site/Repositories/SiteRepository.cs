using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class SiteRepository : ISiteRepository
{
    public Task<IList<SiteEntity>> GetSites(UserAccount userAccount, CancellationToken cancellationToken)
    {
        return MockedSiteEntities();
    }

    public async Task<SiteEntity> GetSite(UserAccount userAccount, string siteId)
    {
        return (await MockedSiteEntities()).FirstOrDefault(x => x.Id == siteId) ?? throw new NotFoundException("Site not found", siteId);
    }

    private Task<IList<SiteEntity>> MockedSiteEntities()
    {
        return Task.FromResult<IList<SiteEntity>>(new List<SiteEntity>
        {
            new("1", "Mocked Site 1"),
            new("2", "Mocked Site Carquinez"),
            new("3", "Mocked Site JJ"),
            new("4", "Mocked Site Antonios"),
            new("5", "Mocked Site 5"),
            new("6", "Mocked Site Dawidex"),
            new("7", "Mocked Site 7"),
            new("8", "Mocked Site Rafus"),
            new("9", "Mocked Site 9"),
            new("10", "Mocked Site 10"),
        });
    }
}
