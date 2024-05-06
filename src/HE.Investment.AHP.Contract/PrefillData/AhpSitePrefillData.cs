namespace HE.Investment.AHP.Contract.PrefillData;

public record AhpSitePrefillData(string? LocalAuthorityName)
{
    public static AhpSitePrefillData Empty => new(LocalAuthorityName: null);
}
