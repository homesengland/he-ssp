using FluentValidation;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.User;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;
public class UserValidator : AbstractValidator<UserDetailsViewModel>
{
    public UserValidator()
    {
        RuleSet(UserView.ProfileDetails, () =>
        {
            When(
               item => item.FirstName == null,
               () => RuleFor(item => item.FirstName)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterFirstName));

            When(
                item => item.FirstName != null,
                () => RuleFor(item => item.FirstName)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.InputLongerThanHundredCharacters));

            When(
               item => item.Surname == null,
               () => RuleFor(item => item.Surname)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterSurname));

            When(
                item => item.Surname != null,
                () => RuleFor(item => item.Surname)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.InputLongerThanHundredCharacters));

            When(
               item => item.JobTitle == null,
               () => RuleFor(item => item.JobTitle)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterJobTitle));

            When(
                item => item.JobTitle != null,
                () => RuleFor(item => item.JobTitle)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.InputLongerThanHundredCharacters));

            When(
               item => item.TelephoneNumber == null,
               () => RuleFor(item => item.TelephoneNumber)
                       .NotEmpty()
                       .WithMessage(ValidationErrorMessage.EnterTelephoneNumber));

            When(
                item => item.TelephoneNumber != null,
                () => RuleFor(item => item.TelephoneNumber)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.InputLongerThanHundredCharacters));

            When(
                item => item.SecondaryTelephoneNumber != null,
                () => RuleFor(item => item.SecondaryTelephoneNumber)
                    .Must(value => value!.Length <= MaximumInputLength.ShortInput)
                    .WithMessage(ValidationErrorMessage.InputLongerThanHundredCharacters));
        });
    }
}
