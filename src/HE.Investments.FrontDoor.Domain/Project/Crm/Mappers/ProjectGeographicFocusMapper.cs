using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectGeographicFocusMapper : EnumMapper<ProjectGeographicFocus>
{
    protected override IDictionary<ProjectGeographicFocus, int?> Mapping => new Dictionary<ProjectGeographicFocus, int?>
    {
        { ProjectGeographicFocus.National, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.National },
        { ProjectGeographicFocus.Regional, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.Regional },
        { ProjectGeographicFocus.SpecificLocalAuthority, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.Specificlocalauthority },
        { ProjectGeographicFocus.Unknown, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.Idonotknow },
        { ProjectGeographicFocus.Undefined, null },
    };
}
