using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class SiteUsingModernMethodsOfConstructionMapper : EnumMapper<SiteUsingModernMethodsOfConstruction>
{
    protected override IDictionary<SiteUsingModernMethodsOfConstruction, int?> Mapping =>
        new Dictionary<SiteUsingModernMethodsOfConstruction, int?>
        {
            { SiteUsingModernMethodsOfConstruction.Yes, (int)invln_MMCuse.YesImusingMMCforallthehousingonthissite },
            { SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes, (int)invln_MMCuse.Yesbutonlyforsomeofthehousingonasite },
            { SiteUsingModernMethodsOfConstruction.No, (int)invln_MMCuse.NoImnotusinganyMMConthissite },
        };
}
