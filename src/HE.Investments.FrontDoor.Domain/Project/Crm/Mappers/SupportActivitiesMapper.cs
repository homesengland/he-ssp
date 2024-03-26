using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class SupportActivitiesMapper : EnumMapper<SupportActivityType>
{
    private readonly IFrontDoorProjectEnumMapping _mapping;

    public SupportActivitiesMapper(IFrontDoorProjectEnumMapping mapping)
    {
        _mapping = mapping;
    }

    protected override IDictionary<SupportActivityType, int?> Mapping => _mapping.ActivityType;

    public SupportActivities Map(IList<int> values)
    {
        return values.Count == 0
            ? SupportActivities.Empty()
            : new SupportActivities(values.Select(x => ToDomain(x)!.Value).ToList());
    }

    public List<int> Map(SupportActivities value)
    {
        return value.Values.Select(x => ToDto(x)!.Value).ToList();
    }
}
