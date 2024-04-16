using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectInfrastructureMapper : EnumMapper<InfrastructureType>
{
    protected override IDictionary<InfrastructureType, int?> Mapping => new Dictionary<InfrastructureType, int?>
    {
        { InfrastructureType.SitePreparation, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Sitepreparation },
        { InfrastructureType.Enabling, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Enabling },
        { InfrastructureType.PhysicalInfrastructure, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Physicalinfrastructure },
        { InfrastructureType.Other, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Other },
        { InfrastructureType.IDoNotKnow, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Idonotknow },
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
