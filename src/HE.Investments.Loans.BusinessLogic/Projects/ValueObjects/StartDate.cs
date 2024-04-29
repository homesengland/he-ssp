using System.Globalization;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class StartDate : DateValueObject
{
    private const string FieldDescription = "project start date";

    public StartDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(StartDate), FieldDescription, !exists)
    {
    }

    public StartDate(DateTime? value)
        : base(value)
    {
    }

    public static StartDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
