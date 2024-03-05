using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class AffordableHomesAmountMapper : EnumMapper<AffordableHomesAmount>
{
    protected override IDictionary<AffordableHomesAmount, int?> Mapping => new Dictionary<AffordableHomesAmount, int?>
    {
        { AffordableHomesAmount.OnlyAffordableHomes, (int)invln_FrontDoorAmountofAffordableHomes.Iwanttodeliver100affordablehomes },
        { AffordableHomesAmount.OpenMarkedAndAffordableHomes, (int)invln_FrontDoorAmountofAffordableHomes.Iwanttodeilveropenmarkethomesandalsoaffordablehomesabovetheamountrequiredbyplanning },
        { AffordableHomesAmount.OpenMarkedAndRequiredAffordableHomes, (int)invln_FrontDoorAmountofAffordableHomes.Iwanttodeliveropenmarkethomesandtheamountofaffordablehomesrequiredbyplanning },
        { AffordableHomesAmount.OnlyOpenMarketHomes, (int)invln_FrontDoorAmountofAffordableHomes.Ionlywanttodeliveropenmarkethomes },
        { AffordableHomesAmount.Unknown, (int)invln_FrontDoorAmountofAffordableHomes.Idonotknow },
        { AffordableHomesAmount.Undefined, null },
    };
}
