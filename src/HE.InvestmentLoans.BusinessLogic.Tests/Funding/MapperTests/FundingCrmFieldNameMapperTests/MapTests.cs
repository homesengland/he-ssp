using HE.InvestmentLoans.BusinessLogic.Funding.Mappers;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.CRM.Model;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.MapperTests.FundingCrmFieldNameMapperTests;

public class MapTests
{
    [Fact]
    public void ShouldReturnExternalStatusAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsGetEmpty()
    {
        // given
        var getEmpty = FundingFieldsSet.GetEmpty;

        // when
        var result = FundingCrmFieldNameMapper.Map(getEmpty);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnProjectGdvAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsGdv()
    {
        // given
        var projectGdv = FundingFieldsSet.GDV;

        // when
        var result = FundingCrmFieldNameMapper.Map(projectGdv);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ProjectGDV).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnProjectEstimatedTotalCostAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsEstimatedTotalCosts()
    {
        // given
        var estimatedTotalCosts = FundingFieldsSet.EstimatedTotalCosts;

        // when
        var result = FundingCrmFieldNameMapper.Map(estimatedTotalCosts);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Projectestimatedtotalcost).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void
        ShouldReturnPrivateSectorApproachWithPrivateSectorApproachInformationAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsPrivateSectorFunding()
    {
        // given
        var privateSectorFunding = FundingFieldsSet.PrivateSectorFunding;

        // when
        var result = FundingCrmFieldNameMapper.Map(privateSectorFunding);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Privatesectorapproach).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Privatesectorapproachinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnProjectAbnormalCostsWithProjectAbnormalCostsInformationAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsAbnormalCosts()
    {
        // given
        var abnormalCosts = FundingFieldsSet.AbnormalCosts;

        // when
        var result = FundingCrmFieldNameMapper.Map(abnormalCosts);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Projectabnormalcosts).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectabnormalcostsinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnRefinanceRepaymentWithRefinanceRepaymentDetailsAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsRepaymentSystem()
    {
        // given
        var repaymentSystem = FundingFieldsSet.RepaymentSystem;

        // when
        var result = FundingCrmFieldNameMapper.Map(repaymentSystem);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Refinancerepayment).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Refinancerepaymentdetails).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAdditionalProjectsAndFundingDetailsCompletionStatusWhenFundingFieldsSetIsAdditionalProjects()
    {
        // given
        var additionalProjects = FundingFieldsSet.AdditionalProjects;

        // when
        var result = FundingCrmFieldNameMapper.Map(additionalProjects);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_Additionalprojects).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAllFieldsWhenFundingFieldsSetIsGetAllFields()
    {
        // given
        var getAllFields = FundingFieldsSet.GetAllFields;

        // when
        var result = FundingCrmFieldNameMapper.Map(getAllFields);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_ProjectGDV).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectestimatedtotalcost).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectabnormalcosts).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectabnormalcostsinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Privatesectorapproach).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Privatesectorapproachinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Additionalprojects).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Refinancerepayment).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Refinancerepaymentdetails).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }

    [Fact]
    public void ShouldReturnAllFieldsExceptExternalStatusWhenFundingFieldsSetIsSaveAllFields()
    {
        // given
        var saveAllFields = FundingFieldsSet.SaveAllFields;

        // when
        var result = FundingCrmFieldNameMapper.Map(saveAllFields);

        // then
        result.Should()
            .ContainAll(
                $"{nameof(invln_Loanapplication.invln_ProjectGDV).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectestimatedtotalcost).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectabnormalcosts).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Projectabnormalcostsinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Privatesectorapproach).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Privatesectorapproachinformation).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Additionalprojects).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Refinancerepayment).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_Refinancerepaymentdetails).ToLowerInvariant()},",
                $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}");
    }
}
