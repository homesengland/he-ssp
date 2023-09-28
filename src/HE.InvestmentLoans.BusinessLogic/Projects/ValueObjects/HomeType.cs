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
public class HomeType : ValueObject
{
    public HomeType(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomeType), ValidationErrorMessage.EmptyHomesCount)
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    public static HomeType From(string homeType)
    {
        if (homeType.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(HomesCount), ValidationErrorMessage.EnterFirstName)
                .CheckErrors();
        }

        return new HomeType(homeType);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
