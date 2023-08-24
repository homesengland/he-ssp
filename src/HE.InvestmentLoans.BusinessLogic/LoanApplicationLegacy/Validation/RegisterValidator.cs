using FluentValidation;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.User;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;
public class RegisterValidator : AbstractValidator<UserDetailsViewModel>
{
    public RegisterValidator()
    {
        RuleSet(RegisterView.ProfileDetails, () =>
        {
            When(
               item => item.FirstName == null,
               () => RuleFor(item => item.FirstName)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterFirstName));

            When(
               item => item.Surname == null,
               () => RuleFor(item => item.Surname)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterSurname));

            When(
               item => item.JobTitle == null,
               () => RuleFor(item => item.JobTitle)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterJobTitle));

            When(
               item => item.TelephoneNumber == null,
               () => RuleFor(item => item.TelephoneNumber)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterTelephoneNumber));

            When(
              item => item.IsTermsAndConditionsAccepted == null,
              () => RuleFor(item => item.IsTermsAndConditionsAccepted)
                      .NotEmpty()
                      .WithMessage(ValidationErrorMessage.AcceptTermsAndConditions));
        });
    }
}
