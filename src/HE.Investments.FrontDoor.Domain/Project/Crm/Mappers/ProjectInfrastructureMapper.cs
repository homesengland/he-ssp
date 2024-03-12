using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectInfrastructureMapper : EnumMapper<InfrastructureType>
{
    protected override IDictionary<InfrastructureType, int?> Mapping => new Dictionary<InfrastructureType, int?>
    {
        { InfrastructureType.SitePreparation, (int)invln_FrontDoorInfrastructureDelivered.Sitepreparation },
        { InfrastructureType.Enabling, (int)invln_FrontDoorInfrastructureDelivered.Enabling },
        { InfrastructureType.PhysicalInfrastructure, (int)invln_FrontDoorInfrastructureDelivered.Physicalinfrastructure },
        { InfrastructureType.Other, (int)invln_FrontDoorInfrastructureDelivered.Other },
        { InfrastructureType.IDoNotKnow, (int)invln_FrontDoorInfrastructureDelivered.Idonotknow },
    };

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
