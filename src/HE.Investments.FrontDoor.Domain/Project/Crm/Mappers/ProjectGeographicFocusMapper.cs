using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectGeographicFocusMapper : EnumMapper<ProjectGeographicFocus>
{
    private readonly IFrontDoorProjectEnumMapping _mapping;

    public ProjectGeographicFocusMapper(IFrontDoorProjectEnumMapping mapping)
    {
        _mapping = mapping;
    }

    protected override IDictionary<ProjectGeographicFocus, int?> Mapping => _mapping.GeographicFocus;
}
