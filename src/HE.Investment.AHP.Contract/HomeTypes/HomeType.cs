using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public class HomeType
{
    public string? HomeTypeId { get; set; }

    public string? HomeTypeName { get; set; }

    public HousingType HousingType { get; set; }

    public HomeTypeConditionals Conditionals { get; set; }
}
