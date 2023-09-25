namespace HE.InvestmentLoans.Common.Tests.TestFramework;

public interface IDependencyTestBuilder<TDependency>
{
    public TDependency Build();
}
