using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class ValidFileExtensionPolicy : IFilePolicy<FileName>
{
    private readonly string _fieldName;
    private readonly IAhpDocumentSettings _documentSettings;

    public ValidFileExtensionPolicy(string fieldName, IAhpDocumentSettings documentSettings)
    {
        _fieldName = fieldName;
        _documentSettings = documentSettings;
    }

    public void Apply(FileName value, OperationResult operationResult)
    {
        if (!_documentSettings.AllowedExtensions.Contains(value.Extension))
        {
            operationResult.Aggregate(() => throw new DomainValidationException(
                        _fieldName,
                        GenericValidationError
                            .InvalidFileType(value.Value, _documentSettings.AllowedExtensions
                                .Select(x => x.Value))));
        }
    }
}
