using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationTenureMapper
{
    private static readonly IDictionary<Tenure, invln_tenure> Tenures = new Dictionary<Tenure, invln_tenure>
    {
        { Tenure.AffordableRent, invln_tenure.Affordablerent },
        { Tenure.SocialRent, invln_tenure.Socialrent },
        { Tenure.SharedOwnership, invln_tenure.Sharedownership },
        { Tenure.RentToBuy, invln_tenure.Renttobuy },
        { Tenure.HomeOwnershipLongTermDisabilities, invln_tenure.HOLD },
        { Tenure.OlderPersonsSharedOwnership, invln_tenure.OPSO },
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

        var contract = (invln_tenure?)value;

        var tenure = Tenures.Where(t => t.Value == contract).Select(t => (Tenure?)t.Key).FirstOrDefault();
        if (tenure == null)
        {
            throw new ArgumentException($"Not supported Tenure value {value}");
        }

        return new ApplicationTenure(tenure.Value);
    }
}
