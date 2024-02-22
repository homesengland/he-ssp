using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Site.Factories;

public interface ISiteSummaryViewModelFactory
{
    IEnumerable<SectionSummaryViewModel> CreateSiteSummary(SiteModel siteDetails, IUrlHelper urlHelper, bool isEditable);
}
