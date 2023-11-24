namespace HE.Investments.Loans.Common.Tests;

public abstract class TestEntityBuilderBase<T>
{
    protected T Item { get; set; }

    public T Build() => Item;
}
