using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public static class ApplicationTenureMapper
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

    public static int? ToDto(ApplicationTenure? value)
    {
        if (value == null)
        {
            return null;
        }

        if (!Tenures.TryGetValue(value.Value, out var tenure))
        {
            throw new ArgumentException($"Not supported Tenure value {value.Value}");
        }

        return (int)tenure;
    }

    public static ApplicationTenure? ToDomain(int? value)
    {
        if (value == null)
        {
            return null;
        }

        var contract = (invln_Tenure?)value;

        var tenure = Tenures.Where(t => t.Value == contract).Select(t => (Tenure?)t.Key).FirstOrDefault() ?? throw new ArgumentException($"Not supported Tenure value {value}");

        return new ApplicationTenure(tenure);
    }
}
