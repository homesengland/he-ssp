namespace HE.Investment.AHP.Contract.HomeTypes;

public class HomeType
{
    public string? HomeTypeId { get; set; }

    public string? HomeTypeName { get; set; }

    // TODO: add all Home Type segments when implemented
    public HousingType HousingType { get; set; }
}
