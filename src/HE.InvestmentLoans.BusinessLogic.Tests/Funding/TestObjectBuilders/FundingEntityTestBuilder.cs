using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;

public class FundingEntityTestBuilder
{
    private readonly FundingEntity _item;

    private FundingEntityTestBuilder(FundingEntity fundingEntity)
    {
        _item = fundingEntity;
    }

    public static FundingEntityTestBuilder New() =>
        new(new FundingEntity(LoanApplicationIdTestData.LoanApplicationIdOne, null, null, null, null, null, null, SectionStatus.NotStarted));

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

    public FundingEntity Build()
    {
        return _item;
    }
}
