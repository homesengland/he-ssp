using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class SupportActivitiesMapper : EnumMapper<SupportActivityType>
{
    protected override IDictionary<SupportActivityType, int?> Mapping => new Dictionary<SupportActivityType, int?>
    {
        { SupportActivityType.AcquiringLand, (int)invln_FrontDoorActivitiesinProject.Acquiringland },
        { SupportActivityType.DevelopingHomes, (int)invln_FrontDoorActivitiesinProject.Developinghomesincludinganyminorsiterelatedinfrastructure },
        { SupportActivityType.ProvidingInfrastructure, (int)invln_FrontDoorActivitiesinProject.Providinginfrastructure },
        { SupportActivityType.ManufacturingHomesWithinFactory, (int)invln_FrontDoorActivitiesinProject.Manufacturinghomeswithinafactory },
        { SupportActivityType.SellingLandToHomesEngland, (int)invln_FrontDoorActivitiesinProject.SellinglandtoHomesEngland },
        { SupportActivityType.Other, (int)invln_FrontDoorActivitiesinProject.Other },
    };
}
