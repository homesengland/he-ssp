namespace HE.InvestmentLoans.Common.Tests.TestFramework;

public interface IRegisterDependency
{
    void RegisterDependency<TDependency>(TDependency dependency);
}
