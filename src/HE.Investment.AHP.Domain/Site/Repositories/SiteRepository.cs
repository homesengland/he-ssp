using System.Globalization;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class SiteRepository : ISiteRepository
{
    private static readonly IList<SiteEntity> MockedSites = MockedSiteEntities();

    public Task<bool> IsExist(SiteName name, SiteId exceptSiteId, CancellationToken cancellationToken)
    {
        return Task.FromResult(MockedSites.Any(x => x.Name == name && x.Id != exceptSiteId));
    }

    public Task<IList<SiteEntity>> GetSites(UserAccount userAccount, CancellationToken cancellationToken)
    {
        return Task.FromResult(MockedSites);
    }

    public Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (siteId.IsNew)
        {
            return Task.FromResult(new SiteEntity());
        }

        return Task.FromResult(MockedSites.FirstOrDefault(x => x.Id == siteId) ?? throw new NotFoundException("Site not found", siteId));
    }

    public Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (site.Id.IsNew)
        {
            site.Id = new SiteId((MockedSites.Count + 1).ToString(CultureInfo.InvariantCulture));
            MockedSites.Add(site);
        }
        else
        {
            var existingSite = MockedSites.SingleOrDefault(x => x.Id == site.Id)
                ?? throw new NotFoundException("Site not found", site.Id);

            MockedSites.Remove(existingSite);
            MockedSites.Add(site);
        }

        return Task.FromResult(site.Id);
    }

    private static IList<SiteEntity> MockedSiteEntities()
    {
        return new List<SiteEntity>
        {
            new(new SiteId("1"), new SiteName("Mocked Site 1"), new Section106Entity()),
            new(new SiteId("2"), new SiteName("Mocked Site Carquinez"), new Section106Entity()),
            new(new SiteId("3"), new SiteName("Mocked Site JJ"), new Section106Entity()),
            new(new SiteId("4"), new SiteName("Mocked Site Antonios"), new Section106Entity()),
            new(new SiteId("5"), new SiteName("Mocked Site 5"), new Section106Entity()),
            new(new SiteId("6"), new SiteName("Mocked Site Dawidex"), new Section106Entity()),
            new(new SiteId("7"), new SiteName("Mocked Site 7"), new Section106Entity()),
            new(new SiteId("8"), new SiteName("Mocked Site Rafus"), new Section106Entity(true, false, null, null, null, null)),
            new(new SiteId("9"), new SiteName("Mocked Site 9"), new Section106Entity()),
            new(new SiteId("10"), new SiteName("Mocked Site 10"), new Section106Entity()),
        };
    }
}
