namespace HE.InvestmentLoans.BusinessLogic.User.Entities;
public class UserDetails
{
    public UserDetails(
        string? firstName,
        string? lastName,
        string? jobTitle,
        string? email,
        string? telephoneNumber,
        string? secondaryTelephoneNumber,
        bool? isTermsAndConditionsAccepted)
    {
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        Email = email;
        TelephoneNumber = telephoneNumber;
        SecondaryTelephoneNumber = secondaryTelephoneNumber;
        IsTermsAndConditionsAccepted = isTermsAndConditionsAccepted;
    }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public string? JobTitle { get; private set; }

    public string? Email { get; private set; }

    public string? TelephoneNumber { get; private set; }

    public string? SecondaryTelephoneNumber { get; private set; }

    public bool? IsTermsAndConditionsAccepted { get; private set; }

    public void ProvideUserDetails(
        string firstName,
        string lastName,
        string jobTitle,
        string telephoneNumber,
        string secondaryTelephoneNumber,
        string userEmail)
    {
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        Email = userEmail;
        TelephoneNumber = telephoneNumber;
        SecondaryTelephoneNumber = secondaryTelephoneNumber;
        IsTermsAndConditionsAccepted = true;
    }

    public bool IsProfileCompleted()
    {
        return !string.IsNullOrEmpty(FirstName) &&
                !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(JobTitle) &&
                !string.IsNullOrEmpty(TelephoneNumber) &&
                IsTermsAndConditionsAccepted == true;
    }
}
