using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class ValidFileExtensionPolicy : IFilePolicy<FileName>
{
    private static readonly IList<FileExtension> AllowedExtensions =
        new[] { new FileExtension("jpg"), new FileExtension("png"), new FileExtension("pdf"), new FileExtension("docx") };

    private readonly string _fieldName;

    public ValidFileExtensionPolicy(string fieldName)
    {
        _fieldName = fieldName;
    }

    public void Apply(FileName value)
    {
        if (!AllowedExtensions.Contains(value.Extension))
        {
            OperationResult.New()
                .AddValidationError(_fieldName, GenericValidationError.InvalidFileType(value.Value, AllowedExtensions.Select(x => x.Value)))
                .CheckErrors();
        }
    }
}
