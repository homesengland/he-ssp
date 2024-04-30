using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.WWW.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Models.Factories;

public class ProjectSummaryViewModelFactory : IProjectSummaryViewModelFactory
{
    private delegate string CreateAction(string actionName);

    public IEnumerable<SectionSummaryViewModel> CreateProjectSummary(
        ProjectDetails projectDetails,
        ProjectSites projectSites,
        IUrlHelper urlHelper,
        bool isEditable,
        bool useWorkflowRedirection)
    {
        string CreateProjectAction(string actionName) =>
            CreateProjectActionUrl(urlHelper, projectDetails.Id, actionName, useWorkflowRedirection);

        if (projectDetails.IsSiteIdentified == true)
        {
            yield return new SectionSummaryViewModel(string.Empty, CreateProjectBasicDetailsSummary(projectDetails, CreateProjectAction, isEditable));

            if (!projectSites.Sites.Any())
            {
                projectSites.Sites.Add(new SiteDetails { Id = FrontDoorSiteId.New() });
            }

            foreach (var siteSummary in CreateSitesSummary(urlHelper, projectDetails, projectSites, CreateProjectAction, isEditable, useWorkflowRedirection))
            {
                yield return siteSummary;
            }

            yield return new SectionSummaryViewModel(string.Empty, CreateProjectFinancialDetailsSummary(projectDetails, CreateProjectAction, isEditable));
        }
        else
        {
            yield return new SectionSummaryViewModel(string.Empty, CreateProjectWithoutSiteSummary(projectDetails, CreateProjectAction, isEditable));
        }
    }

    private static IList<SectionSummaryItemModel> CreateProjectWithoutSiteSummary(ProjectDetails projectDetails, CreateAction createAction, bool isEditable)
    {
        var sectionSummary = CreateProjectBasicDetailsSummary(projectDetails, createAction, isEditable);

        sectionSummary.Add(new(
            "Identified site",
            SummaryAnswerHelper.ToYesNo(projectDetails.IsSiteIdentified),
            createAction(nameof(ProjectController.IdentifiedSite)),
            IsEditable: isEditable));

        sectionSummary.Add(new(
            "Geographic focus ",
            SummaryAnswerHelper.ToEnum(projectDetails.GeographicFocus),
            createAction(nameof(ProjectController.GeographicFocus)),
            IsEditable: isEditable));

        sectionSummary.AddWhen(
            new(
            "Region",
            projectDetails.Regions?.Select(x => x.GetDescription()).ToList(),
            createAction(nameof(ProjectController.Region)),
            IsEditable: isEditable),
            projectDetails.GeographicFocus == ProjectGeographicFocus.Regional);

        sectionSummary.AddWhen(
            new(
                "Local authority",
                projectDetails.LocalAuthorityName.ToOneElementList(),
                createAction(nameof(ProjectController.LocalAuthoritySearch)),
                IsEditable: isEditable),
            projectDetails.GeographicFocus == ProjectGeographicFocus.SpecificLocalAuthority);

        sectionSummary.Add(new(
            "Homes your project enables",
            projectDetails.HomesNumber.ToOneElementList(),
            createAction(nameof(ProjectController.HomesNumber)),
            IsEditable: isEditable));

        sectionSummary.AddRange(CreateProjectFinancialDetailsSummary(projectDetails, createAction, isEditable));

        return sectionSummary;
    }

    private static List<SectionSummaryItemModel> CreateProjectBasicDetailsSummary(ProjectDetails projectDetails, CreateAction createAction, bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new(
                "Project in England",
                SummaryAnswerHelper.ToYesNo(projectDetails.IsEnglandHousingDelivery),
                createAction(nameof(ProjectController.EnglandHousingDelivery)),
                IsEditable: false),
            new(
                "Project name",
                projectDetails.Name.ToOneElementList(),
                createAction(nameof(ProjectController.Name)),
                IsEditable: isEditable),
            new(
                "Activities you require support for",
                projectDetails.SupportActivityTypes?.Select(x => x.GetDescription()).ToList(),
                createAction(nameof(ProjectController.SupportRequiredActivities)),
                IsEditable: isEditable),
        };

        summary.AddWhen(
            new(
                "What infrastructure does your project deliver?",
                projectDetails.InfrastructureTypes?.Select(x => x.GetDescription()).ToList(),
                createAction(nameof(ProjectController.Infrastructure)),
                IsEditable: isEditable),
            projectDetails.SupportActivityTypes?.Count == 1 && projectDetails.SupportActivityTypes.Contains(SupportActivityType.ProvidingInfrastructure));

        if (projectDetails.SupportActivityTypes?.Count == 1 && projectDetails.SupportActivityTypes.Contains(SupportActivityType.DevelopingHomes))
        {
            summary.AddRange(new[]
            {
                new SectionSummaryItemModel(
                    "Amount of affordable homes",
                    SummaryAnswerHelper.ToEnum(projectDetails.AffordableHomesAmount),
                    createAction(nameof(ProjectController.Tenure)),
                    IsEditable: isEditable),
                new SectionSummaryItemModel(
                    "Previous residential building experience",
                    projectDetails.OrganisationHomesBuilt.ToOneElementList(),
                    createAction(nameof(ProjectController.OrganisationHomesBuilt)),
                    IsEditable: isEditable),
            });
        }

