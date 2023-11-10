namespace HE.Investments.Common.Extensions;

public static class ObjectExtensions
{
    public static bool IsProvided(this object? value)
    {
        return value is not null;
    }

    public static bool IsNotProvided(this object? value)
    {
        return value.IsProvided() is false;
    }
}
