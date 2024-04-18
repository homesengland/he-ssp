using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class NationalDesignGuideMapper : EnumMapper<NationalDesignGuidePriority>
{
    protected override IDictionary<NationalDesignGuidePriority, int?> Mapping => new Dictionary<NationalDesignGuidePriority, int?>
    {
        { NationalDesignGuidePriority.Context, (int)invln_NationalDesignGuideelements.AContext },
        { NationalDesignGuidePriority.Identity, (int)invln_NationalDesignGuideelements.BIdentity },
        { NationalDesignGuidePriority.BuiltForm, (int)invln_NationalDesignGuideelements.CBuiltform },
        { NationalDesignGuidePriority.Movement, (int)invln_NationalDesignGuideelements.DMovement },
        { NationalDesignGuidePriority.Nature, (int)invln_NationalDesignGuideelements.ENature },
        { NationalDesignGuidePriority.PublicSpaces, (int)invln_NationalDesignGuideelements.FPublicspaces },
        { NationalDesignGuidePriority.Uses, (int)invln_NationalDesignGuideelements.GUses },
        { NationalDesignGuidePriority.HomesAndBuildings, (int)invln_NationalDesignGuideelements.HHomesandbuildings },
        { NationalDesignGuidePriority.Resources, (int)invln_NationalDesignGuideelements.IResources },
        { NationalDesignGuidePriority.Lifespan, (int)invln_NationalDesignGuideelements.JLifespan },
        { NationalDesignGuidePriority.NoneOfTheAbove, (int)invln_NationalDesignGuideelements.Noneoftheabove },
    };
}
