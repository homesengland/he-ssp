namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class HomeTypesData
{
    public HomeTypesData()
    {
        General = new GeneralHomeTypeData();
    }

    public GeneralHomeTypeData General { get; }
}
