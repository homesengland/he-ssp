namespace HE.Investments.Account.Contract.User;

public class UserProfileDetails
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? JobTitle { get; set; }

    public string? Email { get; set; }

    public string? TelephoneNumber { get; set; }

    public string? SecondaryTelephoneNumber { get; set; }

    public bool? IsTermsAndConditionsAccepted { get; set; }
}
