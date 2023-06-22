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
                    c => c.CompanyInfoFile != null,
                    () => RuleFor(e => e.CompanyInfoFile)
                        .Must(
                            e => e.Length < 20 * 1024 * 1024
                        )
                        .WithMessage("The selected file must be smaller than or equal to 20MB")
                        .Must(
                            e => _allowedExtensions.Contains(Path.GetExtension(e.Name))
                        )
                        .WithMessage("The selected file must be a PDF, Word Doc, JPEG or RTF")
                    );
            });

            RuleSet("HomesBuilt", () =>
            {
                RuleFor(c => c.HomesBuilt)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("The amount of homes your organisation has built must be a number")
                    .Must(value =>
                    {
                        if ((decimal.TryParse(value.Replace('.', ','), out decimal result) || decimal.TryParse(value.Replace(',', '.'), out result)) && result != Math.Floor(result))
                        {
                            return false;
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
                            return value.Length <= 5 && intValue <= 99999;
                        }
                        return true;
                    }).WithMessage("The number of homes your organisation has built in the past 3 years must be 99,999 or less");
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
