using System.Collections.ObjectModel;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteProcurements : ValueObject, IQuestion
{
    public SiteProcurements(IList<SiteProcurement>? procurements = null)
    {
        if (procurements != null && procurements.Any(x => x == SiteProcurement.Other) && procurements.Count > 1)
        {
            OperationResult.New()
                .AddValidationError("SiteProcurements", ValidationErrorMessage.InvalidValue)
                .CheckErrors();
        }

        Procurements = new ReadOnlyCollection<SiteProcurement>(procurements ?? []);
    }

    public IReadOnlyCollection<SiteProcurement> Procurements { get; }

    public bool IsAnswered()
    {
        return Procurements.Count != 0;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Procurements;
    }
}
