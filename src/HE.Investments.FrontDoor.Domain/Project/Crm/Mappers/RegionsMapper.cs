using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RegionsMapper : EnumMapper<RegionType>
{
    protected override IDictionary<RegionType, int?> Mapping => FrontDoorProjectEnumMapping.RegionType;

    public Regions Map(IList<int> values)
    {
        return values.Count == 0
            ? Regions.Empty()
            : new Regions(values.Select(x => new RegionsMapper().ToDomain(x)!.Value).ToList());
    }

    public List<int> Map(Regions value)
    {
        return value.Values.Select(x => new RegionsMapper().ToDto(x)!.Value).ToList();
    }
}