        return summary;
    }

    private static List<SectionSummaryViewModel> CreateSitesSummary(
        IUrlHelper urlHelper,
        ProjectDetails projectDetails,
        ProjectSites projectSites,
        CreateAction createAction,
        bool isEditable,
        bool useWorkflowRedirection)
    {
        var summaries = new List<SectionSummaryViewModel>();

        foreach (var site in projectSites.Sites)
        {
            string CreateSiteAction(string actionName) =>
                CreateSiteActionUrl(urlHelper, projectSites.ProjectId, site.Id, actionName, useWorkflowRedirection);

            var sectionSummaryItems = new List<SectionSummaryItemModel>
            {
                new(
                    "Identified site",
                    SummaryAnswerHelper.ToYesNo(projectDetails.IsSiteIdentified),
                    createAction(nameof(ProjectController.IdentifiedSite)),
                    IsEditable: isEditable),
            };

            var siteSummary = new SectionSummaryViewModel(
                site.Name,
                CreateSiteDetailsSummary(site, CreateSiteAction, isEditable));

            if (siteSummary.Items.IsProvided())
            {
                sectionSummaryItems.AddRange(siteSummary.Items!);
            }

            summaries.Add(new SectionSummaryViewModel(site.Name, sectionSummaryItems));
        }

        return summaries;
    }

    private static List<SectionSummaryItemModel> CreateSiteDetailsSummary(SiteDetails site, CreateAction createAction, bool isEditable)
    {
        return new List<SectionSummaryItemModel>
        {
            new("Site name", site.Name.ToOneElementList(), createAction(nameof(SiteController.Name)), IsEditable: isEditable),
            new("Number of homes", site.HomesNumber.ToOneElementList(), createAction(nameof(SiteController.HomesNumber)), IsEditable: isEditable),
            new(
                "Local authority",
                site.LocalAuthorityName.ToOneElementList(),
                createAction(nameof(SiteController.LocalAuthoritySearch)),
                IsEditable: isEditable),
            new(
                "Planning status",
                SummaryAnswerHelper.ToEnum(site.PlanningStatus),
                createAction(nameof(SiteController.PlanningStatus)),
                IsEditable: isEditable),
        };
    }

    private static List<SectionSummaryItemModel> CreateProjectFinancialDetailsSummary(
        ProjectDetails projectDetails,
        CreateAction createAction,
        bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new(
                "Project progress more slowly or stall",
                SummaryAnswerHelper.ToYesNo(projectDetails.IsSupportRequired),
                createAction(nameof(ProjectController.Progress)),
                IsEditable: isEditable),
            new(
                "Funding required",
                SummaryAnswerHelper.ToYesNo(projectDetails.IsFundingRequired),
                createAction(nameof(ProjectController.RequiresFunding)),
                IsEditable: isEditable),
        };

        if (projectDetails.IsFundingRequired == true)
        {
            summary.AddRange(new[]
            {
                new SectionSummaryItemModel(
                    "How much funding",
                    SummaryAnswerHelper.ToEnum(projectDetails.RequiredFunding),
                    createAction(nameof(ProjectController.FundingAmount)),
                    IsEditable: isEditable),
                new SectionSummaryItemModel(
                    "Intention to make a profit",
                    SummaryAnswerHelper.ToYesNo(projectDetails.IsProfit),
                    createAction(nameof(ProjectController.Profit)),
                    IsEditable: isEditable),
            });
        }

        summary.Add(new(
            "Expected project start date",
            SummaryAnswerHelper.ToOnlyMonthAndYearDate(projectDetails.ExpectedStartDate),
            createAction(nameof(ProjectController.ExpectedStart)),
            IsEditable: isEditable));

        return summary;
    }

    private static string CreateProjectActionUrl(IUrlHelper urlHelper, FrontDoorProjectId projectId, string actionName, bool useWorkflowRedirection)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(ProjectController)).WithoutPrefix(),
            new { projectId = projectId.Value, redirect = useWorkflowRedirection ? nameof(ProjectController.CheckAnswers) : null });

        return action ?? string.Empty;
    }

    private static string CreateSiteActionUrl(IUrlHelper urlHelper, FrontDoorProjectId projectId, FrontDoorSiteId siteId, string actionName, bool useWorkflowRedirection)
    {
        if (siteId.IsNew)
        {
            return urlHelper.Action(
                nameof(SiteController.NewName),
                new ControllerName(nameof(SiteController)).WithoutPrefix(),
                new { projectId = projectId.Value, redirect = useWorkflowRedirection ? nameof(ProjectController.CheckAnswers) : null }) ?? string.Empty;
        }

        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(SiteController)).WithoutPrefix(),
            new { projectId = projectId.Value, siteId = siteId.Value, redirect = useWorkflowRedirection ? nameof(ProjectController.CheckAnswers) : null });

        return action ?? string.Empty;
    }
}
