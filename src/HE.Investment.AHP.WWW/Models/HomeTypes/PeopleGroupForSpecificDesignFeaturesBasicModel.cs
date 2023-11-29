using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class PeopleGroupForSpecificDesignFeaturesBasicModel : HomeTypeBasicModel
{
    public PeopleGroupForSpecificDesignFeaturesBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public PeopleGroupForSpecificDesignFeaturesBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures { get; set; }
}
