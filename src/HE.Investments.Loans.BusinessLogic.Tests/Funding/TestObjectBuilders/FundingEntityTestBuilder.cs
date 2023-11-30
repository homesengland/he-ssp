using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Funding.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.TestObjectBuilders;

public class FundingEntityTestBuilder
{
    private readonly FundingEntity _item;

    private FundingEntityTestBuilder(FundingEntity fundingEntity)
    {
        _item = fundingEntity;
    }

    public static FundingEntityTestBuilder New() =>
        new(new FundingEntity(
            LoanApplicationIdTestData.LoanApplicationIdOne,
            null,
            null,
            null,
            null,
            null,
            null,
            SectionStatus.NotStarted,
            ApplicationStatus.Draft));

    public FundingEntityTestBuilder WithGrossDevelopmentValue()
    {
        _item.ProvideGrossDevelopmentValue(GrossDevelopmentValueTestData.GrossDevelopmentValue5);
        return this;
    }

    public FundingEntityTestBuilder WithEstimatedTotalCosts()
    {
        _item.ProvideEstimatedTotalCosts(EstimatedTotalCostsTestData.EstimatedTotalCosts5);
        return this;
    }

    public FundingEntityTestBuilder WithAbnormalCosts()
    {
        _item.ProvideAbnormalCosts(AbnormalCostsTestData.AbnormalCostsTrue);
        return this;
    }

    public FundingEntityTestBuilder WithPrivateSectorFunding()
    {
        _item.ProvidePrivateSectorFunding(PrivateSectorFundingTestData.PrivateSectorFundingFalse);
        return this;
    }

    public FundingEntityTestBuilder WithRepaymentSystem()
    {
        _item.ProvideRepaymentSystem(RepaymentSystemTestData.RepaymentSystemRefinance);
        return this;
    }

    public FundingEntityTestBuilder WithAdditionalProjects()
    {
        _item.ProvideAdditionalProjects(AdditionalProjectsTestData.AdditionalProjectsTrue);
        return this;
    }

    public FundingEntityTestBuilder WithAllDataProvided()
    {
        WithGrossDevelopmentValue();
        WithEstimatedTotalCosts();
        WithAbnormalCosts();
        WithPrivateSectorFunding();
        WithRepaymentSystem();
        WithAdditionalProjects();

        return this;
    }

    public FundingEntity Build()
    {
        return _item;
    }
}
