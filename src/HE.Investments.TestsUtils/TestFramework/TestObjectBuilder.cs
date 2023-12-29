namespace HE.Investments.TestsUtils.TestFramework;

public class TestObjectBuilder<TItem>
    where TItem : class
{
    protected TItem Item { get; set; }

    public TItem Build()
    {
        return Item;
    }
}
