using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Globalization;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class CompanyStructureValidator : AbstractValidator<CompanyStructureViewModel>
    {
        private string[] _allowedExtensions = new string[] { ".pdf", ".doc", ".docx", ".jpeg", ".jpg", ".rtf" };

        public CompanyStructureValidator()
        {
            RuleSet("ExistingCompany", () =>
            {
                When(
                    c => c.CompanyInfoFileName != null,
                    () => RuleFor(e => e.CompanyInfoFile)
                        .Must(
                            e => e.Length < 20 * 1024 * 1024
                        )
                        .WithMessage(ErrorMessages.FileIncorrectSize.ToString())
                    );

                RuleFor(e => e.CompanyInfoFileName)
                  .Must(
                            e => _allowedExtensions.Contains(Path.GetExtension(e.ToLower()))
                        )
                        .WithMessage(ErrorMessages.FileIncorrectFormat.ToString())
                        .When(e => !string.IsNullOrEmpty(e.CompanyInfoFileName));
            });

            RuleSet("HomesBuilt", () =>
            {
                When(item => item.HomesBuilt != null,
                    () => RuleFor(item => item.HomesBuilt)
                            .Cascade(CascadeMode.Stop)
                            .Must(value =>
                            {
                                CultureInfo culture = CultureInfo.InvariantCulture;
                                if (decimal.TryParse(value, NumberStyles.Float, culture, out decimal decimalResult))
                                {
                                    if (decimalResult % 1 != 0)
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }).WithMessage(ErrorMessages.HomesBuiltDecimalNumber.ToString())
                            .Must(value =>
                            {
                                if (!int.TryParse(value, out var intValue))
                                {
                                    return false;
                                }
                                return true;
                            }).WithMessage(ErrorMessages.HomesBuiltIncorretInput.ToString())
                            .Matches(@"^0$|^[1-9][0-9]*$")
                            .WithMessage(ErrorMessages.HomesBuiltIncorrectNumber.ToString())
                            .Must(value =>
                            {
                                if (int.TryParse(value, out var intValue))
                                {
                                    return intValue >= 0 && intValue <= 99999;
                                }
                                return true;
                            }).WithMessage(ErrorMessages.HomesBuiltIncorrectNumber.ToString())
                    );
            });

            RuleSet("CheckAnswers", () =>
            {
                RuleFor(item => item.CheckAnswers)
               .NotEmpty()
               .WithMessage(ErrorMessages.SecurityCheckAnswers.ToString());

                When(item => item.CheckAnswers == "Yes", () =>
                {
                    RuleFor(m => m).Must(x => 
                    !string.IsNullOrEmpty(x.Purpose) &&
                    !string.IsNullOrEmpty(x.ExistingCompany) &&
                    !string.IsNullOrEmpty(x.HomesBuilt)
                    ).WithMessage(ErrorMessages.CheckAnswersOption.ToString());
                });
                
            });
        }
    }
}
