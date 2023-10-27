using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Account.Domain.User.ValueObjects;

namespace HE.Investments.Account.Domain.User.Entities;

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
        string? firstName,
        string? lastName,
        string? jobTitle,
        string? telephoneNumber,
        string? secondaryTelephoneNumber,
        string? userEmail)
    {
        OperationResult.New()
            .WithValidation(() => FirstName = new FirstName(firstName))
            .WithValidation(() => LastName = new LastName(lastName))
            .WithValidation(() => JobTitle = new JobTitle(jobTitle))
            .WithValidation(() => TelephoneNumber = new TelephoneNumber(telephoneNumber))
            .WithValidation(() => SecondaryTelephoneNumber = secondaryTelephoneNumber == null ? null : new SecondaryTelephoneNumber(secondaryTelephoneNumber))
            .CheckErrors();

        Email = userEmail;
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
