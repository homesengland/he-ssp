using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User.Entities;
public class UserDetails
{
    public UserDetails(
        FirstName? firstName,
        LastName? lastName,
        JobTitle? jobTitle,
        string? email,
        TelephoneNumber? telephoneNumber,
        SecondaryTelephoneNumber? secondaryTelephoneNumber,
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

    public FirstName? FirstName { get; private set; }

    public LastName? LastName { get; private set; }

    public JobTitle? JobTitle { get; private set; }

    public string? Email { get; private set; }

    public TelephoneNumber? TelephoneNumber { get; private set; }

    public SecondaryTelephoneNumber? SecondaryTelephoneNumber { get; private set; }

    public bool? IsTermsAndConditionsAccepted { get; private set; }

    public void ProvideUserDetails(
        FirstName firstName,
        LastName lastName,
        JobTitle jobTitle,
        TelephoneNumber telephoneNumber,
        SecondaryTelephoneNumber secondaryTelephoneNumber,
        string userEmail)
    {
        OperationResult.New().AddErrorsFromValueObject(firstName.Error, lastName.Error, jobTitle.Error, telephoneNumber.Error, secondaryTelephoneNumber.Error)
            .CheckErrors();

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
        return FirstName.IsProvided() &&
                LastName.IsProvided() &&
                JobTitle.IsProvided() &&
                TelephoneNumber.IsProvided() &&
                IsTermsAndConditionsAccepted == true;
    }
}
