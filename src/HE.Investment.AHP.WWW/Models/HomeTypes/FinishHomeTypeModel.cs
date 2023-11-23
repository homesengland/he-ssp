using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class FinishHomeTypeModel : HomeTypeModelBase
{
    public FinishHomeTypeModel(string applicationName)
        : base(applicationName)
    {
    }

    public FinishHomeTypeModel()
        : base(string.Empty)
    {
    }

    public FinishHomeTypesAnswer FinishAnswer { get; set; }
}
