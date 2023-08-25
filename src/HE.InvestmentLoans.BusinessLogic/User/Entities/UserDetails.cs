using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.BusinessLogic.User.Entities;
public class UserDetails
{
    public UserDetails(
        string? firstName,
        string? surname,
        string? jobTitle,
        string? email,
        string? telephoneNumber,
        string? secondaryTelephoneNumber,
        bool? isTermsAndConditionsAccepted)
    {
        FirstName = firstName;
        Surname = surname;
        JobTitle = jobTitle;
        Email = email;
        TelephoneNumber = telephoneNumber;
        SecondaryTelephoneNumber = secondaryTelephoneNumber;
        IsTermsAndConditionsAccepted = isTermsAndConditionsAccepted;
    }

    public string? FirstName { get; private set; }

    public string? Surname { get; private set; }

    public string? JobTitle { get; private set; }

    public string? Email { get; private set; }

    public string? TelephoneNumber { get; private set; }

    public string? SecondaryTelephoneNumber { get; private set; }

    public bool? IsTermsAndConditionsAccepted { get; private set; }

    public void ProvideUserDetails(
        string firstName,
        string surname,
        string jobTitle,
        string telephoneNumber,
        string secondaryTelephoneNumber,
        string userEmail,
        string isTermsAndConditionsAccepted)
    {
        FirstName = firstName;
        Surname = surname;
        JobTitle = jobTitle;
        Email = userEmail;
        TelephoneNumber = telephoneNumber;
        SecondaryTelephoneNumber = secondaryTelephoneNumber;
        IsTermsAndConditionsAccepted = isTermsAndConditionsAccepted == CommonResponse.Checked;
    }

    public bool IsProfileCompleted()
    {
        return !string.IsNullOrEmpty(FirstName) &&
                !string.IsNullOrEmpty(Surname) &&
                !string.IsNullOrEmpty(JobTitle) &&
                !string.IsNullOrEmpty(TelephoneNumber);
    }
}
