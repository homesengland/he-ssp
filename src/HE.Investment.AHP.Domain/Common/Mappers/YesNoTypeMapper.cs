using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.Common.Mappers;

public static class YesNoTypeMapper
{
    public static bool? Map(YesNoType? value)
    {
        return value switch
        {
            YesNoType.Yes => true,
            YesNoType.No => false,
            _ => null,
        };
    }

    public static YesNoType Map(bool? value)
    {
        return value switch
        {
            true => YesNoType.Yes,
            false => YesNoType.No,
            _ => YesNoType.Undefined,
        };
    }
}
