using System.Globalization;
using FluentValidation;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.CompanyStructure;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;

public class CompanyStructureValidator : AbstractValidator<CompanyStructureViewModel>
{
    private readonly IAppConfig _appConfig;

    private readonly string[] _allowedExtensions = new string[]
    {
        AllowedFileExtension.PDF,
        AllowedFileExtension.DOC,
        AllowedFileExtension.DOCX,
        AllowedFileExtension.JPEG,
        AllowedFileExtension.JPG,
        AllowedFileExtension.RTF,
    };

    public CompanyStructureValidator(IAppConfig appConfig)
    {
        _appConfig = appConfig;

        RuleSet(CompanyStructureView.MoreInformationAboutOrganization, () =>
        {
            When(
                c => c.CompanyInfoFileName != null,
                () => RuleFor(e => e.CompanyInfoFile)
                    .Must(
                        e => e?.Length < _appConfig.MaxFileSizeInMegabytes * 1024 * 1024)
                    .WithMessage(string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, _appConfig.MaxFileSizeInMegabytes)));

            RuleFor(e => e.CompanyInfoFileName)
              .Must(
                        e => _allowedExtensions.Contains(Path.GetExtension(e?.ToLower(CultureInfo.InvariantCulture))))
                    .WithMessage(ValidationErrorMessage.FileIncorrectFormat)
                    .When(e => !string.IsNullOrEmpty(e.CompanyInfoFileName));

            When(
                item => item.ExistingCompany != null,
                () => RuleFor(item => item.ExistingCompany)
                    .Must(value => value!.Length <= MaximumInputLength.LongInput)
                    .WithMessage(ValidationErrorMessage.InputLongerThanThousandCharacters));
        });

        RuleSet(CompanyStructureView.HomesBuilt, () => When(
                item => item.HomesBuilt != null,
                () => RuleFor(item => item.HomesBuilt)
                        .Cascade(CascadeMode.Stop)
                        .Must(value =>
                        {
                            var culture = CultureInfo.InvariantCulture;
                            if (decimal.TryParse(value, NumberStyles.Float, culture, out var decimalResult))
                            {
                                if (decimalResult % 1 != 0)
                                {
                                    return false;
                                }
                            }

                            return true;
                        }).WithMessage(ValidationErrorMessage.HomesBuiltDecimalNumber)
                        .Must(value =>
                        {
                            if (!int.TryParse(value, out var intValue))
                            {
                                return false;
                            }

                            return true;
                        }).WithMessage(ValidationErrorMessage.HomesBuiltIncorretInput)
                        .Matches(@"^0$|^[1-9][0-9]*$")
                        .WithMessage(ValidationErrorMessage.HomesBuiltIncorrectNumber)
                        .Must(value =>
                        {
                            if (int.TryParse(value, out var intValue))
                            {
                                return intValue is >= 0 and <= 99999;
                            }

                            return true;
                        }).WithMessage(ValidationErrorMessage.HomesBuiltIncorrectNumber)));

        RuleSet(CompanyStructureView.CheckAnswers, () =>
        {
            RuleFor(item => item.CheckAnswers)
           .NotEmpty()
           .WithMessage(ValidationErrorMessage.SecurityCheckAnswers);

            When(item => item.CheckAnswers == CommonResponse.Yes, () => RuleFor(m => m).Must(x =>
                !string.IsNullOrEmpty(x.Purpose) &&
                !string.IsNullOrEmpty(x.ExistingCompany) &&
                !string.IsNullOrEmpty(x.HomesBuilt))
                .WithMessage(ValidationErrorMessage.CheckAnswersOption)
                .OverridePropertyName(nameof(CompanyStructureViewModel.CheckAnswers)));
        });
    }
}
