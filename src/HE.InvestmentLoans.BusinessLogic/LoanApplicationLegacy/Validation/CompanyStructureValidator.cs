using System.Globalization;
using FluentValidation;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils.Constants;
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
        });
    }
}
