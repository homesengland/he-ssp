namespace HE.InvestmentLoans.Common.Tests;

public abstract class TestEntityBuilderBase<T>
{
    protected T Item { get; }

    public T Build() => Item;
}
