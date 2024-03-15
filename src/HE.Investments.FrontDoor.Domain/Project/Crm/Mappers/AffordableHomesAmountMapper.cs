using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class AffordableHomesAmountMapper : EnumMapper<AffordableHomesAmount>
{
    protected override IDictionary<AffordableHomesAmount, int?> Mapping => FrontDoorProjectEnumMapping.AffordableHomes;
}
