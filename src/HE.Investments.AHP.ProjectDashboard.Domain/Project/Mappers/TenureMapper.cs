using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Mappers;

internal sealed class TenureMapper : EnumMapper<Tenure>
{
    protected override IDictionary<Tenure, int?> Mapping => new Dictionary<Tenure, int?>
    {
        { Tenure.AffordableRent, (int)invln_Tenure.Affordablerent },
        { Tenure.SocialRent, (int)invln_Tenure.Socialrent },
        { Tenure.SharedOwnership, (int)invln_Tenure.Sharedownership },
        { Tenure.RentToBuy, (int)invln_Tenure.Renttobuy },
        { Tenure.HomeOwnershipLongTermDisabilities, (int)invln_Tenure.HOLD },
        { Tenure.OlderPersonsSharedOwnership, (int)invln_Tenure.OPSO },
    };
}
