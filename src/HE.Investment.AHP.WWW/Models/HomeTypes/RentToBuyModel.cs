using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RentToBuyModel : HomeTypeBasicModel
{
    public RentToBuyModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public RentToBuyModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MarketValue { get; set; }

    public string? MarketRentPerWeek { get; set; }

    public string? RentPerWeek { get; set; }

    public string? ProspectiveRentAsPercentageOfMarketRent { get; set; }

    public YesNoType TargetRentExceedMarketRent { get; set; }
}
