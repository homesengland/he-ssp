namespace HE.Investments.Common.Utils;

public enum NumberParseResult
{
    SuccessfullyParsed,
    ValueMissing,
    ValueNotANumber,
    ValueInvalidPrecision,
    ValueTooHigh,
    ValueTooLow,
}
