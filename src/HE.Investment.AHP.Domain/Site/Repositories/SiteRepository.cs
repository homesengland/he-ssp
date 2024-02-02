extern alias Org;

using System.Globalization;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;
using LocalAuthorityId = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthorityId;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class SiteRepository : ISiteRepository
{
    private static readonly IList<SiteEntity> MockedSites = MockedSiteEntities();

    public Task<bool> IsExist(SiteName name, SiteId exceptSiteId, CancellationToken cancellationToken)
    {
        return Task.FromResult(MockedSites.Any(x => x.Name == name && x.Id != exceptSiteId));
    }

    public Task<PaginationResult<SiteEntity>> GetSites(UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PaginationResult<SiteEntity>(
            MockedSites.TakePage(paginationRequest).ToList(),
            paginationRequest.Page,
            paginationRequest.ItemsPerPage,
            MockedSites.Count));
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
            new(new SiteId("1"), new SiteName("Mocked Site 1"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(), new LocalAuthority(new LocalAuthorityId("1"), "local auth")),
            new(new SiteId("2"), new SiteName("Mocked Site Carquinez"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("3"), new SiteName("Mocked Site JJ"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(SiteTenderingStatus.TenderForWorksContract)),
            new(new SiteId("4"), new SiteName("Mocked Site Antonios"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(), new LocalAuthority(new LocalAuthorityId("4"), "local auth")),
            new(new SiteId("5"), new SiteName("Mocked Site 5"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("6"), new SiteName("Mocked Site Dawidex"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(), new LocalAuthority(new LocalAuthorityId("6"), "local auth")),
            new(new SiteId("7"), new SiteName("Mocked Site 7"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("8"), new SiteName("Mocked Site Rafus"), new Section106(true, false, null, null, null, null), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("9"), new SiteName("Mocked Site 9"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("10"), new SiteName("Mocked Site 2 10"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("11"), new SiteName("Mocked Site 2 1"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("12"), new SiteName("Mocked Site 2 Carquinez"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("13"), new SiteName("Mocked Site 2 JJ"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(SiteTenderingStatus.ConditionalWorksContract)),
            new(new SiteId("14"), new SiteName("Mocked Site 2 Antonios"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(), new LocalAuthority(new LocalAuthorityId("14"), "local auth")),
            new(new SiteId("15"), new SiteName("Mocked Site 2 5"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("16"), new SiteName("Mocked Site 2 Dawidex"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("17"), new SiteName("Mocked Site 2 7"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("18"), new SiteName("Mocked Site 2 Rafus"), new Section106(true, false, null, null, null, null), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
            new(new SiteId("19"), new SiteName("Mocked Site 2 9"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails(), new LocalAuthority(new LocalAuthorityId("19"), "local auth")),
            new(new SiteId("20"), new SiteName("Mocked Site 2 10"), new Section106(), PlanningDetailsFactory.CreateEmpty(), new NationalDesignGuidePriorities(), new TenderingStatusDetails()),
        };
    }
}
