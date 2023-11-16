using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public class AmountValidator
{
    public static (bool Result, int? Value) Validate(string input)
    {
        if (!int.TryParse(input, out var value) || value < 0 || value > 999999999)
        {
            return (false, null);
        }

        return (true, value);
    }
}
