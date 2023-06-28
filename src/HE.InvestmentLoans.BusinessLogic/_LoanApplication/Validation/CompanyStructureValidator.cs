using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class CompanyStructureValidator : AbstractValidator<CompanyStructureViewModel>
    {
        private string[] _allowedExtensions = new string[] { ".pdf", ".dic", ".jpeg", ".rtf" };

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
                        .WithMessage("The selected file must be smaller than or equal to 20MB")
                    );

                RuleFor(e => e.CompanyInfoFileName)
                  .Must(
                            e => _allowedExtensions.Contains(Path.GetExtension(e.ToLower()))
                        )
                        .WithMessage("The selected file must be a PDF, Word Doc, JPEG or RTF")
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
                            }).WithMessage("The number of homes your organisation has built must be a whole number")
                            .Must(value =>
                            {
                                if (!int.TryParse(value, out var intValue))
                                {
                                    return false;
                                }
                                return true;
                            }).WithMessage("The amount of homes your organisation has built must be a number")
                            .Must(value =>
                            {
                                if (int.TryParse(value, out var intValue))
                                {
                                    return value.Length <= 5 && intValue >= 0 && intValue <= 99999;
                                }
                                return true;
                            }).WithMessage("The number of homes your organisation has built in the past 3 years must be 99,999 or less")
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
