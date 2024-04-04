using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;

public class FinancialDetailsSummaryViewModelFactory : IFinancialDetailsSummaryViewModelFactory
{
    private readonly IMediator _mediator;

    public FinancialDetailsSummaryViewModelFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<FinancialDetailsCheckAnswersModel> GetFinancialDetailsAndCreateSummary(
        AhpApplicationId applicationId,
        IUrlHelper urlHelper,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFinancialCheckAnswersQuery(applicationId), cancellationToken);

        var landValueSectionSummary = GetLandValueSectionSummary(result.LandValue, applicationId, result.Application.IsEditable, urlHelper);
        var costsSectionSummary = GetCostsSectionSummary(result.TotalSchemeCost, applicationId, result.Application.IsEditable, urlHelper);
        var contributionsSectionSummary = GetContributionsSectionSummary(result.TotalContributions, applicationId, result.Application.IsEditable, urlHelper);

        return new FinancialDetailsCheckAnswersModel(
            Guid.Parse(applicationId.Value),
            result.Application.Name,
            landValueSectionSummary,
            costsSectionSummary,
            contributionsSectionSummary,
            result.SectionStatus == SectionStatus.Completed ? IsSectionCompleted.Yes : IsSectionCompleted.Undefied,
            result.Application.AllowedOperations);
    }

    private static IList<string> GetCurrencyStringWithPrefix(decimal? value)
    {
        return CurrencyHelper.DisplayPounds(value).ToOneElementList() ?? Array.Empty<string>();
    }

    private static string CreateFinancialDetailsActionUrl(IUrlHelper urlHelper, AhpApplicationId applicationId, string actionName, bool allowWcagDuplicate = false)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(FinancialDetailsController)).WithoutPrefix(),
            new { applicationId = applicationId.Value, redirect = nameof(FinancialDetailsController.CheckAnswers) });

        return $"{action}{(allowWcagDuplicate ? "#" : string.Empty)}";
    }

    private static SectionSummaryViewModel GetLandValueSectionSummary(LandValueSummary landValueSummary, AhpApplicationId applicationId, bool isEditable, IUrlHelper urlHelper)
    {
        var landValueItems = new List<SectionSummaryItemModel>
        {
            new(
                "Purchase price",
                GetCurrencyStringWithPrefix(landValueSummary.PurchasePrice),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandStatus)),
                IsEditable: isEditable),
            new(
                "Current value",
                GetCurrencyStringWithPrefix(landValueSummary.CurrentValue),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
                IsEditable: isEditable),
            new(
                "Public land",
                landValueSummary.IsPublicLand == YesNoType.Undefined ? null : landValueSummary.IsPublicLand.GetDescription().ToOneElementList(),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
                IsEditable: isEditable),
        };

        return new SectionSummaryViewModel("Land value", landValueItems);
    }

    private static SectionSummaryViewModel GetCostsSectionSummary(TotalSchemeCost totalSchemeCost, AhpApplicationId applicationId, bool isEditable, IUrlHelper urlHelper)
    {
        var costsItems = new List<SectionSummaryItemModel>
        {
            new(
                "Current value",
                GetCurrencyStringWithPrefix(totalSchemeCost.CurrentValue),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
                IsEditable: isEditable),
            new(
                "Works costs",
                GetCurrencyStringWithPrefix(totalSchemeCost.WorkCosts),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.OtherApplicationCosts)),
                IsEditable: isEditable),
            new(
                "On costs",
                GetCurrencyStringWithPrefix(totalSchemeCost.OnCosts),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.OtherApplicationCosts)),
                IsEditable: isEditable),
            new(
                "Total scheme costs",
                GetCurrencyStringWithPrefix(totalSchemeCost.Total),
                string.Empty,
                IsEditable: false),
        };

        return new SectionSummaryViewModel("Total scheme costs", costsItems);
    }

    private static SectionSummaryViewModel GetContributionsSectionSummary(TotalContributions totalContributions, AhpApplicationId applicationId, bool isEditable, IUrlHelper urlHelper)
    {
        var contributionsItems = new List<SectionSummaryItemModel>
        {
            new(
                "Your contributions",
                GetCurrencyStringWithPrefix(totalContributions.YourContributions),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.Contributions)),
                IsEditable: isEditable),
            new(
                "Grants from other public bodies",
                GetCurrencyStringWithPrefix(totalContributions.GrantsFromOtherPublicBodies),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.Grants)),
                IsEditable: isEditable),
            new(
                "Total contributions",
                GetCurrencyStringWithPrefix(totalContributions.Total),
                string.Empty,
                IsEditable: false),
        };

        return new SectionSummaryViewModel("Total contributions", contributionsItems);
    }
}
