using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HousingTypeModel
{
    public HousingTypeModel(string financialSchemeName)
    {
        FinancialSchemeName = financialSchemeName;
    }

    // Required by Model Binder
    public HousingTypeModel()
    {
    }

    public string FinancialSchemeName { get; set; }

    public string HomeTypeName { get; set; }

    public HousingType HousingType { get; set; }
}
