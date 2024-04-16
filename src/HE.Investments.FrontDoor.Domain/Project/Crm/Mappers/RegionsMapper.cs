using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RegionsMapper : EnumMapper<RegionType>
{
    protected override IDictionary<RegionType, int?> Mapping => new Dictionary<RegionType, int?>
    {
        { RegionType.NorthEast, (int)he_pipeline_he_regionlocation.NorthEast },
        { RegionType.NorthWest, (int)he_pipeline_he_regionlocation.NorthWest },
        { RegionType.YorkshireAndTheHumber, (int)he_pipeline_he_regionlocation.YorkshireandtheHumber },
        { RegionType.EastMidlands, (int)he_pipeline_he_regionlocation.EastMidlands },
        { RegionType.WestMidlands, (int)he_pipeline_he_regionlocation.WestMidlands },
        { RegionType.EastOfEngland, (int)he_pipeline_he_regionlocation.EastofEngland },
        { RegionType.SouthEast, (int)he_pipeline_he_regionlocation.SouthEast },
        { RegionType.SouthWest, (int)he_pipeline_he_regionlocation.SouthWest },
        { RegionType.London, (int)he_pipeline_he_regionlocation.London },
    };

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
