namespace HE.InvestmentLoans.BusinessLogic.User.Entities;
public class UserDetails
{
    public UserDetails(string firstName, string surname, string jobTitle, string email, string telephoneNumber, string secondaryTelephoneNumber)
    {
        FirstName = firstName;
        Surname = surname;
        JobTitle = jobTitle;
        Email = email;
        TelephoneNumber = telephoneNumber;
        SecondaryTelephoneNumber = secondaryTelephoneNumber;
    }

    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string JobTitle { get; set; }

    public string Email { get; set; }

    public string TelephoneNumber { get; set; }

    public string SecondaryTelephoneNumber { get; set; }

    public void ProvideUserDetails(string firstName, string surname, string jobTitle, string telephoneNumber, string secondaryTelephoneNumber, string userEmail)
    {
        FirstName = firstName;
        Surname = surname;
        JobTitle = jobTitle;
        Email = userEmail;
        TelephoneNumber = telephoneNumber;
        SecondaryTelephoneNumber = secondaryTelephoneNumber;
    }
}
