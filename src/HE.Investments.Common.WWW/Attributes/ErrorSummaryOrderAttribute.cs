namespace HE.Investments.Common.WWW.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class ErrorSummaryOrderAttribute : Attribute
{
    public ErrorSummaryOrderAttribute(int order)
    {
        Order = order;
    }

    public int Order { get; }
}
