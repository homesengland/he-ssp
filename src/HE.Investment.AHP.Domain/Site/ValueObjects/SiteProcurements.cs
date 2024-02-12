using System.Collections.ObjectModel;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteProcurements : ValueObject, IQuestion
{
    public SiteProcurements(IList<SiteProcurement>? procurements = null)
    {
        Procurements = new ReadOnlyCollection<SiteProcurement>(procurements ?? new List<SiteProcurement>());
    }

    public IReadOnlyCollection<SiteProcurement> Procurements { get; }

    public bool IsAnswered()
    {
        return Procurements.Any();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Procurements;
    }
}
