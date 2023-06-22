using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Constants;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.IO;
using System.Linq;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation
{
    public class CompanyStructureValidator : AbstractValidator<CompanyStructureViewModel>
    {
        private string[] _allowedExtensions = new string[] { ".pdf", ".dic", ".jpeg", ".rtf" };

        public CompanyStructureValidator()
        {
            RuleSet("Purpose", () =>
            {
                RuleFor(e => e.Purpose)
                .NotEmpty()
                .WithMessage(ErrorMessages.RadioOption.ToString());
            });

            RuleSet("ExistingCompany", () =>
            {
                RuleFor(e => e.ExistingCompany)
                .NotEmpty()
                .WithMessage(ErrorMessages.EnterMoreDetails.ToString());

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
                        .WithMessage("The selected file must be a PDF, Word Doc, JPEG or RTF");

            });

            RuleSet("HomesBuilt", () =>
            {
                RuleFor(e => e.HomesBuilt)
                .NotEmpty()
                .WithMessage("The amount of homes your organisation has built must be a number")
                .Matches("^\\d{1,2}[.]\\d*$")
                .WithMessage("The number of homes your organisation has built in the past 3 years must be 99,999 or less")
                .Matches("^\\d{1,2}$")
                .WithMessage("The number of homes your organisation has built must be a whole number");
            });

            RuleSet("CheckAnswers", () =>
            {
                RuleFor(item => item.CheckAnswers)
                .NotEmpty()
                .WithMessage(ErrorMessages.CheckAnswersOption.ToString());
            });
        }
    }
}
