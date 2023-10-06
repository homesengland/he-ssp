using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class ProjectType : ValueObject
{
    public ProjectType(string? value)
    {
        Value = value;
    }

    public static ProjectType Default => new(string.Empty);

    public string? Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value ?? null!;
    }
}
