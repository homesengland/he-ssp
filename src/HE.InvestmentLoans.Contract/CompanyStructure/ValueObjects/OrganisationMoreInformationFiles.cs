using System.Globalization;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationFiles : ValueObject
{
    private readonly int _allowedFilesCount = 10;

    public OrganisationMoreInformationFiles(int? filesCount)
    {
        var operationResult = OperationResult.New();
        if (filesCount > _allowedFilesCount)
        {
            operationResult.AddValidationError(nameof(OrganisationMoreInformationFile), string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FilesMaxCount, _allowedFilesCount));
        }

        operationResult.CheckErrors();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return null;
    }
}
