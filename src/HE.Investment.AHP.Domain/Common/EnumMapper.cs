namespace HE.Investment.AHP.Domain.Common;

public static class EnumMapper
{
    public static TResultEnum MapByName<TInputEnum, TResultEnum>(TInputEnum? input)
        where TInputEnum : struct, Enum
        where TResultEnum : struct, Enum
    {
        return Enum.Parse<TResultEnum>(input?.ToString() ?? "Undefined");
    }
}
