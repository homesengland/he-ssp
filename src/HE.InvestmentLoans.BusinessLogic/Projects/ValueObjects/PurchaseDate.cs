using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class PurchaseDate
{
    public PurchaseDate(ProjectDate date, DateTime now)
    {
        if (date.Value.IsAfter(now))
        {
            OperationResult.ThrowValidationError("PurchaseDay", ValidationErrorMessage.FuturePurchaseDate);
        }

        Date = date;
    }

    public ProjectDate Date { get; private set; }

    public static PurchaseDate FromString(string year, string month, string day, DateTime now)
    {
        var operationResult = OperationResult.ResultOf(() => ProjectDate.FromString(year, month, day), "PurchaseDay");

        operationResult.CheckErrors();

        return new PurchaseDate(operationResult.ReturnedData, now);
    }

    internal DateTime AsDateTime()
    {
        return Date.Value;
    }
}
