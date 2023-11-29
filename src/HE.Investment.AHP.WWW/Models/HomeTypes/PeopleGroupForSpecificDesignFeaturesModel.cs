using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class PeopleGroupForSpecificDesignFeaturesModel : HomeTypeBasicModel
{
    public PeopleGroupForSpecificDesignFeaturesModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public PeopleGroupForSpecificDesignFeaturesModel()
        : this(string.Empty, string.Empty)
    {
    }

    public PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures { get; set; }
}
