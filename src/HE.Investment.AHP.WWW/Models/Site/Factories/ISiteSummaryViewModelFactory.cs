using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Site.Factories;

public interface ISiteSummaryViewModelFactory
{
    IEnumerable<SectionSummaryViewModel> CreateSiteSummary(SiteModel siteDetails, IUrlHelper urlHelper, bool isEditable, bool useWorkflowRedirection);
}
