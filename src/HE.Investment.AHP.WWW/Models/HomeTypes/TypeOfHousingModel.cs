namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class TypeOfHousingModel
{
    public TypeOfHousingModel(string schemeName)
    {
        SchemeName = schemeName;
    }

    public string SchemeName { get; }

    public string HomeTypeName { get; set; }

    public HousingType HousingType { get; set; }
}
