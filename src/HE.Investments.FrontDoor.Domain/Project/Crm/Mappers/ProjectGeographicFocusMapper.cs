using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectGeographicFocusMapper : EnumMapper<ProjectGeographicFocus>
{
    protected override IDictionary<ProjectGeographicFocus, int?> Mapping => new Dictionary<ProjectGeographicFocus, int?>
    {
        { ProjectGeographicFocus.National, (int)invln_FrontDoorGeographicFocus.National },
        { ProjectGeographicFocus.Regional, (int)invln_FrontDoorGeographicFocus.Regional },
        { ProjectGeographicFocus.SpecificLocalAuthority, (int)invln_FrontDoorGeographicFocus.Specificlocalauthority },
        { ProjectGeographicFocus.Unknown, (int)invln_FrontDoorGeographicFocus.Idonotknow },
        { ProjectGeographicFocus.Undefined, null },
    };
}
