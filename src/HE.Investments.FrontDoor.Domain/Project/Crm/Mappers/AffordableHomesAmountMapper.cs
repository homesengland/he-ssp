using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class AffordableHomesAmountMapper : EnumMapper<AffordableHomesAmount>
{
    protected override IDictionary<AffordableHomesAmount, int?> Mapping => new Dictionary<AffordableHomesAmount, int?>
    {
        { AffordableHomesAmount.OnlyAffordableHomes, (int)he_Pipeline_he_amountofaffordablehomes.Iwanttodeliver100affordablehomes },
        { AffordableHomesAmount.OpenMarkedAndAffordableHomes, (int)he_Pipeline_he_amountofaffordablehomes.Iwanttodeliveropenmarkethomesandalsoaffordablehomesabovetheamountrequiredbyplanning },
        { AffordableHomesAmount.OpenMarkedAndRequiredAffordableHomes, (int)he_Pipeline_he_amountofaffordablehomes.Iwanttodeliveropenmarkethomesandtheamountofaffordablehomesrequiredbyplanning },
        { AffordableHomesAmount.OnlyOpenMarketHomes, (int)he_Pipeline_he_amountofaffordablehomes.Ionlywanttodeliveropenmarkethomes },
        { AffordableHomesAmount.Unknown, (int)he_Pipeline_he_amountofaffordablehomes.Idonotknow },
        { AffordableHomesAmount.Undefined, null },
    };
}
