namespace HE.Investments.Common.Contract.Enum;

[AttributeUsage(AttributeTargets.Field)]
public class HintAttribute : Attribute
{
    public HintAttribute(string text)
    {
        Text = text;
    }

    public string Text { get; private set; }
}
