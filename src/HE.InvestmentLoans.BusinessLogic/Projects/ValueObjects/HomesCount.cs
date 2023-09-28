using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class HomesCount : ValueObject
{
    public HomesCount(int value)
    {
        if (value.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesCount), ValidationErrorMessage.EmptyHomesCount)
                .CheckErrors();
        }

        if (value > ProjectFormOption.MinHomesCountRequired)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesCount), ValidationErrorMessage.NotEnoughHomesCount)
                .CheckErrors();
        }

        Value = value;
    }

    public HomesCount(string value)
    {
        Value = HomesCount.From(value).Value;
    }

    public static HomesCount Default => new(0);

    public int Value { get; }

    public static HomesCount From(string homesCount)
    {
        var homesCountInt = homesCount.TryParseNullableInt();

        if (!homesCountInt.HasValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(HomesCount), ValidationErrorMessage.EmptyHomesCount)
                .CheckErrors();
        }

        if (homesCountInt is < 1 or > 9999)
        {
            OperationResult.New()
                .AddValidationError(nameof(HomesCount), ValidationErrorMessage.ManyHomesAmount)
                .CheckErrors();
        }

        return new HomesCount(homesCountInt ?? 0);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
