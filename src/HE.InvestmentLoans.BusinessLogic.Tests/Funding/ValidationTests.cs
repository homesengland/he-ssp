using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Validation;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding;

[TestClass]
public class ValidationTests
{
    private ValidationResult _result;

    public ValidationTests()
    {
        _result = new ValidationResult();
    }

    [TestMethod]
    [DataRow("2,232")]
    [DataRow(",34")]
    [DataRow("non number")]
    public void Fail_when_gdv_is_in_incorrect_format(string value)
    {
        var model = ModelThatPassesValidation();

        model.GrossDevelopmentValue = value;

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    [DataRow("2")]
    [DataRow("2,34")]
    [DataRow("2.34")]
    public void Pass_when_gdv_is_in_correct_format(string value)
    {
        var model = ModelThatPassesValidation();

        model.GrossDevelopmentValue = value;

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    [DataRow("2,232")]
    [DataRow(",34")]
    [DataRow("non number")]
    public void Fail_when_total_costs_is_in_incorrect_format(string value)
    {
        var model = ModelThatPassesValidation();

        model.TotalCosts = value;

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    [DataRow("2")]
    [DataRow("2,34")]
    [DataRow("2.34")]
    public void Pass_when_total_costs_is_in_correct_format(string value)
    {
        var model = ModelThatPassesValidation();

        model.TotalCosts = value;

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Fail_when_private_sector_funding_result_was_not_provided()
    {
        var model = ModelThatPassesValidation();

        model.PrivateSectorFunding = "Yes";
        model.PrivateSectorFundingResult = string.Empty;

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    public void Pass_when_private_sector_funding_result_was_provided()
    {
        var model = ModelThatPassesValidation();

        model.PrivateSectorFunding = "Yes";
        model.PrivateSectorFundingResult = "Any text";

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Fail_when_private_sector_funding_reason_was_not_provided()
    {
        var model = ModelThatPassesValidation();

        model.PrivateSectorFunding = "No";
        model.PrivateSectorFundingReason = string.Empty;

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    public void Pass_when_private_sector_funding_reason_was_provided()
    {
        var model = ModelThatPassesValidation();

        model.PrivateSectorFunding = "No";
        model.PrivateSectorFundingReason = "Any text";

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Fail_when_abnormal_cost_info_was_not_provided()
    {
        var model = ModelThatPassesValidation();

        model.AbnormalCosts = "Yes";
        model.AbnormalCostsInfo = string.Empty;

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    public void Pass_when_abnormal_cost_info_was_not_provided()
    {
        var model = ModelThatPassesValidation();

        model.AbnormalCosts = "No";
        model.AbnormalCostsInfo = string.Empty;

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Pass_when_abnormal_cost_was_not_selected_and_more_info_was_not_provided()
    {
        var model = ModelThatPassesValidation();

        model.AbnormalCosts = "No";
        model.AbnormalCostsInfo = string.Empty;

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Fail_when_refinance_info_was_not_provided()
    {
        var model = ModelThatPassesValidation();

        model.Refinance = "refinance";
        model.RefinanceInfo = string.Empty;

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    public void Pass_when_refinance_info_was_provided()
    {
        var model = ModelThatPassesValidation();

        model.Refinance = "refinance";
        model.RefinanceInfo = "Any text";

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Pass_when_repay_was_selected_as_refinance_method()
    {
        var model = ModelThatPassesValidation();

        model.Refinance = "refund";
        model.RefinanceInfo = string.Empty;

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    [TestMethod]
    public void Fail_when_confirming_section_and_not_all_answers_not_provided()
    {
        var model = ModelThatPassesValidation();
        model.CheckAnswers = "Yes";

        WhenValidating(model);

        ThenValidationShouldFail();
    }

    [TestMethod]
    public void Pass_when_confirming_section_and_all_answers_are_provided()
    {
        var model = ModelThatPassesCheckAnswersValidation();
        model.CheckAnswers = "Yes";

        WhenValidating(model);

        ThenValidationShouldPass();
    }

    private FundingViewModel ModelThatPassesValidation()
    {
        return new FundingViewModel
        {
            CheckAnswers = "No",
        };
    }

    private FundingViewModel ModelThatPassesCheckAnswersValidation()
    {
        return new FundingViewModel
        {
            GrossDevelopmentValue = "2",
            TotalCosts = "2",
            PrivateSectorFunding = "Yes",
            PrivateSectorFundingResult = "Any text",
            AbnormalCosts = "No",
            AdditionalProjects = "Yes",
            Refinance = "refund",
            CheckAnswers = "Yes",
        };
    }

    private void WhenValidating(FundingViewModel model)
    {
        var validator = new FundingValidator();

        _result = validator.Validate(model, x => x.IncludeAllRuleSets());
    }

    private void ThenValidationShouldPass()
    {
        _result.Errors.Should().HaveCount(0);
    }

    private void ThenValidationShouldFail()
    {
        _result.Errors.Should().HaveCount(1);
    }
}
