using HE.Investment.AHP.Contract.Site;
using HE.Investments.Account.Shared;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Site.Factories;

public interface ISiteSummaryViewModelFactory
{
    IEnumerable<SectionSummaryViewModel> CreateSiteSummary(
        SiteModel siteDetails,
        OrganisationBasicInfo organisation,
        IUrlHelper urlHelper,
        bool isEditable,
        bool useWorkflowRedirection);
}
