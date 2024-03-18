using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectGeographicFocusMapper : EnumMapper<ProjectGeographicFocus>
{
    protected override IDictionary<ProjectGeographicFocus, int?> Mapping => FrontDoorProjectEnumMapping.GeographicFocus;
}
