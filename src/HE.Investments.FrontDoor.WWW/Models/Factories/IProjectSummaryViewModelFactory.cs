using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Models.Factories;

public interface IProjectSummaryViewModelFactory
{
    IEnumerable<SectionSummaryViewModel> CreateProjectSummary(
        ProjectDetails projectDetails,
        ProjectSites projectSites,
        IUrlHelper urlHelper,
        bool isEditable,
        bool useWorkflowRedirection);
}
