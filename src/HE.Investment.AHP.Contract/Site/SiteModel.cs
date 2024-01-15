namespace HE.Investment.AHP.Contract.Site;

public class SiteModel
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool? Section106GeneralAgreement { get; set; }

    public bool? Section106AffordableHomes { get; set; }

    public bool? Section106OnlyAffordableHomes { get; set; }

    public bool? Section106AdditionalAffordableHomes { get; set; }

    public bool? Section106CapitalFundingEligibility { get; set; }

    public string? Section106ConfirmationFromLocalAuthority { get; set; }
}
