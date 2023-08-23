using HE.InvestmentLoans.Contract.User;

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

    public void ProvideUserDetails(UserDetailsViewModel userDetailsViewModel, string userEmail)
    {
        FirstName = userDetailsViewModel.FirstName;
        Surname = userDetailsViewModel.Surname;
        JobTitle = userDetailsViewModel.JobTitle;
        Email = userEmail;
        TelephoneNumber = userDetailsViewModel.TelephoneNumber;
        SecondaryTelephoneNumber = userDetailsViewModel.SecondaryTelephoneNumber;
    }
}
