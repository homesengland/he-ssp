using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RegionsMapper : EnumMapper<RegionType>
{
    private readonly IFrontDoorProjectEnumMapping _mapping;

    public RegionsMapper(IFrontDoorProjectEnumMapping mapping)
    {
        _mapping = mapping;
    }

    protected override IDictionary<RegionType, int?> Mapping => _mapping.RegionType;

    public Regions Map(IList<int> values)
    {
        return values.Count == 0
            ? Regions.Empty()
            : new Regions(values.Select(x => ToDomain(x)!.Value).ToList());
    }

    public List<int> Map(Regions value)
    {
        return value.Values.Select(x => ToDto(x)!.Value).ToList();
    }
}
