namespace HE.Investments.TestsUtils.TestFramework;

public interface IRegisterDependency
{
    void RegisterDependency<TDependency>(TDependency dependency);
}
