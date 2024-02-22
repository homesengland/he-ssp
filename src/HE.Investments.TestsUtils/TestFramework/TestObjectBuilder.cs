using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace HE.Investments.TestsUtils.TestFramework;

public abstract class TestObjectBuilder<TBuilder, TItem> : TestObjectBuilder<TItem>
    where TBuilder : TestObjectBuilder<TItem>
    where TItem : class
{
    protected TestObjectBuilder(TItem item)
    {
        Item = item;
    }

    protected abstract TBuilder Builder { get; }

    protected TBuilder SetProperty<TProperty>(Expression<Func<TItem, TProperty>> propertyExpression, TProperty propertyValue)
    {
        var expression = (MemberExpression)propertyExpression.Body;
        var propertyName = expression.Member.Name;

        PrivatePropertySetter.SetPropertyWithNoSetter(Item, propertyName, propertyValue);

        return Builder;
    }
}

[SuppressMessage("Maintainability Rules", "SA1402", Justification = "The same class with different generic arguments")]
public class TestObjectBuilder<TItem>
    where TItem : class
{
    protected TItem Item { get; init; }

    public TItem Build()
    {
        return Item;
    }
}
