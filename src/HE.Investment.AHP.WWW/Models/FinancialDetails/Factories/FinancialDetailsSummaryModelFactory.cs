using System.Globalization;
using System.Security.Policy;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;

public class FinancialDetailsSummaryModelFactory : IFinancialDetailsSummaryModelFactory
{
    private readonly IMediator _mediator;

    public FinancialDetailsSummaryModelFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<FinancialDetailsCheckAnswersModel> GetFinancialDetailsAndCreateSummary(
        IUrlHelper urlHelper,
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()), cancellationToken);

        var landValueItems = new List<SectionSummaryItemModel>
        {
            new SectionSummaryItemModel(
            "Purchase price",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.PurchasePrice) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.LandStatus)),
            null,
            true,
            true),
            new SectionSummaryItemModel(
            "Current value",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.LandValue) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.LandValue)),
            null,
            true,
            true),
            new SectionSummaryItemModel(
            "Public land",
            new List<string>() { financialDetails.IsSchemaOnPublicLand.HasValue ? financialDetails.IsSchemaOnPublicLand.Value ? CommonResponse.Yes : CommonResponse.No : "Not provided" },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.LandValue)),
            null,
            true,
            true),
        };

        var costItems = new List<SectionSummaryItemModel>
        {
        new SectionSummaryItemModel(
           "Purchase price",
           new List<string>() { GetCurrencyStringWithPrefix(financialDetails.PurchasePrice) },
           CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.LandStatus)),
           null,
           true,
           true),
        new SectionSummaryItemModel(
            "Works costs",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.ExpectedWorkCost) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.OtherApplicationCosts)),
            null,
            true,
            true),
        new SectionSummaryItemModel(
            "On costs",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.ExpectedOnCost) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.OtherApplicationCosts)),
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

        var contributionItems = new List<SectionSummaryItemModel>
        {
        new SectionSummaryItemModel(
           "Your contributions",
           new List<string>() { GetCurrencyStringWithPrefix(financialDetails.TotalExpectedContributions) },
           CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.Contributions)),
           null,
           true,
           true),
        new SectionSummaryItemModel(
            "Grants from other public bodies",
            new List<string>() { GetCurrencyStringWithPrefix(financialDetails.TotalRecievedGrands) },
            CreateFinancialDetailsActionUrl(urlHelper, applicationId.ToString(), nameof(FinancialDetailsController.Grants)),
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

        return new FinancialDetailsCheckAnswersModel(
            applicationId,
            financialDetails.ApplicationName,
            landValueItems,
            costItems,
            contributionItems,
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
}
