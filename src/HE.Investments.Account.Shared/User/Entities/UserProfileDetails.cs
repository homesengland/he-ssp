using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Shared.User.Entities;

public class UserProfileDetails
{
    public UserProfileDetails(
        FirstName? firstName,
        LastName? lastName,
        JobTitle? jobTitle,
        string? email,
        TelephoneNumber? telephoneNumber,
        TelephoneNumber? secondaryTelephoneNumber,
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

    public TelephoneNumber? SecondaryTelephoneNumber { get; private set; }

    public bool? IsTermsAndConditionsAccepted { get; private set; }

    public void ProvideUserProfileDetails(
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
            .WithValidation(() => TelephoneNumber = TelephoneNumber.FromString(telephoneNumber, nameof(TelephoneNumber), "your preferred telephone number"))
            .WithValidation(() => SecondaryTelephoneNumber = secondaryTelephoneNumber.IsProvided()
                ? TelephoneNumber.FromString(secondaryTelephoneNumber, nameof(SecondaryTelephoneNumber), "your secondary telephone number")
                : null)
            .CheckErrors();

        Email = userEmail;
        IsTermsAndConditionsAccepted = true;
    }

    public bool IsCompleted()
    {
        return FirstName.IsProvided() &&
               LastName.IsProvided() &&
               JobTitle.IsProvided() &&
               TelephoneNumber.IsProvided() &&
               IsTermsAndConditionsAccepted == true;
    }
}
