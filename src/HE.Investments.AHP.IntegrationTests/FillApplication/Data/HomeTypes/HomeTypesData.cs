namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data.HomeTypes;

public class HomeTypesData
{
    public HomeTypesData()
    {
        General = new GeneralHomeTypeData();
        Disabled = new HomesForDisabledPeopleData();
    }

    public GeneralHomeTypeData General { get; }

    public HomesForDisabledPeopleData Disabled { get; private set; }

    public void DuplicateGeneralAsDisabled(string homeTypeId, string homeTypeName)
    {
        Disabled = HomesForDisabledPeopleData.DuplicateFromGeneralHomeType(homeTypeId, homeTypeName, General);
    }
}
