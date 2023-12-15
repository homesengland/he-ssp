using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class AffordableRentModel : HomeTypeBasicModel
{
    public AffordableRentModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public AffordableRentModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MarketValue { get; set; }

    public string? MarketRent { get; set; }

    public string? ProspectiveRent { get; set; }

    public string? ProspectiveRentAsPercentageOfMarketRent { get; set; }

    public YesNoType TargetRentExceedMarketRent { get; set; }
}
