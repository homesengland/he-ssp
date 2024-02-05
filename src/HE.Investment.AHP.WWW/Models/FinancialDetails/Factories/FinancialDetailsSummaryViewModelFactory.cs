using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Helpers;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
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
        bool isReadOnly,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFinancialCheckAnswersQuery(applicationId), cancellationToken);
        isReadOnly = isReadOnly || ApplicationStatusDivision.GetAllStatusesForReadonlyMode().Contains(result.ApplicationStatus);
        var landValueSectionSummary = GetLandValueSectionSummary(result.LandValue, applicationId, isReadOnly, urlHelper);
        var costsSectionSummary = GetCostsSectionSummary(result.TotalSchemeCost, applicationId, isReadOnly, urlHelper);
        var contributionsSectionSummary = GetContributionsSectionSummary(result.TotalContributions, applicationId, isReadOnly, urlHelper);

        return new FinancialDetailsCheckAnswersModel(
            Guid.Parse(applicationId.Value),
            result.ApplicationName,
            landValueSectionSummary,
            costsSectionSummary,
            contributionsSectionSummary,
            result.SectionStatus == SectionStatus.Completed ? IsSectionCompleted.Yes : IsSectionCompleted.Undefied,
            !isReadOnly);
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

    private static SectionSummaryViewModel GetLandValueSectionSummary(LandValueSummary landValueSummary, AhpApplicationId applicationId, bool isReadOnly, IUrlHelper urlHelper)
    {
        var landValueItems = new List<SectionSummaryItemModel>
        {
            new(
                "Purchase price",
                GetCurrencyStringWithPrefix(landValueSummary.PurchasePrice),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandStatus)),
                IsEditable: !isReadOnly),
            new(
                "Current value",
                GetCurrencyStringWithPrefix(landValueSummary.CurrentValue),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
                IsEditable: !isReadOnly),
            new(
                "Public land",
                landValueSummary.IsPublicLand == YesNoType.Undefined ? null : landValueSummary.IsPublicLand.GetDescription().ToOneElementList(),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
                IsEditable: !isReadOnly),
        };

        return new SectionSummaryViewModel("Land value", landValueItems);
    }

    private static SectionSummaryViewModel GetCostsSectionSummary(TotalSchemeCost totalSchemeCost, AhpApplicationId applicationId, bool isReadOnly, IUrlHelper urlHelper)
    {
        var costsItems = new List<SectionSummaryItemModel>
        {
            new(
                "Current value",
                GetCurrencyStringWithPrefix(totalSchemeCost.CurrentValue),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
                IsEditable: !isReadOnly),
            new(
                "Works costs",
                GetCurrencyStringWithPrefix(totalSchemeCost.WorkCosts),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.OtherApplicationCosts)),
                IsEditable: !isReadOnly),
            new(
                "On costs",
                GetCurrencyStringWithPrefix(totalSchemeCost.OnCosts),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.OtherApplicationCosts)),
                IsEditable: !isReadOnly),
            new(
                "Total scheme costs",
                GetCurrencyStringWithPrefix(totalSchemeCost.Total),
                string.Empty,
                IsEditable: false),
        };

        return new SectionSummaryViewModel("Total scheme costs", costsItems);
    }

    private static SectionSummaryViewModel GetContributionsSectionSummary(TotalContributions totalContributions, AhpApplicationId applicationId, bool isReadOnly, IUrlHelper urlHelper)
    {
        var contributionsItems = new List<SectionSummaryItemModel>
        {
            new(
                "Your contributions",
                GetCurrencyStringWithPrefix(totalContributions.YourContributions),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.Contributions)),
                IsEditable: !isReadOnly),
            new(
                "Grants from other public bodies",
                GetCurrencyStringWithPrefix(totalContributions.GrantsFromOtherPublicBodies),
                CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.Grants)),
                IsEditable: !isReadOnly),
            new(
                "Total contributions",
                GetCurrencyStringWithPrefix(totalContributions.Total),
                string.Empty,
                IsEditable: false),
        };

        return new SectionSummaryViewModel("Total contributions", contributionsItems);
    }
}
