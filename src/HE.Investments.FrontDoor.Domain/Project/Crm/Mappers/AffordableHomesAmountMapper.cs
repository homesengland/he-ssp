using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class AffordableHomesAmountMapper : EnumMapper<AffordableHomesAmount>
{
    private readonly IFrontDoorProjectEnumMapping _mapping;

    public AffordableHomesAmountMapper(IFrontDoorProjectEnumMapping mapping)
    {
        _mapping = mapping;
    }

    protected override IDictionary<AffordableHomesAmount, int?> Mapping => _mapping.AffordableHomes;
}
