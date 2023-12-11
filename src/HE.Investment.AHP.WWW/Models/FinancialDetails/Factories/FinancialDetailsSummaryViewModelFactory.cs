using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
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
        string applicationId,
        IUrlHelper urlHelper,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFinancialCheckAnswersQuery(applicationId), cancellationToken);

        var landValueSectionSummary = GetLandValueSectionSummary(result.LandValue, applicationId, urlHelper);
        var costsSectionSummary = GetCostsSectionSummary(result.TotalSchemeCost, applicationId, urlHelper);
        var contributionsSectionSummary = GetContributionsSectionSummary(result.TotalContributions, applicationId, urlHelper);

        return new FinancialDetailsCheckAnswersModel(
            Guid.Parse(applicationId),
            result.ApplicationName,
            landValueSectionSummary,
            costsSectionSummary,
            contributionsSectionSummary,
            null);
    }

    private string GetCurrencyStringWithPrefix(decimal? value)
    {
        return "£" + value.ToWholeNumberString() ?? "Not provided";
    }

    private string CreateFinancialDetailsActionUrl(IUrlHelper urlHelper, string applicationId, string actionName, bool allowWcagDuplicate = false)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(FinancialDetailsController)).WithoutPrefix(),
            new { applicationId, redirect = nameof(FinancialDetailsController.CheckAnswers) });

        return $"{action}{(allowWcagDuplicate ? "#" : string.Empty)}";
    }

    private SectionSummaryViewModel GetLandValueSectionSummary(LandValueSummary landValueSummary, string applicationId, IUrlHelper urlHelper)
    {
        var landValueItems = new List<SectionSummaryItemModel>
        {
            new SectionSummaryItemModel(
            "Purchase price",
            new List<string>() { GetCurrencyStringWithPrefix(landValueSummary.PurchasePrice) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandStatus)),
            null,
            true,
            true),
            new SectionSummaryItemModel(
            "Current value",
            new List<string>() { GetCurrencyStringWithPrefix(landValueSummary.CurrentValue) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
            null,
            true,
            true),
            new SectionSummaryItemModel(
            "Public land",
            new List<string>() { landValueSummary.IsPublicLand.HasValue ? landValueSummary.IsPublicLand.Value ? CommonResponse.Yes : CommonResponse.No : "Not provided" },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandValue)),
            null,
            true,
            true),
        };

        var result = new SectionSummaryViewModel("Land value", landValueItems);

        return result;
    }

    private SectionSummaryViewModel GetCostsSectionSummary(TotalSchemeCost totalSchemeCost, string applicationId, IUrlHelper urlHelper)
    {
        var costsItems = new List<SectionSummaryItemModel>
        {
        new SectionSummaryItemModel(
           "Purchase price",
           new List<string>() { GetCurrencyStringWithPrefix(totalSchemeCost.PurchasePrice) },
           CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.LandStatus)),
           null,
           true,
           true),
        new SectionSummaryItemModel(
            "Works costs",
            new List<string>() { GetCurrencyStringWithPrefix(totalSchemeCost.WorkCosts) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.OtherApplicationCosts)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "On costs",
            new List<string>() { GetCurrencyStringWithPrefix(totalSchemeCost.OnCosts) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.OtherApplicationCosts)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "Total scheme costs",
            new List<string>() { GetCurrencyStringWithPrefix(totalSchemeCost.Total) },
            string.Empty,
            null,
            false,
            true),
        };

        var result = new SectionSummaryViewModel("Total scheme costs", costsItems);

        return result;
    }

    private SectionSummaryViewModel GetContributionsSectionSummary(TotalContributions totalContributions, string applicationId, IUrlHelper urlHelper)
    {
        var contributionsItems = new List<SectionSummaryItemModel>
        {
        new SectionSummaryItemModel(
           "Your contributions",
           new List<string>() { GetCurrencyStringWithPrefix(totalContributions.YourContributions) },
           CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.Contributions)),
           null,
           true,
           true),
        new SectionSummaryItemModel(
            "Grants from other public bodies",
            new List<string>() { GetCurrencyStringWithPrefix(totalContributions.GrantsFromOtherPublicBodies) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId, nameof(FinancialDetailsController.Grants)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "Total contributions",
            new List<string>() { GetCurrencyStringWithPrefix(totalContributions.Total) },
            string.Empty,
            null,
            false,
            true),
        };

        var result = new SectionSummaryViewModel("Total contributions", contributionsItems);

        return result;
    }
}
