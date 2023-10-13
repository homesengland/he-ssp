using System.Globalization;
using System.Threading;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;
using HE.Investments.DocumentService.Models.File;
using MediatR;
using Microsoft.AspNetCore.Http;

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
