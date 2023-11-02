using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dawn;
using HE.InvestmentLoans.Contract.Domain;
using Newtonsoft.Json.Linq;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;
public class LocalAuthorityId : ValueObject
{
    public LocalAuthorityId(string value)
    {
        Value = Guard.Argument(value, nameof(LocalAuthorityId)).NotNull();
    }

    public string Value { get; }

    public static LocalAuthorityId From(string value) => new(value);

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
