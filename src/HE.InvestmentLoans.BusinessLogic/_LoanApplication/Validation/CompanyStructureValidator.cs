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
                .WithMessage("The amount of homes your organization has built must be a number")
                .Matches("^\\d*$")
                .WithMessage("The number of homes your organization has built must be a whole number")
                .DependentRules(() =>
                {
                    RuleFor(e => e.HomesBuilt)
                        .Must(value => int.Parse(value) <= 99999)
                        .WithMessage("The number of homes your organization has built in the past 3 years must be 99,999 or less");
                });
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
