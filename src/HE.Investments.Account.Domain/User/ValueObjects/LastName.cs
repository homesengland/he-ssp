using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;
using HE.InvestmentLoans.Contract.User;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class LastName : RequiredStringValueObject
{
    public LastName(string? value)
        : base(value, nameof(LastName), "your last name", MaximumInputLength.ShortInput)
    {
    }
}
