namespace HE.Investments.FrontDoor.Domain.Site.Utilities;

public static class LocalAuthorityDivision
{
    public static bool IsLocalAuthorityNotAllowedForLoanApplication(string? localAuthorityName)
    {
        return localAuthorityName is "Bolton"
            or "Bury"
            or "Manchester"
            or "Oldham"
            or "Rochdale"
            or "Salford"
            or "Stockport"
            or "Tameside"
            or "Trafford "
            or "Wigan";
    }
}
