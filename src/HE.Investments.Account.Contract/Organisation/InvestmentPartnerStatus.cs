using System.ComponentModel;

namespace HE.Investments.Account.Contract.Organisation;

public enum InvestmentPartnerStatus
{
    Undefined = 0,

    [Description("has full Investment Partner status")]
    InvestmentPartnerFull,

    [Description("is a restricted Investment Partner")]
    InvestmentPartnerRestricted,

    [Description("is not an Investment Partner")]
    NotAnInvestmentPartner,
}
