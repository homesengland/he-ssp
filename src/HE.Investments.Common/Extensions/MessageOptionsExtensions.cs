using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Extensions;

public static class MessageOptionsExtensions
{
    public static MessageOptions WithCalculation(this MessageOptions options, bool isCalculation)
    {
        return options | (isCalculation ? MessageOptions.Calculation : MessageOptions.None);
    }
}
