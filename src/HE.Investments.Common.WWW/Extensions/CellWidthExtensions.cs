using HE.Investments.Common.WWW.Enums;

namespace HE.Investments.Common.WWW.Extensions;

public static class CellWidthExtensions
{
    public static string GetWidthClass(this CellWidth width) => width switch
    {
        CellWidth.OneHalf => "govuk-!-width-one-half",
        CellWidth.OneThird => "govuk-!-width-one-third",
        CellWidth.OneQuarter => "govuk-!-width-one-quarter",
        CellWidth.OneFifth => "govuk-!-width-one-fifth",
        CellWidth.OneSixth => "govuk-!-width-one-sixth",
        CellWidth.OneEighth => "govuk-!-width-one-eighth",
        CellWidth.Undefined => string.Empty,
        _ => throw new ArgumentOutOfRangeException(nameof(width), width, null),
    };
}
