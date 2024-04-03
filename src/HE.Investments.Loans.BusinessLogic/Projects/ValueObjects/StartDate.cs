using System.Globalization;
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
    private const string FieldDescription = "you plan to start the project";

    public StartDate(bool exists, string? day, string? month, string? year, string fieldName = nameof(StartDate))
        : base(day, month, year, nameof(StartDate), FieldDescription, !exists)
    {
        Exists = exists;
    }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
