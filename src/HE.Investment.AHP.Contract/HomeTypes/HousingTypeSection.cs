namespace HE.Investment.AHP.Contract.HomeTypes;

public class HousingTypeSection
{
    public HousingTypeSection(string homeTypeId)
    {
        HomeTypeId = homeTypeId;
    }

    public string HomeTypeId { get; set; }

    public string? HomeTypeName { get; set; }

    public HousingType HousingType { get; set; }
}
