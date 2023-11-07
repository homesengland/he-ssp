using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Shared.User;

public class UserDetails
{
    public UserDetails(
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

    public void ProvideUserDetails(
        string firstName,
        string lastName,
        string jobTitle,
        string telephoneNumber,
        string secondaryTelephoneNumber,
        string userEmail)
    {
        var operationResult = OperationResult.New();
        var firstNameResult = operationResult.CatchResult(() => FirstName.New(firstName));
        var lastNameResult = operationResult.CatchResult(() => LastName.New(lastName));
        var jobTitleResult = operationResult.CatchResult(() => JobTitle.New(jobTitle));
        var telephoneNumberResult = operationResult.CatchResult(() => TelephoneNumber.New(telephoneNumber));
        TelephoneNumber? secondaryTelephoneNumberResult = null;

        if (secondaryTelephoneNumber.IsProvided())
        {
            secondaryTelephoneNumberResult = operationResult.CatchResult(() => TelephoneNumber.New(secondaryTelephoneNumber, nameof(SecondaryTelephoneNumber)));
        }

        operationResult.CheckErrors();

        FirstName = firstNameResult;
        LastName = lastNameResult;
        JobTitle = jobTitleResult;
        Email = userEmail;
        TelephoneNumber = telephoneNumberResult;
        SecondaryTelephoneNumber = secondaryTelephoneNumberResult;
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
