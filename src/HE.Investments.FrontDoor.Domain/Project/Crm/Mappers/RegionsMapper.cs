using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RegionsMapper : EnumMapper<RegionType>
{
    protected override IDictionary<RegionType, int?> Mapping => new Dictionary<RegionType, int?>
    {
        { RegionType.NorthEast, (int)invln_FrontDoorRegion.NorthEast },
        { RegionType.NorthWest, (int)invln_FrontDoorRegion.NorthWest },
        { RegionType.YorkshireAndTheHumber, (int)invln_FrontDoorRegion.YorkshireandtheHumber },
        { RegionType.EastMidlands, (int)invln_FrontDoorRegion.EastMidlands },
        { RegionType.WestMidlands, (int)invln_FrontDoorRegion.WestMidlands },
        { RegionType.EastOfEngland, (int)invln_FrontDoorRegion.EastofEngland },
        { RegionType.SouthEast, (int)invln_FrontDoorRegion.SouthEast },
        { RegionType.SouthWest, (int)invln_FrontDoorRegion.SouthWest },
        { RegionType.London, (int)invln_FrontDoorRegion.London },
    };

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
