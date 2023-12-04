using System.Globalization;
using System.Security.Policy;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

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
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()), cancellationToken);

        var landValueSectionSummary = GetLandValueSectionSummary(financialDetails, urlHelper);
        var costsSectionSummary = GetCostsSectionSummary(financialDetails, urlHelper);
        var contributionsSectionSummary = GetContributionsSectionSummary(financialDetails, urlHelper);

        return new FinancialDetailsCheckAnswersModel(
            Guid.Parse(applicationId),
            financialDetails.ApplicationName,
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

    private SectionSummaryViewModel GetLandValueSectionSummary(Contract.FinancialDetails.FinancialDetails financialDetails, IUrlHelper urlHelper)
    {
        var landValueItems = new List<SectionSummaryItemModel>
        {
            new SectionSummaryItemModel(
            "Purchase price",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.PurchasePrice) },
            CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.LandStatus)),
            null,
            true,
            true),
            new SectionSummaryItemModel(
            "Current value",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.LandValue) },
            CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.LandValue)),
            null,
            true,
            true),
            new SectionSummaryItemModel(
            "Public land",
            new List<string>() { financialDetails.IsSchemaOnPublicLand.HasValue ? financialDetails.IsSchemaOnPublicLand.Value ? CommonResponse.Yes : CommonResponse.No : "Not provided" },
            CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.LandValue)),
            null,
            true,
            true),
        };

        var result = new SectionSummaryViewModel("Land value", landValueItems);

        return result;
    }

    private SectionSummaryViewModel GetCostsSectionSummary(Contract.FinancialDetails.FinancialDetails financialDetails, IUrlHelper urlHelper)
    {
        var costsItems = new List<SectionSummaryItemModel>
        {
        new SectionSummaryItemModel(
           "Purchase price",
           new List<string>() { GetCurrencyStringWithPrefix(financialDetails.PurchasePrice) },
           CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.LandStatus)),
           null,
           true,
           true),
        new SectionSummaryItemModel(
            "Works costs",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.ExpectedWorkCost) },
            CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.OtherApplicationCosts)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "On costs",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.ExpectedOnCost) },
            CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.OtherApplicationCosts)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "Total scheme costs",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.TotalExpectedCosts) },
            string.Empty,
            null,
            false,
            true),
        };

        var result = new SectionSummaryViewModel("Total scheme costs", costsItems);

        return result;
    }

    private SectionSummaryViewModel GetContributionsSectionSummary(Contract.FinancialDetails.FinancialDetails financialDetails, IUrlHelper urlHelper)
    {
        var contributionsItems = new List<SectionSummaryItemModel>
        {
        new SectionSummaryItemModel(
           "Your contributions",
           new List<string>() { GetCurrencyStringWithPrefix(financialDetails.TotalExpectedContributions) },
           CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.Contributions)),
           null,
           true,
           true),
        new SectionSummaryItemModel(
            "Grants from other public bodies",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.TotalRecievedGrands) },
            CreateFinancialDetailsActionUrl(urlHelper, financialDetails.ApplicationId.ToString(), nameof(FinancialDetailsController.Grants)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "Total contributions",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.TotalExpectedContributions + financialDetails.TotalRecievedGrands) },
            string.Empty,
            null,
            false,
            true),
        };

        var result = new SectionSummaryViewModel("Total contributions", contributionsItems);

        return result;
    }
}
