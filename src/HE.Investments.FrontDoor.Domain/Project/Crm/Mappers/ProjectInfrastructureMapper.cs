using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectInfrastructureMapper : EnumMapper<InfrastructureType>
{
    private readonly IFrontDoorProjectEnumMapping _mapping;

    public ProjectInfrastructureMapper(IFrontDoorProjectEnumMapping mapping)
    {
        _mapping = mapping;
    }

    protected override IDictionary<InfrastructureType, int?> Mapping => _mapping.Infrastructure;

    public ProjectInfrastructure Map(IList<int> values)
    {
        return values.Count == 0
            ? ProjectInfrastructure.Empty()
            : new ProjectInfrastructure(values.Select(x => ToDomain(x)!.Value).ToList());
    }

    public List<int> Map(ProjectInfrastructure value)
    {
        return value.Values.Select(x => ToDto(x)!.Value).ToList();
    }
}
