using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.Common.CRM.Model;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;

public static class AllocationTenureMapper
{
    private static readonly Dictionary<Tenure, invln_Tenure> Tenures = new()
    {
        { Tenure.AffordableRent, invln_Tenure.Affordablerent },
        { Tenure.SocialRent, invln_Tenure.Socialrent },
        { Tenure.SharedOwnership, invln_Tenure.Sharedownership },
        { Tenure.RentToBuy, invln_Tenure.Renttobuy },
        { Tenure.HomeOwnershipLongTermDisabilities, invln_Tenure.HOLD },
        { Tenure.OlderPersonsSharedOwnership, invln_Tenure.OPSO },
    };

    public static AllocationTenure ToDomain(int? value)
    {
        var contract = (invln_Tenure?)value;

        var tenure = Tenures.Where(t => t.Value == contract).Select(t => (Tenure?)t.Key).FirstOrDefault() ??
                     throw new ArgumentException($"Not supported Tenure value {value}");

        return new AllocationTenure(tenure);
    }
}
