using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.Account.Domain.Organisation.Mappers;

public class InvestmentPartnerStatusMapper : EnumMapper<InvestmentPartnerStatus>
{
    protected override IDictionary<InvestmentPartnerStatus, int?> Mapping =>
        new Dictionary<InvestmentPartnerStatus, int?>
        {
            { InvestmentPartnerStatus.InvestmentPartnerFull, (int)invln_ExternalStatusAHPOrganisation.InvestmentPartnerFull },
            { InvestmentPartnerStatus.InvestmentPartnerRestricted, (int)invln_ExternalStatusAHPOrganisation.InvestmentPartnerRestricted },
            { InvestmentPartnerStatus.NotAnInvestmentPartner, (int)invln_ExternalStatusAHPOrganisation.NotAnInvestmentPartner },
            { InvestmentPartnerStatus.Undefined, null },
        };

    protected override InvestmentPartnerStatus? ToDomainMissing => InvestmentPartnerStatus.Undefined;
}
